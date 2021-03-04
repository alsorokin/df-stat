using System.Collections.Generic;

namespace Snay.DFStat.Watch
{
    public static class LineHelper
    {
        public static Dictionary<LineType, string[]> PatternMappings => new()
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
            { LineType.Slaughter, SlaughterPatterns },
        };

        public static List<LineTraitMapping> LineTraitMappings => new()
        {
            new LineTraitMapping() { 
                LnType = LineType.JobCancellation,
                Trait = "eqMismatch",
                Patterns = new string[]{ EquipmentMismatchPattern },
            },
        };

        // TODO: get rid of too general patterns, like "strikes" or "kicks"
        private static string[] CombatPatterns => new string[]
        {
            @"strikes",
            @"misses",
            @"attacks",
            @"bashes",
            @"charges at",
            @"is no longer stunned",
            @"stands up",
            @"is knocked over",
            @"tangle together",
            @"collides with",
            @"jumps away",
            @"kicks",
            @"scratches",
            @"bites",
            @"looks surprised",
            @"punches",
            @"hacks",
            @"slashes",
            @"slaps",
            @"stabs",
            @"bounces backward",
            @"pushes",
            @"falls over",
            @"gives in to pain",
            @"has become enraged",
            @"An artery has been opened",
            @"has been knocked unconscious",
            @"ligament has been torn",
            @"tendon has been torn",
            @"^The force bends",
            @"^The force pulls",
            @"^An artery has been opened",
            @"^A (.+) has been bruised",
            @"^The force twists",
            @"is propelled away",
            @"^The (.+) slams into an obstacle",
            @"^The (.+) looks sick",
            @"^The (.+) vomits",
            @"^The (.+) retches",
            @"is having trouble breathing!$",
            @"is having more trouble breathing!$",
            @"^The (.+) regains consciousness",
            @"^The (.+) passes out from exhaustion",
            @"^A tendon in the (.+) has been (.+)!$",
            @"^The (.+) pulls on the embedded (.+)\.$",
            @"^The (.+) gains possession of the (.+)\.$",
            @"^The (.+) loses hold of the",
            @"^The (.+) has lodged firmly in the wound!$",
            @"^The (.+) shakes the (.+) around by",
            @"^The (.+) collapses and falls to the ground from over-exertion\.$",
            @"has entered a martial trance!$",
            @"has left the martial trance\.$",
            @"latches on firmly!$",
            @"^A major artery has been opened by the attack!$",
            @"^A major artery in the (.+) has been opened by the attack!$",
            @"^An artery in the (.+) has been opened by the attack!$",
            @"^A sensory nerve has been severed!$",
            @"^A motor nerve has been severed!$",
            @"^Many nerves have been severed!$",
            @"looks even more sick!$",
            @"breaks the grip of the (.+)'s (.+) on The (.+)'s (.+)\.$",
            @"^The (.+) grabs the (.+) by the (.+) with (.+)!$",
            @"^The (.+) releases the grip of (.+)'s (.+) (from|on) (.+)'s (.+)\.$",
            @"^The (.+) is unable to break the grip of The (.+)'s (.+) on The (.+)'s (.+)!$",
            @"^The (.+) catches The (.+)'s (.+) with the (.+)'s (.+)!$",
            @"^The (.+) breaks the grip of The (.+)'s (.+) from The (.+)'s (.+)!$",
            @"^The (.+) snatches at the (.+)!$",
            @"^The (.+) is caught in a burst of (.+)!$",
            @"^The (.+) is caught in a cloud of (.+)!$",
            @"^The (.+) breathes a cloud of (.+)'s (.+) vapor!$",
            @"^The (.+) breathes a cloud of (.+) boiling extract!$",
            @"^The (.+) locks the (.+)'s (.+) with The (.+)'s (.+)!$",
            @"^The (.+) gores the (.+) in the (.+) with her (.+)!$",
            @"^The (.+)'s (.+) takes the full force of the impact(.*)!$",
            @"^The (.+) throws the (.+) by the (.+) with The (.+)'s (.+)!$",
            @"^The (.+) breaks the grip of The (.+)'s (.+) (from|on) the (.+)'s (.+)\.$",
            @"^The (.+) has been stunned!$",
            @"^The (.+) has been stunned again!$",
            @"^The (.+) takes the (.+) down by the (.+) with The (.+)'s (.+)!$",
            @"^The (.+) places a chokehold on the (.+)'s throat with The (.+)'s (.+)!$",
            @"^The (.+) strangles the (.+)'s throat!$",
            @"^The (.+) passes out\.$",
            @"^The (.+)'s attack is interrupted!$",
            @"^The (.+) bends the (.+)'s (.+) with The (.+)'s (.+) and the (.+) collapses!$",
            @"^The (.+) releases the joint lock of The (.+)'s (.+) on the (.+)'s (.+)\.$",
            @"is no longer enraged\.$",
            @"^The (.+) is ripped away and remains in The (.+)'s grip!$",
            @"^The (.+)'s (.+) skids along the ground(.*)!$",
            @"^(.+) venom is injected into the the (.+)'s (.+) blood!$",
            @"^The (.+) is caught up in the web!$",
            @"^The (.+) is partially free of the web\.$",
            @"^The (.+) is completely free of the web\.$",
            @"^The (.+) blocks The flying (.+) with the (.+)!$",
        };

        public const string RepeatedLinePattern =
            @"^x(\d+)$";

        private const string EquipmentMismatchPattern =
            @"cancels Pickup Equipment: Equipment mismatch\.$";

        private static string[] StuffBreakingPatterns => new string[]
        {
            @"^The (.+) is ripped to shreds!$",
            @"^The (.+) breaks!$",
        };

        private static string[] DFHackPatterns => new string[]
        {
            @"is no longer rusty",
            @"is now rusty",
            @"is no longer very rusty",
            @"is now very rusty",
            @"has became (.+)\.",
        };

        private static string[] AnnouncementBadPatterns => new string[]
        {
            ForgottenBeastHasComePattern,
        };

        private static string[] MerchantPatterns => new string[]
        {
            @"caravan(.+) has arrived\.$",
            @"^Merchants have arrived and are unloading their goods\.$",
            @"^The merchants from (.+) will be leaving soon\.$",
            @"^The merchants from (.+) have embarked on their journey.$",
        };

        private static string[] MandatesPatterns => new string[]
        {
            ConstructionMandatePattern,
            ExportsBannedMandatePattern,
        };

        private static string[] MasterpiecePatterns => new string[]
        {
            @"has created a masterpiece (.+)!$",
            @"has cooked a masterpiece!$",
            @"has dyed a masterpiece!$",
            @"has improved (.+) masterfully!$",
            @"has constructed a masterpiece!$",
        };

        private static string[] StrangeMoodPatterns => new string[]
        {
            @"is taken by a fey mood!$",
            @"withdraws from society\.\.\.$",
            @"has claimed a (.+)\.$",
            @"has begun a mysterious construction!$",
            @"has created (.+), a (.+)!",
        };

        private static string[] JobCancellationPatterns => new string[]
        {
            @"(.+), (.+) cancels (.+): (.+)\.",
        };

        private static string[] WarPatterns => new string[]
        {
            @"^The enemy have come and are laying siege to the fortress\.$",
            @"^A vile force of darkness has arrived!$",
        };

        private static string[] OrderPatterns => new string[]
        {
            @"\(\d+\) has been completed\.$",
        };

        private static string[] OccupationPatterns => new string[]
        {
            @"has become a (.+)\.$",
        };

        private static string[] BirthDwarfPatterns => new string[]
        {
            @"^(.+), (.+) has given birth to (a girl|a boy|twins|triplets)\.$",
        };

        private static string[] BirthAnimalPatterns => new string[]
        {
            @"has given birth to a (.+)\.",
            @"has given birth to (.+)s\.",
        };

        private static string[] GrowthAnimalPatterns => new string[]
        {
            @"^An animal has grown to become a (.+)\.$",
        };

        private static string[] GrowthDwarfPatterns => new string[]
        {
            @"(?<!An animal) has grown to become a (.+)\.$",
        };

        private static string[] SlaughterPatterns => new string[]
        {
            @"has been slaughtered\.$",
        };

        private const string ForgottenBeastHasComePattern =
            @"^The Forgotten Beast (.+) has come!";

        private const string ConstructionMandatePattern =
            @"^(.+) has mandated the construction of certain goods\.";

        private const string ExportsBannedMandatePattern =
            @"^(.+) has imposed a ban on certain exports\.";
    }
}
