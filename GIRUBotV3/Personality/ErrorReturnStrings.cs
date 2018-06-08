using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace GIRUBotV3.Personality
{
    public static class ErrorReturnStrings
    {

        public static async Task<string> GetError()
        {
           Random rnd = new Random();
           string[] insultsArray = new string[]
           {
               "not work",
               "not allow",
               "me not allow",
               "no cannot",          
               "me not can do",
               "sary not able to do",
               "n"
           };
               int pull = rnd.Next(insultsArray.Length);
               string insult = insultsArray[pull].ToString();
             return  insult;

        }

        public static async Task<string> GetNoPerm()
        {
            Random rnd = new Random();
            string[] NoPermArray = new string[]
            {
               "no",
               "nah",
               "u think im gonna let a random shitter like u do that ?",
               "sry but no",
               "mm, nah",
               "mmm, no",
               "yea coz im gonna let some random retard do that",
               "no name = no power"
            };
            int pull = rnd.Next(NoPermArray.Length);
            string noPermString = NoPermArray[pull].ToString();
            return noPermString;

        }
    }
}
