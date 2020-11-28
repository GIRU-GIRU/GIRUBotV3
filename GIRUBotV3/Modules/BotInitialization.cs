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

        public static async Task GIRUBotInitializationTasks()
        {
            try
            {
                ITextChannel chnl = null;

                while (chnl == null)
                {
                    chnl = _client.GetChannel(Global.Config.MeleeSlasherMainChannel) as ITextChannel;
                    if (chnl != null)
                    {
                        await chnl.SendMessageAsync("GIRUBotV3 starting...");


                        var exceptionHandlerReady = ExceptionHandler.InitContext(_client);
                        if (exceptionHandlerReady)
                        {
                            await chnl.SendMessageAsync("Reporting systems online");
                        }

                        await chnl.SendMessageAsync("Ready");
                    }

                    Task.Delay(1000).Wait();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize GIRUBot {ex.Message}");
                throw ex;
            }
        }
    }
}