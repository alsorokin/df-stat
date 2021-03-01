using Snay.DFStat.Watch;
using Snay.DFStat.Watch.Achievements;

namespace Watch.Achievements
{
    class Butcher : Achievement
    {
        public override string Description =>
            $"Butcher {ProgressNeeded[Stage < MaxStage ? Stage + 1 : MaxStage]} animal{(Stage > 0 ? "s" : "")}.";

        public override string Name =>
            "Butcher of the Mountainhomes";

        protected override int[] ProgressNeeded => progressNeeded;

        private readonly int[] progressNeeded =
        {
            0,     // Achievement locked
            1,     // Stage 1
            10,    // Stage 2
            100,   // Stage 3
            1000,  // Stage 4
            10000, // Stage 5
        };

        public Butcher(GameLogWatcher watcher) : base(watcher)
        {
            watcher.LineAdded += HandleLineAdded;
        }

        private void HandleLineAdded(object sender, LineAddedArgs e)
        {
            if (e.LnType == LineType.Slaughter)
            {
                Progress += 1;
            }
        }
    }
}
