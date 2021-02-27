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
                watcher.LineAdded += (sender, args) => Console.WriteLine(args.LineText);
                watcher.StartWatching();
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}
