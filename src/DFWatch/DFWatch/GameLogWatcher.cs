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

        private string lastLine;

        public GameLogWatcher(string workingDir = null)
        {
            GameLogFilePath = GetGameLogPath(!string.IsNullOrEmpty(workingDir) ? workingDir : Directory.GetCurrentDirectory());
        }

        public void StartWatching()
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

        public delegate void LineAddedHandler(object sender, LineAddedArgs e);
        public event LineAddedHandler LineAdded;

        protected void HandleLine(string line)
        {
            bool isRepeatedLine = Regex.IsMatch(line, LineHelper.RepeatedLinePattern);
            if (isRepeatedLine)
                line = lastLine;

            LineType type = LineType.General;

            foreach (KeyValuePair<LineType, string[]> mapping in LineHelper.PatternMappings)
            {
                if (mapping.Value.Any(l => Regex.IsMatch(line, l)))
                    type = mapping.Key;
            }

            if (!isRepeatedLine)
                lastLine = line;
            LineAdded?.Invoke(this, new LineAddedArgs(line, type));
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
