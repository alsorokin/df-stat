using Snay.DFStat.Watch;
using Snay.DFStat.Watch.Achievements;

namespace Watch.Achievements
{
    class Craftsdwarf : Achievement
    {
        public override string Description =>
            $"Build, craft or improve {ProgressNeededPerStage[Stage < MaxStage ? Stage + 1 : MaxStage]} masterpiece item{(Stage > 0 ? "s" : "")}.";

        public override string Name =>
            "Craftsdwarf";

        protected override int[] ProgressNeededPerStage => progressNeeded;

        private readonly int[] progressNeeded =
        {
            0,     // Achievement locked
            1,     // Stage 1
            10,    // Stage 2
            100,   // Stage 3
            1000,  // Stage 4
            10000, // Stage 5
        };

        public Craftsdwarf(GameLogWatcher watcher) : base(watcher)
        {
            watcher.LineAdded += HandleLineAdded;
        }

        private void HandleLineAdded(object sender, Line e)
        {
            if (e.LnType == LineType.Masterpiece)
            {
                Progress += 1;
            }
        }
    }
}
