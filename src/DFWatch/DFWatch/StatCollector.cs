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

        private void HandleLineAdded(object sender, LineAddedArgs e)
        {
            if (!lineTypeCounts.ContainsKey(e.LnType))
                lineTypeCounts.Add(e.LnType, 0);

            lineTypeCounts[e.LnType] += 1;
        }
    }
}
