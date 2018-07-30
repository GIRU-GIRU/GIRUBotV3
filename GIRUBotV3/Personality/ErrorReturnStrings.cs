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
               "sry but nah",
               "not happening kid, sorry",
               "not happening",
               "lmao, nah",
               "ok",
               "ok sure yea ill definitely do that for u",
               "lmfao, yeah ok kid whatever you want",
               "err, yea ok im gonna let shitter do that",
               "LOL no chance",
               "LOL not happening kiddo",
               "u rly think ur in a position to ask me to do that",
               "stfu",
               "no stfu",
               "idk man just stfu",
               "yea ok sure man XD",
               "fuck off",
               "shut the FUCK up",
               "FUCK off",
               "just fuck off man no",
               "not happing so fuck off ty",
               "stfu plz",
               "just stfu man",
               "?",
               "u fr?",
               "fuck off plz ty",
               "er, nah",
               "er, no",
               "yea coz im gonna let some random retard do that",
               "no name = no power"
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
