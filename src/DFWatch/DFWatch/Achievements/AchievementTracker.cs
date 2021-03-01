using System.Collections.Generic;
using Watch.Achievements;

namespace Snay.DFStat.Watch.Achievements
{
    public class AchievementTracker
    {
        private readonly GameLogWatcher watcher;

        public readonly List<Achievement> Achievements;

        public event NewStageUnlockedHandler NewStageUnlocked;
        public event ProgressChangedHandler ProgressChanged;

        public AchievementTracker(GameLogWatcher watcher)
        {
            this.watcher = watcher;

            this.Achievements = new()
            {
                new Butcher(watcher),
            };

            foreach (Achievement achievement in this.Achievements)
            {
                achievement.NewStageUnlocked += HandleNewStageUnlocked;
                achievement.ProgressChanged += HandleProgressChanged;
            }

        }

        private void HandleProgressChanged(Achievement sender)
        {
            ProgressChanged?.Invoke(sender);
        }

        private void HandleNewStageUnlocked(Achievement sender)
        {
            NewStageUnlocked?.Invoke(sender);
        }
    }
}
