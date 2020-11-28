using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Preconditions;
using Discord.Net;
using System.Linq;
using GIRUBotV3.Models;


namespace GIRUBotV3.Modules
{

    public static class CommandToggles
    {
        public static bool Memestore = true;
    }

    public class Toggles : ModuleBase<SocketCommandContext>
    {
        [Command("memestore on")]
        private async Task memestoreOn()
        {
            CommandToggles.Memestore = true;
            await Context.Channel.SendMessageAsync("Memestore turned on");
        }

        [Command("memestore off")]
        private async Task memestoreOff()
        {
            CommandToggles.Memestore = false;
            await Context.Channel.SendMessageAsync("Memestore turned off");
        }

    }

}


