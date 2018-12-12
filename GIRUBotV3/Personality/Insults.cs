using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace GIRUBotV3.Personality
{
    public static class Insults
    {
        public static async Task<string> GetInsult()
        {
           Random rnd = new Random();
           string[] insultsArray = new string[]
           {
            "son of bastard",
            "spastic",
            "fat cunt",
            "cockroach",
            "stupid fuck",
            "gay cunt",
            "fucking retard",          
            "retard",
            "autist",
            "mongoloid",
            "creature",
            "noob",
            "trash kid",
            "LOSER",
            "idiot",
            "weakling",
            "smallson",
            "saddo",
            "cuck",
            "gay",
            "shitter",
            "gimp",
            "pubber trash",
            "slut bitch",
           "delinquent"
           };
               int pull = rnd.Next(insultsArray.Length);
               string insult = insultsArray[pull].ToString();
               return insult;
        }

        public static async Task<string> GetWarning()
        {
            Random rnd = new Random();
            string[] warnArray = new string[]
            {
               "stop it immediate fucking asshuele",
               "stop FAKKING around",
               "what the FAK are you doing",
               "kid, wanna get banned ?",
               "want ban?",
               "wat the FAK are u doing stop that IMMEDIATE",
               "stop or ban",
               "im gonna fuck ur mum",
               "u want fucking ban i suggest u keep fucking about",
               "ok u gona get ban",
               "stop fucking around stupid fucking bastard",
               "fucking bastard stop that immediate",
               "son of bastard stop that immediately",
               "fucking son of bastard stop doing that at once",
               "fucking mother cunt stop it NOW",
               "stop FUCK around",
               "I fucking ban u"
            };
            int pull = rnd.Next(warnArray.Length);
            string warning = warnArray[pull].ToString();
            return warning;
        }
    }
}
