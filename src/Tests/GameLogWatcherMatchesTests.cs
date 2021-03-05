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
            "The Swordmaster pulls on the embedded *copper short sword*.",
            "The Swordmaster gains possession of the *copper short sword*.",
            "The human lasher loses hold of the X({large copper right gauntlet})X.",
            "The Axe Lord shakes the human pikeman around by the upper body, tearing apart the upper body's fat!",
            "The XX({large bronze mail shirt})XX is ripped to shreds!",
            "The XX({large moose leather robe})XX breaks!",
        };

        [TestMethod]
        public void CanReadLines()
        {
            foreach (string sample in CombatSamples)
            {
                Dictionary<string, int> lineCalls = new();
                foreach (string line in CombatSamples)
                {
                    lineCalls.Add(line, 0);
                }

                GameLogWatcher watcher = new GameLogWatcher();
                watcher.LineAdded += (sender, args) => {
                    if (lineCalls.ContainsKey(args.Text))
                        lineCalls[args.Text] += 1;
                };
                watcher.ScanOnce();
                foreach (KeyValuePair<string, int> kv in lineCalls)
                {
                    Assert.AreEqual(1, kv.Value, $"Expected this line to be called once:{Environment.NewLine + kv.Key}");
                }
            }
        }
    }
}
