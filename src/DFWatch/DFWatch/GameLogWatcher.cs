using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Snay.DFStat.Watch
{
    public class GameLogWatcher
    {
        protected const string GameLogFileName = "gamelog.txt";

        public string GameLogFilePath { get; protected set; }
        public string GameLogDirectory { get => Path.GetDirectoryName(GameLogFilePath); }

        public Queue<Line> RecentLines;

        private const int MaxRecentLinesCount = 5;

        private string lastLine;

        private FileSystemWatcher GameLogFileWatcher { get; set; }

        public GameLogWatcher(string workingDir = null)
        {
            GameLogFilePath = GetGameLogPath(!string.IsNullOrEmpty(workingDir) ? workingDir : Directory.GetCurrentDirectory());
            RecentLines = new();
            GameLogFileWatcher = new(GameLogDirectory, GameLogFileName);
        }

        public void StartWatchingOld()
        {
            FileSystemWatcher gameLogWatcher = new FileSystemWatcher(GameLogDirectory, GameLogFileName);
            var wh = new AutoResetEvent(false);
            gameLogWatcher.Changed += (s, e) => wh.Set();
            gameLogWatcher.EnableRaisingEvents = true;

            var fs = new FileStream(GameLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var sr = new StreamReader(fs))
            {
                var s = string.Empty;
                while (true)
                {
                    s = sr.ReadLine();
                    if (s != null)
                        HandleLine(s);
                    else
                        wh.WaitOne(1000);
                }
            }
            // ??
            // wh.Close();
        }

        public void StartWatching()
        {
            FileStream fs = new(GameLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new(fs);

            GameLogFileWatcher.Changed += (_, __) =>
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s != null)
                    {
                        HandleLine(s);
                    }
                }
            };

            GameLogFileWatcher.EnableRaisingEvents = true;
        }

        public void ScanOnce()
        {
            foreach (string line in File.ReadAllLines(this.GameLogFilePath))
            {
                if (line != null)
                {
                    HandleLine(line);
                }
            }
        }

        public delegate void LineAddedHandler(object sender, Line line);
        public event LineAddedHandler LineAdded;

        protected void HandleLine(string text)
        {
            List<string> traits = new();
            LineType type = LineType.General;
            bool isRepeatedLine = Regex.IsMatch(text, LineHelper.RepeatedLinePattern);
            if (isRepeatedLine)
                text = lastLine;

            foreach (KeyValuePair<LineType, string[]> mapping in LineHelper.PatternMappings)
            {
                if (mapping.Value.Any(l => Regex.IsMatch(text, l)))
                {
                    type = mapping.Key;
                    break;
                }
            }

            foreach (LineTraitMapping mapping in LineHelper.LineTraitMappings.Where(m => m.LnType == type))
            {
                foreach (string pattern in mapping.Patterns)
                {
                    if (Regex.IsMatch(text, pattern))
                        traits.Add(mapping.Trait);
                }
            }

            if (!isRepeatedLine)
                lastLine = text;
            Line line = new(type, text, traits);
            RecentLines.Enqueue(line);
            if (RecentLines.Count() > MaxRecentLinesCount)
                RecentLines.Dequeue();
            LineAdded?.Invoke(this, line);
        }

        protected static string GetGameLogPath(string workingDir)
        {
            string currentDir = workingDir;
            do
            {
                string file = Directory.GetFiles(currentDir).FirstOrDefault(f =>
                    string.Equals(Path.GetFileName(f), GameLogFileName));

                if (!string.IsNullOrEmpty(file))
                {
                    // Calling GetFullPath to normalize the directory separators in path so that they look pretty
                    return Path.GetFullPath(file);
                }

                currentDir = Directory.GetParent(currentDir).FullName;
            }
            while (Directory.GetParent(currentDir) != null);

            throw new FileNotFoundException("Could not find game log file.");
        }
    }
}
