using System;
using System.Linq;

namespace Snay.DFStat.Watch.Achievements
{
    public delegate void NewStageUnlockedHandler(Achievement sender);
    public delegate void ProgressChangedHandler(Achievement sender);

    public abstract class Achievement
    {
        public abstract string Description { get; }

        public abstract string Name { get; }

        public virtual int Stage =>
            Array.IndexOf(ProgressNeeded, ProgressNeeded.Last(pn => pn <= Progress));

        public virtual int MaxStage => ProgressNeeded.Length - 1;

        private int progress;
        public virtual int Progress 
        {
            get => progress; 
            protected set
            {
                // Remember old values for max progress and stage because
                // incrementing progress will automatically increase these values
                // when new stage is about to be unlocked
                int oldMaxProgress = MaxProgress;
                int oldStage = Stage;
                progress = value;
                OnProgress();

                if (value >= oldMaxProgress && oldStage != MaxStage)
                {
                    OnNewStageUnlocked();
                }
            }
        }

        public virtual int MaxProgress =>
            this.Stage < MaxStage ? ProgressNeeded[Stage + 1] : ProgressNeeded[MaxStage];
        
        public event NewStageUnlockedHandler NewStageUnlocked;
        public event ProgressChangedHandler ProgressChanged;

        protected abstract int[] ProgressNeeded { get; }

        protected GameLogWatcher watcher;

        public Achievement(GameLogWatcher watcher)
        {
            this.watcher = watcher;
        }

        protected void OnNewStageUnlocked()
        {
            NewStageUnlocked?.Invoke(this);
        }

        protected void OnProgress()
        {
            ProgressChanged?.Invoke(this);
        }
    }
}
