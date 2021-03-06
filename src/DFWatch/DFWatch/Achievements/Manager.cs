using Snay.DFStat.Watch;
using Snay.DFStat.Watch.Achievements;
using System.Text.RegularExpressions;

namespace Watch.Achievements
{
    class Manager : Achievement
    {
        public override string Description =>
            $"Create/improve/dye {ProgressNeededPerStage[Stage < MaxStage ? Stage + 1 : MaxStage]} items using work orders.";

        public override string Name =>
            "Manager";

        protected override int[] ProgressNeededPerStage => progressNeeded;

        private readonly int[] progressNeeded =
        {
            0,     // Achievement locked
            128,   // Stage 1
            256,   // Stage 2
            512,   // Stage 3
            1024,  // Stage 4
            2048,  // Stage 5
            4096,  // Stage 6
            8192,  // Stage 7
            16386, // Stage 8
            32786, // Stage 9
            65536, // Stage 10
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
