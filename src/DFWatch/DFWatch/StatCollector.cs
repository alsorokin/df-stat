using System.Collections.Generic;
using System.Linq;

namespace Snay.DFStat.Watch
{
    public class StatCollector
    {
        public IReadOnlyDictionary<LineType, int> LineTypeCounts
        {
            get => lineTypeCounts;
        }

        public int TotalLinesProcessed { get => LineTypeCounts.Values.Sum(); }

        private readonly Dictionary<LineType, int> lineTypeCounts = new();
        
        private GameLogWatcher watcher;

        public StatCollector(GameLogWatcher watcher)
        {
            this.watcher = watcher;
            watcher.LineAdded += HandleLineAdded;
        }

        private void HandleLineAdded(object sender, Line line)
        {
            if (!lineTypeCounts.ContainsKey(line.LnType))
                lineTypeCounts.Add(line.LnType, 0);

            lineTypeCounts[line.LnType] += 1;
        }
    }
}
