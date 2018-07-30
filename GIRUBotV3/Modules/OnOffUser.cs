using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class OnOffUser
    {
        public static List<IGuildUser> TurnedOffUsers { get; } = new List<IGuildUser>();
    }
}