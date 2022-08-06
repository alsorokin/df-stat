using System.Linq;
using System.Text.RegularExpressions;

namespace Snay.DFStat.Watch.Achievements
{
    class Adventurer : Achievement
    {
        public override string Description =>
            $"Take an arrow or shoot someone else in the knee{(Stage > 0 ? " " + MaxProgress + " times" : string.Empty)}.";

        public override string Name =>
            "Adventurer";

        protected override int[] ProgressNeededPerStage => progressNeededPerStage;

        private readonly int[] progressNeededPerStage =
        {
            0,     // Achievement locked
            1,     // Stage 1
            100,   // Stage 2
        };

        public Adventurer(GameLogWatcher watcher) : base(watcher)
        {
            watcher.LineAdded += HandleLineAdded;
        }

        private void HandleLineAdded(object sender, Line line)
        {
            if (line.LnType == LineType.Combat)
            {
                if (Regex.IsMatch(line.Text, LineHelper.KneeHurtPattern))
                {
                    // A knee has been hurt!
                    // Let's look back a few lines to see if this has been because of an arrow/bolt
                    if (watcher.RecentLines.TakeLast(4).Any(l => Regex.IsMatch(l.Text, LineHelper.BoltInTheLowerLegPattern)))
                    {
                        // If any of the recent 4 lines is about a bolt hitting a lower leg, assume achievement completed
                        // A better way would be to exclude the possibility of something like the following:
                        // 1. A bolt hits a lower leg
                        // 2. Someone hits another lower leg with a mace
                        // 3. A knee is hurt, but not because of the bolt!
                        // Ideally, we need to iterate from 3 to 1 and break on any melee message
                        // TODO: Return to this when we have something like melee/ranged combat tags for lines
                        Progress += 1;
                    }
                }
            }
        }
    }
}
