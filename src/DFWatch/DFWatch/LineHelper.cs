using System.Collections.Generic;

namespace Snay.DFStat.Watch
{
    public static class LineHelper
    {
        public static Dictionary<LineType, string[]> PatternMappings
        { get => new()
            {
                { LineType.Combat, CombatPatterns },
                { LineType.DFHack, DFHackPatterns },
                { LineType.AnnouncementGood, AnnouncementGoodPatterns },
                { LineType.AnnouncementBad, AnnouncementBadPatterns }
            };
        }

        public static readonly string[] CombatPatterns = {
            "strikes",
            "misses",
            "attacks",
            "bashes",
            "charges at",
            "is no longer stunned",
            "stands up",
            "is knocked over",
            "tangle together",
            "collides with",
            "jumps away",
            "kicks",
            "scratches",
            "bites",
            "looks surprised",
            "punches",
            "hacks",
            "slashes",
            "slaps",
            "stabs",
            "bounces backward",
            "pushes"
        };

        public static readonly string[] DFHackPatterns =
        {
            "is no longer rusty",
            "is now rusty",
            "is no longer very rusty",
            "is now very rusty"
        };

        public static readonly string[] AnnouncementBadPatterns =
        {
            ForgottenBeastHasComePattern
        };

        public static readonly string[] AnnouncementGoodPatterns =
        {
            CaravanHasArrivedPattern
        };

        public const string ForgottenBeastHasComePattern =
            "^The Forgotten Beast (.+) has come!(.*)";

        public const string CaravanHasArrivedPattern =
            "(.+)caravan(.+) has arrived.";
    }
}
