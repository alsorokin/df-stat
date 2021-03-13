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

            // These two should also be in that particular order:
            { LineType.CombatMinor, CombatMinorPatterns },
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
            { LineType.Visitors, VisitorsPatterns },
            { LineType.JobSuspended, JobSuspendedPatterns },
            { LineType.TimeOfYear, TimeOfYearPatterns },
            { LineType.Weather, WeatherPatterns },
            { LineType.AnimalWild, AnimalWildPatterns },
            { LineType.Diplomacy, DiplomacyPatterns },
            { LineType.Politics, PoliticsPatterns },
            { LineType.Discovery, DiscoveryPatterns },
            { LineType.Dead, DeadPatterns },
            { LineType.Adamantine, AdamantinePatterns },
            { LineType.Minerals, MineralsPatterns },
            { LineType.CaveIn, CaveInPatterns },
            { LineType.ArtDefacement, ArtDefacementPatterns },
            { LineType.AnimalKnowledge, AnimalKnowledgePatterns },
            { LineType.NamedWeapons, NamedWeaponsPatterns },
        };

        public static List<LineTraitMapping> LineTraitMappings => new()
        {
            new LineTraitMapping() { 
                LnType = LineType.JobCancellation,
                Trait = "eqMismatch",
                Patterns = new string[]{ EquipmentMismatchPattern },
            },
        };

        private static string[] CombatMinorPatterns => new string[]
        {
            @"lightly tapping the target!$",
            @"^The (.+) strikes at the (.+) but the shot is blocked(.*)!$",
            @"^The (.+) attacks the (.+) but (He|She|It) (scrambles|rolls|jumps) away!$",
            @"^The (.+) strikes at the (.+) but the shot is ?(just barely|deftly|easily)? parried by (.+)!$",
            @"^The (.+) strikes at the (.+) but the shot is ?(narrowly|easily|effortlessly)? deflected by (.+)!$",
            @"^The (.+) stands up\.$",
            @"^The (.+) misses the (.+)!$",
            @"^The (.+) is no longer stunned\.$",
            @"^The (.+) charges at the (.+)!$",
            @"^The (.+) collides with the (.+)!$",
            @"^The (.+) is knocked over!$",
            @"^The (.+) looks surprised by the ferocity of The (.+)'s onslaught!$",
            @"^They tangle together and (fall over|tumble forward)!$",
            @"^The (.+) bounces backward!$",
            @"^The (.+) collapses and falls to the ground from over-exertion\.$",
            @"^The (.+) is knocked over and tumbles backward!$",
            @"^The (.+) jumps away!$",
        };

        // TODO: get rid of too general patterns, like "strikes" or "kicks"
        private static string[] CombatPatterns => new string[]
        {
            @"^The flying (.+) strikes the (.+) in the (.+),? ?(.*)!$",
            @"^The (.+) strikes the (.+) in the (.+),? (.+)!$",
            @"bashes",
            @"^The (.+) kicks the (.+) in the (.+) with (its|his|her)",
            @"scratches",
            @"^The (.+) bites the (.+) in the (.+)",
            @"^The (.+) punches the (.+) in the (.+)",
            @"hacks",
            @"slashes",
            @"slaps",
            @"stabs",
            @"pushes",
            @"^The (.+) falls over\.$",
            @"^The (.+) gives in to pain\.$",
            @"^The (.+) has become enraged!$",
            @"^An artery has been opened by the attack",
            @"^The (.+) has been knocked unconscious!$",
            @"ligament has been torn",
            @"tendon has been torn",
            @"^The force (bends|twists|pulls) the",
            @"^An artery has been opened",
            @"^A (.+) has been bruised",
            @"^The (.+) is propelled away by the force of the blow!",
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
            @"has entered a martial trance!$",
            @"has left the martial trance\.$",
            @"latches on firmly!$",
            @"^A major artery has been opened by the attack!$",
            @"^A major artery in the (.+) has been opened by the attack!$",
            @"^An artery in the (.+) has been opened by the attack!$",
            @"^A (sensory|motor) nerve has been severed!$",
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
            @"^The (.+) struggles in vain against the grip of the (.+)'s (.+) on The (.+)'s (.+)\.$",
            @"^The (.+) slams into the (.+)\!$",
            @"^The (.+) shoots out thick strands of webbing\!$",
            @"^The (.+) bends the (.+)'s (.+) with The (.+)'s (.+)(,?.*)!$",
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
            @"^The merchants from (.+) have embarked on their journey\.$",
        };

        private static string[] MandatesPatterns => new string[]
        {
            ConstructionMandatePattern,
            ExportsBannedMandatePattern,
            @" has ended a mandate\.$"
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
            @"has been possessed!$",
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
            // Note that the Manager achievement is tied to the first capturing group of this one particular pattern
            @"\((\d+)\) has been completed\.$",
        };

        private static string[] OccupationPatterns => new string[]
        {
            @"(?<!An animal) has become a (.+)\.$",
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
            @"^An animal has become a (.+)\.$",
        };

        private static string[] GrowthDwarfPatterns => new string[]
        {
            @"(?<!An animal) has grown to become a (.+)\.$",
        };

        private static string[] SlaughterPatterns => new string[]
        {
            @"has been slaughtered\.$",
        };

        private static string[] VisitorsPatterns => new string[]
        {
            @"^(.+), a (.+), is visiting\.$",
            @"^(.+), (.+) is visiting\.$",
            @"^(.+) and others have returned\.$",
            @"^(.+) have returned\.$",
        };

        private static string[] JobSuspendedPatterns => new string[]
        {
            @"^Digging designation cancelled: warm stone located\.$",
            @"^The dwarves suspended the construction of (.+)\.$",
        };

        private static string[] TimeOfYearPatterns => new string[]
        {
            @"^Spring has arrived on the calendar\.$",
            @"^Spring has arrived!$",
            @"^It is now summer\.$",
            @"^Autumn has come\.$",
            @"^Autumn has arrived on the calendar\.$",
            @"^Winter is upon you\.$",
        };

        private static string[] WeatherPatterns => new string[]
        {
            @"^The weather has cleared\.$",
            @"^It has started raining\.$",
        };

        private static string[] AnimalWildPatterns = new string[]
        {
            @"^The (.+) has reverted to a wild state!$",
        };

        private static string[] DiplomacyPatterns = new string[]
        {
            @"^A (.+) diplomat from (.+) has arrived\.$",
            @"^The outpost liaison (.+) from (.+) has arrived\.$",
        };

        private static string[] PoliticsPatterns = new string[]
        {
            @"^(.+), (.+) has been re-elected\.$",
            @"^After a polite discussion with local rivals, (.+) has claimed the position of (.+) of (.+)\.$",
            @"^The (.+), a (.+) guild, has been established\.$",
        };

        private static string[] DiscoveryPatterns = new string[]
        {
            @"^The (\w+) (.+) has discovered (.+)\.$",
        };

        private static string[] DeadPatterns = new string[]
        {
            @" has been found dead\.$",
        };

        private static string[] AdamantinePatterns = new string[]
        {
            @"^Raw adamantine!\s+Praise the miners!$",
        };

        private static string[] MineralsPatterns = new string[]
        {
            @"^You have struck (.+)\!$",
        };

        private static string[] CaveInPatterns = new string[]
        {
            @"^A section of the cavern has collapsed!$",
        };

        private static string[] ArtDefacementPatterns = new string[]
        {
            @"^A masterwork of (.+) has been lost!$",
        };

        private static string[] AnimalKnowledgePatterns = new string[]
        {
            @"^The dwarves of (.+) are now expert (.+) trainers\.$",
            @"^The dwarves of (.+) are now quite knowledgeable (.+) trainers\.$",
        };

        private static string[] NamedWeaponsPatterns = new string[]
        {
            @"^(.+), (.+) has grown attached to a (.+)!$",
            @"^(.+), (.+) has bestowed the name (.+) upon a (.+)!$",
        };

        private const string ForgottenBeastHasComePattern =
            @"^The Forgotten Beast (.+) has come!";

        private const string ConstructionMandatePattern =
            @"^(.+) has mandated the construction of certain goods\.";

        private const string ExportsBannedMandatePattern =
            @"^(.+) has imposed a ban on certain exports\.";

        public const string KneeHurtPattern =
            @"^The force (bends|twists|pulls) the (.+) knee";

        public const string BoltInTheLowerLegPattern =
            @"^The flying (.+) (bolt|arrow)(.{0,3}) strikes the (.+) in the (.+) lower leg";
    }
}
