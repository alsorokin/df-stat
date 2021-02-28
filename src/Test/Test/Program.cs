using Snay.DFStat.Watch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Snay.DFStat.Test
{
    class Program
    {
        private static Dictionary<LineType, bool> LineTypeFilter = new()
        {
            { LineType.General,          true  },
            { LineType.Combat,           false },
            { LineType.DFHack,           false },
            { LineType.Merchant,         true  },
            { LineType.ForgottenBeast,   true  },
            { LineType.JobCancellation,  true  },
        };

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                GameLogWatcher watcher = new("C:/Games/Dwarf Fortress");
                Console.WriteLine($"Found gamelog: {watcher.GameLogFilePath}");
                watcher.LineAdded += (sender, args) =>
                {
                    WriteLine(args);
                };

                StatCollector collector = new(watcher);

                watcher.StartWatching();
                //watcher.ScanOnce();
                //WriteStats(collector);
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }

        private static void WriteStats(StatCollector collector)
        {
            int longestType = ((IEnumerable<LineType>)Enum.GetValues(typeof(LineType))).Max(lt => lt.ToString().Length);
            Console.ResetColor();
            Console.WriteLine("Stats:");
            foreach (LineType lnType in Enum.GetValues(typeof(LineType)))
            {
                string spaces = string.Empty;
                int spacesToAdd = longestType - lnType.ToString().Length;
                for (int i = 0; i < spacesToAdd; i++)
                    spaces += " ";

                if (!collector.LineTypeCounts.ContainsKey(lnType))
                    continue;

                Console.WriteLine($"{lnType}:{spaces} {collector.LineTypeCounts[lnType]}");
            }

            string spacesForTotal = string.Empty;
            int SpacesToAdd = longestType - 5; // "Total".Length == 5
            for (int i = 0; i < SpacesToAdd; i++)
                spacesForTotal += " ";

            Console.WriteLine();
            Console.WriteLine($"Total: {spacesForTotal}{collector.TotalLinesProcessed}");
        }

        private static void WriteLine(LineAddedArgs args)
        {
            if (LineTypeFilter.ContainsKey(args.LnType) && !LineTypeFilter[args.LnType])
                return;

            Console.ResetColor();
            switch (args.LnType)
            {
                case LineType.Combat:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LineType.Occupation:
                case LineType.DFHack:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LineType.War:
                case LineType.ForgottenBeast:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case LineType.StuffBreaking:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LineType.Order:
                case LineType.Masterpiece:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LineType.Mandate:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LineType.Merchant:
                case LineType.StrangeMood:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LineType.JobCancellation:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LineType.BirthDwarf:
                case LineType.GrowthDwarf:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case LineType.BirthAnimal:
                case LineType.GrowthAnimal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LineType.Slaughter:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
            }

            Console.Write($"({args.LnType}) {args.LnText}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
