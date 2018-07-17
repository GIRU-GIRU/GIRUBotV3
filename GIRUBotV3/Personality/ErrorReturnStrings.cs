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
               "sry no",
               "sry but no",
               "mm no",
               "mmm, no"
            };
            int pull = rnd.Next(NoPermArray.Length);
            string noPermString = NoPermArray[pull].ToString();
            return noPermString;

        }

        public static async Task<string> GetParseFailed()
        {
            Random rnd = new Random();
            string[] ParseFailedArray = new string[]
            {
               "how am i supposed to know who this nobody is",
               "i have no idea who this shitter is",
               "? why u think i know some nobody LOL",
               "that kid not even real",
               "nonexistant shitter",

            };
            int pull = rnd.Next(ParseFailedArray.Length);
            string noParseString = ParseFailedArray[pull].ToString();
            return noParseString;

        }
    }
}
