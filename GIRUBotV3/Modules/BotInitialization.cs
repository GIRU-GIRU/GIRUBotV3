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
    public class BotInitialization : ModuleBase<SocketCommandContext>
    {
        private static DiscordSocketClient _client;

        public BotInitialization(DiscordSocketClient client)
        {
            _client = client;
        }

        public static async Task StartUpMessages()
        {
            var chnl = _client.GetChannel(Config.MeleeSlasherMainChannel) as ITextChannel;
            await chnl.SendMessageAsync("GIRUBotV3 starting...");
            Task.Delay(500).Wait();

            await chnl.SendMessageAsync("Online");

        }



    }

}