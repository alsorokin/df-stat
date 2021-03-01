using Snay.DFStat.Watch;
using Snay.DFStat.Watch.Achievements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Console = Snay.DFstat.Test.Console;

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
                    HandleLineAdded(args);
                };

                StatCollector collector = new(watcher);
                AchievementTracker tracker = new(watcher);
                tracker.NewStageUnlocked += WriteNewStageUnlocked;
                tracker.ProgressChanged += WriteProgressChanged;

                //watcher.StartWatching();
                watcher.ScanOnce();
                WriteStats(collector);
                WriteAchievements(tracker);
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }

        private static void WriteNewStageUnlocked(Achievement sender)
        {
            ConsoleColor fore = ConsoleColor.Yellow;
            ConsoleColor back = ConsoleColor.DarkMagenta;
            Console.WriteLine();
            WriteColoredLine($"Unlocked {sender.Name}, stage {sender.Stage}!", fore, back);
            WriteColoredLine($"Progress to next stage: {sender.Progress} / {sender.MaxProgress}", fore, back);
            Console.WriteLine();
        }

        private static void WriteProgressChanged(Achievement sender)
        {
            ConsoleColor fore = ConsoleColor.Yellow;
            ConsoleColor back = ConsoleColor.DarkMagenta;
            Console.WriteLine();
            WriteColoredLine($"Progress changed for {sender.Name}, stage {sender.Stage}!", fore, back);
            WriteColoredLine($"Progress: {sender.Progress} / {sender.MaxProgress}", fore, back);
            Console.WriteLine();
        }

        private static void WriteStats(StatCollector collector)
        {
            int longestTypeName = ((IEnumerable<LineType>)Enum.GetValues(typeof(LineType))).Max(lt => lt.ToString().Length);
            ResetAndWriteLine();
            Console.WriteLine("###   Line stats   ###");
            foreach (KeyValuePair<LineType, int> line in collector.LineTypeCounts.OrderByDescending(kv => kv.Value))
            {

                string spaces = string.Empty;
                int spacesToAdd = longestTypeName - line.Key.ToString().Length;
                for (int i = 0; i < spacesToAdd; i++)
                    spaces += " ";

                Console.WriteLine($"{line.Key}:{spaces} {line.Value}");
            }

            string spacesForTotal = string.Empty;
            int SpacesToAdd = longestTypeName - 5; // "Total".Length == 5
            for (int i = 0; i < SpacesToAdd; i++)
                spacesForTotal += " ";

            Console.WriteLine();
            Console.WriteLine($"Total: {spacesForTotal}{collector.TotalLinesProcessed}");
        }

        private static void WriteAchievements(AchievementTracker tracker)
        {
            Console.WriteLine();
            Console.Write("###   Achievement stats   ###");
            tracker.Achievements.ForEach(WriteAchievement);
            Console.WriteLine();
        }

        private static void WriteAchievement(Achievement a)
        {
            ConsoleColor fore = ConsoleColor.DarkMagenta;
            ConsoleColor back = ConsoleColor.Cyan;
            WriteColoredLine($"Name: {a.Name}", fore, back);
            WriteColoredLine($"Description: {a.Description}", fore, back);
            WriteColoredLine($"Stage: {a.Stage} / {a.MaxStage}", fore, back);
            WriteColoredLine($"Progress: {a.Progress} / {a.MaxProgress}", fore, back);
        }

        private static void HandleLineAdded(LineAddedArgs args)
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
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
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
            ResetAndWriteLine();
        }

        private static void WriteColoredLine(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Console.ResetColor();
            if (foregroundColor != null)
                Console.ForegroundColor = foregroundColor.Value;
            if (backgroundColor != null)
                Console.BackgroundColor = backgroundColor.Value;

            Console.Write(text);
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void ResetAndWriteLine()
        {
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
