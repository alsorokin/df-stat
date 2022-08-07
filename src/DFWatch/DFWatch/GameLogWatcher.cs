using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Watch;

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
        }

        private void ReadNext(FileStream fs, List<byte> buffer)
        {
            while (fs.Position < fs.Length)
            {
                byte readByte = (byte)fs.ReadByte();
                if (readByte == 0x0A && buffer.LastOrDefault() == 0x0D)
                {
                    buffer.RemoveAt(buffer.Count - 1);
                    string line = Encoding.UTF8.GetString(buffer.ToArray());
                    HandleLine(line);
                    buffer.Clear();
                }
                else
                {
                    if (LineHelper.AsciiToUnicodeReplacements.TryGetValue(readByte, out byte[] replacement))
                    {
                        buffer.AddRange(replacement);
                    }
                    else
                    {
                        buffer.Add(readByte);
                    }
                }
            }
        }

        private FileStream CreateFileStream()
        {
            FileStream fs = new(GameLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return fs;
        }

        public void StartWatching(bool seekToEnd = true)
        {
            GameLogFileWatcher = new(GameLogDirectory, GameLogFileName);
            FileStream fs = CreateFileStream();
            List<byte> buffer = new List<byte>();
            if (seekToEnd)
            {
                fs.Seek(0, SeekOrigin.End);
            }
            else
            {
                ReadNext(fs, buffer);
            }

            GameLogFileWatcher.Changed += (_, __) =>
            {
                ReadNext(fs, buffer);
            };

            GameLogFileWatcher.Disposed += (_, __) =>
            {
                fs.Close();
            };

            GameLogFileWatcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            GameLogFileWatcher.EnableRaisingEvents = false;
            GameLogFileWatcher.Dispose();
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
