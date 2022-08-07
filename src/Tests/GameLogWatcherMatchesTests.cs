using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snay.DFStat.Watch;
using System;
using System.Collections.Generic;

namespace Snay.DFStat.Tests
{
    [TestClass]
    public class GameLogWatcherMatchesTests
    {
        private static string[] CombatSamples = {
            "The elk bird falls over.",
            "The elk bird gives in to pain.",
            "The elk bird has become enraged!",
            "An artery has been opened by the attack!",
            "The elk bird has been knocked unconscious!",
            "A ligament has been torn and a tendon has been torn!",
            "The force bends the left knee, tearing apart the muscle and bruising the bone and tearing apart the muscle and bruising the bone!",
            "The force pulls the left knee and the part splits in gore!",
            "The force bends the right knee, shattering the bone and shattering the bone!",
            "The force twists the right shoulder,  shattering the bone and  shattering the bone!",
            "The gorlak is propelled away by the force of the blow!",
            "The gorlak slams into an obstacle!",
            "The troll gores the forgotten beast in the right wing with her right horn and the severed part sails off in an arc!",
            "The elk bird looks sick!",
            "The elk bird vomits.",
            "The elk bird retches.",
            "The elk bird is having trouble breathing!",
            "The elk bird regains consciousness.",
            "The elk bird passes out from exhaustion.",
            "A major artery in the heart has been opened by the attack!",
            "A tendon in the upper spine has been bruised!",
            "A tendon in the upper spine has been torn!",
            "The human lasher loses hold of the X({large copper right gauntlet})X.",
            "The Axe Lord shakes the human pikeman around by the upper body, tearing apart the upper body's fat!",
            "The XX({large bronze mail shirt})XX is ripped to shreds!",
            "The XX({large moose leather robe})XX breaks!",
        };

        private static string[] UnicodeSamples =
        {
            "The Swordmaster pulls on the embedded ⁎copper short sword⁎.",
            "Datan Somkikrost has improved a ☰honey bee wax scepter☰ masterfully!",
            "The wrestler strikes at the speardwarf but the shot is blocked with the («bronze shield»)!",
            "The speardwarf stabs the wrestler in the right hand with her ⁎silver spear⁎, lightly tapping the target!",
        };

        [TestMethod]
        public void CanReadLines()
        {
            Dictionary<string, int> lineCalls = SetupLineCalls(CombatSamples);
            GameLogWatcher watcher = SetupWatcher(lineCalls);
            watcher.StartWatching(false);
            foreach (KeyValuePair<string, int> kv in lineCalls)
            {
                Assert.AreEqual(1, kv.Value, $"Expected this line to be called once:{Environment.NewLine + kv.Key}");
            }
            watcher.StopWatching();
        }

        [TestMethod]
        public void ReplacesSpecialCharactersWithUnicode()
        {
            Dictionary<string, int> lineCalls = SetupLineCalls(UnicodeSamples);
            GameLogWatcher watcher = SetupWatcher(lineCalls);
            watcher.StartWatching(false);
            foreach (KeyValuePair<string, int> kv in lineCalls)
            {
                Assert.AreEqual(1, kv.Value, $"Expected this line to be called once:{Environment.NewLine + kv.Key}");
            }
            watcher.StopWatching();
        }

        private Dictionary<string, int> SetupLineCalls(string[] samples)
        {
            Dictionary<string, int> lineCalls = new();
            foreach (string line in samples)
            {
                lineCalls.Add(line, 0);
            }

            return lineCalls;
        }

        private GameLogWatcher SetupWatcher(Dictionary<string, int> lineCalls)
        {
            GameLogWatcher watcher = new GameLogWatcher();
            watcher.LineAdded += (sender, args) => {
                foreach (string key in lineCalls.Keys)
                {
                    if (string.Equals(key, args.Text, StringComparison.Ordinal))
                    {
                        lineCalls[key]++;
                    }
                }
            };

            return watcher;
        }
    }
}
