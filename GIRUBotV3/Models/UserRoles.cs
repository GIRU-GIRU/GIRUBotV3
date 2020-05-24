using GIRUBotV3.Modules;
using System;
using System.Collections.Generic;
using System.Text;


namespace GIRUBotV3.Models
{

    public static class UserRoles
    {

        public static Dictionary<string, string> AllowedRolesDictionary { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Viewers"] = "Viewers",
            ["PugEU"] = "PugEU",
            ["PugNA"] = "PugNA",
            ["Roleplayer"] = "Roleplayer",
            ["Weeb"] = "Weeb",
            ["Elite"] = "Elite",
            ["Scotland"] = "Scotland",
            ["Armenia"] = "Armenia",
            ["EU"] = "EU",
            ["NA"] = "NA",
            ["RU"] = "RU",
            ["SA"] = "SA",
            ["ZA"] = "ZA",
            ["OCE"] = "OCE",
            ["noob"] = "noob",
            ["Veteran"] = "Veteran",
            ["Toxic"] = "Toxic",
            ["Simp"] = "Simp",
            ["Goon"] = "Goon",
            ["Aggressor"] = "Aggressor",
            ["Tryhard"] = "Tryhard",
            ["Dogshit"] = "Dogshit",
            ["Weirdo"] = "Weirdo",
            ["Cunt"] = "Cunt",
            ["Titan"] = "Titan",
            ["Weak"] = "Weak",
            ["Fearful"] = "Fearful",
            ["Innocent"] = "Innocent",
            ["Stupid"] = "Stupid",
            ["Whelp"] = "Whelp",
            ["Tilter"] = "Tilter",
            ["Creature"] = "Creature",
            ["Friendly"] = "Friendly",
            ["Helpful"] = "Helpful",
            ["Dev"] = "Dev",
            ["Mongol"] = "Mongol",
            ["Hot"] = "Hot",
            ["Leaker"] = "Leaker",
            ["Manipulative"] = "Manipulative",
            ["ORC"] = "ORC",
            ["Disgusting"] = "Disgusting",
            ["Substances"] = "Substances",
            ["Cool"] = "Cool",
            ["Liar"] = "Liar",
            ["Crybaby"] = "Crybaby",
            ["Loser"] = "Loser",
            ["Shitter"] = "SHITTER",
            ["Alcoholic"] = "Alcoholic",
            ["Slut"] = "Slut",
            ["Snake"] = "🐍",
            ["Gay"] = "Homosexual",
            ["Atlas"] = "Atlas 🌏",
            ["Oasis"] = "🌴 Last Oasis",
            ["Last"] = "🌴 Last Oasis",
            ["LastOasis"] = "🌴 Last Oasis",
            ["Organizer"] = "Organizer",
            ["fc"] = "👊 Fight Club",
            ["fight"] = "👊 Fight Club",
            ["fightclub"] = "👊 Fight Club",
            ["fighter"] = "👊 Fight Club",
            ["captains"] = "Captains Mode Feedback",
            ["captain"] = "Captains Mode Feedback",
            ["cmf"] = "Captains Mode Feedback",

        };

        public static Dictionary<string, string> ExclusiveRolesDictionary { get; private set; } = new Dictionary<string, string>
        {
            ["EU"] = "EU",
            ["NA"] = "NA",
            ["RU"] = "RU",
            ["SA"] = "SA",
            ["OCE"] = "OCE",
            ["ZA"] = "ZA",
            ["noob"] = "noob"
        };

    }
}


