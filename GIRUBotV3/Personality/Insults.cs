﻿using System;
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
               "1",
               "2",          
               "3",
               "4",
               "5"
           };
               int pull = rnd.Next(insultsArray.Length);
               string insult = insultsArray[pull].ToString();
            return insult;

        }

    }
}
