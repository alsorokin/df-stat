using System;
using System.Linq;

namespace Snay.DFStat.Watch.Achievements
{
    public delegate void NewStageUnlockedHandler(Achievement sender);
    public delegate void ProgressChangedHandler(Achievement sender);
    public delegate void ProgressPcChangedHandler(Achievement sender);

    public abstract class Achievement
    {
        public abstract string Description { get; }

        public abstract string Name { get; }

        public virtual int Stage =>
            Array.IndexOf(ProgressNeededPerStage, ProgressNeededPerStage.Last(pn => pn <= Progress));

        public virtual int MaxStage => ProgressNeededPerStage.Length - 1;

        public State State { get; private set; }

        public virtual int Progress
        {
            get => State.Progress;
            protected set
            {
                if (Progress >= ProgressNeededPerStage.Last())
                    return;
                if (value > ProgressNeededPerStage.Last())
                    value = ProgressNeededPerStage.Last();

                // Remember old values for max progress and stage because
                // incrementing progress will automatically increase these values
                // when new stage is about to be unlocked
                int oldMaxProgress = MaxProgress;
                int oldStage = Stage;
                int oldProgressPc = ProgressPercent;
                State.Progress = value;
                OnProgress();

                if (value >= oldMaxProgress && oldStage != MaxStage)
                {
                    OnNewStageUnlocked();
                }

                if (ProgressPercent > oldProgressPc)
                {
                    OnProgressPc();
                }

            }
        }

        public int ProgressPercent => (int)Math.Round((double)Progress / MaxProgress * 100);

        public virtual int MaxProgress =>
            this.Stage < MaxStage ? ProgressNeededPerStage[Stage + 1] : ProgressNeededPerStage[MaxStage];
        
        public event NewStageUnlockedHandler NewStageUnlocked;
        public event ProgressChangedHandler ProgressChanged;
        public event ProgressPcChangedHandler ProgressPcChanged;

        protected abstract int[] ProgressNeededPerStage { get; }

        protected GameLogWatcher watcher;

        public Achievement(GameLogWatcher watcher)
        {
            this.watcher = watcher;
            this.State = new State();
            this.State.AchievementName = Name;
        }

        protected void OnNewStageUnlocked()
        {
            NewStageUnlocked?.Invoke(this);
        }

        protected void OnProgress()
        {
            ProgressChanged?.Invoke(this);
        }

        protected void OnProgressPc()
        {
            ProgressPcChanged?.Invoke(this);
        }

        internal void SetState(State state)
        {
            this.State = state;
        }
    }
}
