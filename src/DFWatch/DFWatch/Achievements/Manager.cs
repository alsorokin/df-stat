using Snay.DFStat.Watch;
using Snay.DFStat.Watch.Achievements;
using System;
using System.Text.RegularExpressions;

namespace Watch.Achievements
{
    class Manager : Achievement
    {
        public override string Description =>
            $"Create/improve/dye {ProgressNeeded[Stage < MaxStage ? Stage + 1 : MaxStage]} items using work orders.";

        public override string Name =>
            "Manager";

        protected override int[] ProgressNeeded => progressNeeded;

        private readonly int[] progressNeeded =
        {
            0,     // Achievement locked
            2,     // Stage 1
            4,     // Stage 2
            8,     // Stage 3
            16,    // Stage 4
            32,    // Stage 5
            64,    // Stage 6
            128,   // Stage 7
            256,   // Stage 8
            512,   // Stage 9
            1024,  // Stage 10
            2048,  // Stage 11
            4096,  // Stage 12
            8192,  // Stage 13
            16386, // Stage 14
            32786, // Stage 15
            65536, // Stage 16
        };

        public Manager(GameLogWatcher watcher) : base(watcher)
        {
            watcher.LineAdded += HandleLineAdded;
        }

        private void HandleLineAdded(object sender, Line line)
        {
            if (line.LnType == LineType.Order)
            {
                Match match = Regex.Match(line.Text, LineHelper.PatternMappings[LineType.Order][0]);
                string orderCountString = match.Groups[1].Value;
                if (int.TryParse(orderCountString, out int orderCount))
                {
                    Progress += orderCount;
                }
            }
        }
    }
}
