using System.Collections.Generic;

namespace Snay.DFStat.Watch
{
    public static class LineHelper
    {
        public static Dictionary<LineType, string[]> PatternMappings
        {
            get => new()
            {
                // Dwarf and animal birth scan order is important
                { LineType.BirthDwarf, BirthDwarfPatterns },
                { LineType.BirthAnimal, BirthAnimalPatterns },

                { LineType.Combat, CombatPatterns },
                { LineType.StuffBreaking, StuffBreakingPatterns },
                { LineType.DFHack, DFHackPatterns },
                { LineType.Merchant, MerchantPatterns },
                { LineType.ForgottenBeast, AnnouncementBadPatterns },
                { LineType.Mandate, MandatesPatterns },
                { LineType.Masterpiece, MasterpiecePatterns },
                { LineType.StrangeMood, StrangeMoodPatterns },
                { LineType.JobCancellation, JobCancellationPatterns },
                { LineType.War, WarPatterns },
                { LineType.Order, OrderPatterns },
                { LineType.Occupation, OccupationPatterns },
                { LineType.GrowthDwarf, GrowthDwarfPatterns },
                { LineType.GrowthAnimal, GrowthAnimalPatterns },
            };
        }

        // TODO: get rid of too general patterns, like "strikes" or "kicks"
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
            "^A (.+) has been bruised",
            "^The force twists",
            "is propelled away",
            "^The (.+) slams into an obstacle",
            "^The (.+) looks sick",
            "^The (.+) vomits",
            "^The (.+) retches",
            "is having trouble breathing!$",
            "is having more trouble breathing!$",
            "^The (.+) regains consciousness",
            "^The (.+) passes out from exhaustion",
            "^A major artery in the heart has been opened by the attack!$",
            "^A tendon in the (.+) has been (.+)!$",
            "^The (.+) pulls on the embedded (.+)\\.$",
            "^The (.+) gains possession of the (.+)\\.$",
            "^The (.+) loses hold of the",
            "^The (.+) has lodged firmly in the wound!$",
            "^The (.+) shakes the (.+) around by",
            "^Many nerves have been severed!$",
            "^The (.+) collapses and falls to the ground from over-exertion\\.$",
            "has entered a martial trance!$",
            "has left the martial trance\\.$",
            "latches on firmly!$",
            "^A major artery has been opened by the attack!$",
            "looks even more sick!$",
            "breaks the grip of the (.+)'s (.+) on The (.+)'s (.+)\\.$",
        };

        public static string[] StuffBreakingPatterns =
        {
            "^The (.+) is ripped to shreds!$",
            "^The (.+) breaks!$",
        };

        public static readonly string[] DFHackPatterns =
        {
            "is no longer rusty",
            "is now rusty",
            "is no longer very rusty",
            "is now very rusty",
            "has became (.+)\\.",
        };

        public static readonly string[] AnnouncementBadPatterns =
        {
            ForgottenBeastHasComePattern,
        };

        public static readonly string[] MerchantPatterns =
        {
            "caravan(.+) has arrived\\.$",
            "^Merchants have arrived and are unloading their goods\\.$",
            "^The merchants from (.+) will be leaving soon\\.$",
            "^The merchants from (.+) have embarked on their journey.$",
        };

        public static readonly string[] MandatesPatterns =
        {
            ConstructionMandatePattern,
            ExportsBannedMandatePattern,
        };

        public static readonly string[] MasterpiecePatterns =
        {
            MasterpieceCookPattern,
            MasterpieceDyerPattern,
            MasterpieceGeneralPattern,
            MasterpieceImprovementPattern,
        };

        public static readonly string[] MandatePatterns =
        {
            MandateConstructionPattern,
            MandateBanGoodsPattern,
        };

        public static readonly string[] StrangeMoodPatterns =
        {
            "is taken by a fey mood!$",
            "withdraws from society\\.\\.\\.$",
            "has claimed a (.+)\\.$",
            "has begun a mysterious construction!$",
            "has created (.+), a (.+)!",
        };

        public static readonly string[] JobCancellationPatterns =
        {
            "(.+), (.+) cancels (.+): (.+)\\.",
        };

        public static readonly string[] WarPatterns =
        {
            "^The enemy have come and are laying siege to the fortress\\.$",
            "^A vile force of darkness has arrived!$",
        };

        public static readonly string[] OrderPatterns =
        {
            "\\(\\d+\\) has been completed\\.$",
        };

        public static readonly string[] OccupationPatterns =
        {
            "has become a (.+)\\.$",
        };

        public static readonly string[] BirthDwarfPatterns =
        {
            "^(.+), (.+) has given birth to (a girl|a boy|twins|triplets)\\.$",
        };

        public static readonly string[] BirthAnimalPatterns =
        {
            "has given birth to a (.+)\\.",
            "has given birth to (.+)s\\.",
        };

        public static readonly string[] GrowthAnimalPatterns =
        {
            "^An animal has grown to become a (.+)\\.$",
        };

        public static readonly string[] GrowthDwarfPatterns =
        {
            "(?<!An animal) has grown to become a (.+)\\.$",
        };

        public const string ForgottenBeastHasComePattern =
            "^The Forgotten Beast (.+) has come!";

        public const string ConstructionMandatePattern =
            "(.+) has mandated the construction of certain goods\\.";

        public const string ExportsBannedMandatePattern =
            "(.+) has imposed a ban on certain exports\\.";

        public const string MasterpieceDyerPattern =
            "has dyed a masterpiece!$";

        public const string MasterpieceCookPattern =
            "has cooked a masterpiece!$";

        public const string MasterpieceGeneralPattern =
            "has created a masterpiece (.+)!$";

        public const string MasterpieceImprovementPattern =
            "has improved (.+) masterfully!$";

        public const string MandateConstructionPattern =
            "has mandated the construction of certain goods\\.$";

        public const string MandateBanGoodsPattern =
            "has imposed a ban on certain exports\\.$";
    }
}
