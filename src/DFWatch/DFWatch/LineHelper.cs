using System.Collections.Generic;

namespace Snay.DFStat.Watch
{
    public static class LineHelper
    {
        public static Dictionary<LineType, string[]> PatternMappings
        {
            get => new()
            {
                { LineType.Combat, CombatPatterns },
                { LineType.DFHack, DFHackPatterns },
                { LineType.AnnouncementGood, AnnouncementGoodPatterns },
                { LineType.AnnouncementBad, AnnouncementBadPatterns },
                { LineType.Mandate, MandatesPatterns },
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
            "pushes",
            "falls over",
            "gives in to pain",
            "has become enraged",
            "An artery has been opened",
            "has been knocked unconscious",
            "ligament has been torn",
            "tendon has been torn",
            "^The force bends",
            "^The force pulls",
            "^An artery has been opened",
            "^The force twists",
            "is propelled away",
            "^The (.+) slams into an obstacle",
            "^The (.+) looks sick",
            "^The (.+) vomits",
            "^The (.+) retches",
            "is having trouble breathing",
            "^The (.+) regains consciousness",
            "^The (.+) passes out from exhaustion",
            "^A major artery in the (\\w+) has been opened by the attack!",

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

        public static readonly string[] MandatesPatterns =
        {
            ConstructionMandatePattern,
            ExportsBannedMandatePattern
        };

        public const string ForgottenBeastHasComePattern =
            "^The Forgotten Beast (.+) has come!";

        public const string CaravanHasArrivedPattern =
            "(.+)caravan(.+) has arrived\\.";

        public const string ConstructionMandatePattern =
            "(.+) has mandated the construction of certain goods\\.";

        public const string ExportsBannedMandatePattern =
            "(.+) has imposed a ban on certain exports\\.";
    }
}
