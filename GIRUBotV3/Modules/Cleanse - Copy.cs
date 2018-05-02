using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using System.Linq;

namespace GIRUBotV3.Modules
{
    public class UtilityModule : ModuleBase
    {
        [Command("purge")]
        public async Task Clean(int amount)
        {
            int index = 0;
            var deleteMessages = new List<IMessage>(amount);
            var messages = Context.Channel.GetMessagesAsync();

        }
    }
}