using Snay.DFStat.Watch;
using System;
using System.Collections.Generic;
using System.IO;

namespace Snay.DFStat.Test
{
    class Program
    {
        static Dictionary<LineType, bool> LineTypeFilter = new()
        {
            { LineType.General,          true  },
            { LineType.Combat,           false },
            { LineType.DFHack,           false },
            { LineType.Merchant,         true  },
            { LineType.ForgottenBeast,   true  },
            { LineType.JobCancellation,  false },
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                var watcher = new GameLogWatcher("C:/Games/Dwarf Fortress");
                Console.WriteLine($"Found gamelog: {watcher.GameLogFilePath}");
                watcher.LineAdded += (sender, args) =>
                {
                    WriteLine(args);
                };
                watcher.StartWatching();
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }

        static void WriteLine(LineAddedArgs args)
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
                case LineType.GrowthDwarf:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case LineType.GrowthAnimal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            Console.Write($"({args.LnType}) {args.LnText}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
