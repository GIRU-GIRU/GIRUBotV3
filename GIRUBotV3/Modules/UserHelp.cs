using Discord.Commands;
using Discord.WebSocket;
using Discord;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;

namespace GIRUBotV3.Modules
{

    public class UserHelp : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync()
        {
            await Context.Channel.SendMessageAsync("dont be so fucking WEAK");
        }
    }
}
