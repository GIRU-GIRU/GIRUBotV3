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
               "1",
               "2",          
               "3",
               "4",
               "5"
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
               "sry no",
               "sry but no",
               "mm no"
            };
            int pull = rnd.Next(NoPermArray.Length);
            string noPermString = NoPermArray[pull].ToString();
            return noPermString;

        }
    }
}
