using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Watch.Achievements;

namespace Snay.DFStat.Watch.Achievements
{
    public class AchievementTracker
    {
        private readonly GameLogWatcher watcher;

        public List<Achievement> Achievements { get; private set; }

        public event NewStageUnlockedHandler NewStageUnlocked;
        public event ProgressChangedHandler ProgressChanged;
        public event ProgressPcChangedHandler ProgressPcChanged;

        private static string DfWatchAppDataFolder { get => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "df-watch"); }
        private static string SaveFilePath { get => Path.Join(DfWatchAppDataFolder, "achievements.xml"); }

        public AchievementTracker(GameLogWatcher watcher)
        {
            this.watcher = watcher;
            LoadState();

            foreach (Achievement achievement in this.Achievements)
            {
                achievement.NewStageUnlocked += HandleNewStageUnlocked;
                achievement.ProgressChanged += HandleProgressChanged;
                achievement.ProgressPcChanged += HandleProgressPcChanged;
            }
        }

        private void InitAchievements()
        {
            this.Achievements = new()
            {
                new Butcher(watcher),
                new Craftsdwarf(watcher),
                new Manager(watcher),
                new Adventurer(watcher),
            };
        }

        private void LoadState()
        {
            InitAchievements();
            if (File.Exists(SaveFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<State>));
                using (FileStream stateFileStream = File.OpenRead(SaveFilePath))
                {
                    List<State> stateList = (List<State>)serializer.Deserialize(stateFileStream);
                    foreach (State state in stateList)
                    {
                        Achievement found = Achievements.FirstOrDefault(a => a.Name == state.AchievementName);
                        if (found != null)
                        {
                            found.SetState(state);
                        }
                    }
                }
            }
        }

        private void SaveState()
        {
            List<State> stateList = Achievements.Select(a => a.State).ToList();
            if (!Directory.Exists(DfWatchAppDataFolder))
            {
                Directory.CreateDirectory(DfWatchAppDataFolder);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<State>));
            using (FileStream stateFileStream = File.OpenWrite(SaveFilePath))
            {
                serializer.Serialize(stateFileStream, stateList);
            }
        }

        private void HandleProgressChanged(Achievement sender)
        {
            ProgressChanged?.Invoke(sender);
            SaveState();
        }

        private void HandleNewStageUnlocked(Achievement sender)
        {
            NewStageUnlocked?.Invoke(sender);
        }

        private void HandleProgressPcChanged(Achievement sender)
        {
            ProgressPcChanged?.Invoke(sender);
        }
    }
}
