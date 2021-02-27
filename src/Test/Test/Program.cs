using Snay.DFStat.Watch;
using System;
using System.IO;

namespace Snay.DFStat.Test
{
    class Program
    {
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
            Console.ResetColor();
            switch (args.LnType)
            {
                case LineType.Combat:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LineType.DFHack:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LineType.AnnouncementGood:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LineType.AnnouncementBad:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.WriteLine($"({args.LnType}) {args.LnText}");
            Console.ResetColor();
        }
    }
}
