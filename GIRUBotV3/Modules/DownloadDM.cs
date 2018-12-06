using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Models;
using System.IO;

namespace GIRUBotV3.Modules
{
    public class DownloadDM : ModuleBase<SocketCommandContext>
    {
        DiscordSocketClient _client;
        public DownloadDM(DiscordSocketClient client)
        {
            _client = client;
        }
     
        [Command("mydmswithgirubot")]
        private async Task Download()
        {
            var user = Context.Message.Author as SocketGuildUser;
            try
            {
                var dms = await user.GetOrCreateDMChannelAsync();
                var allMessagesFlattened = await dms.GetMessagesAsync(300).FlattenAsync();
                string dir = $"{System.Environment.CurrentDirectory.ToString()}\\{dms.Name}.txt";

                if (File.Exists(dir))
                {
                    File.Delete(dir);
                }

                int i = 0;
                using (StreamWriter file =
                new StreamWriter(dir, true))
                {
                    foreach (var item in allMessagesFlattened)
                    {
                        file.WriteLine($"{item.Timestamp.DateTime}: {item.Author} \n {item.Content} \n \n");
                        i++;
                    }
                }

                if (i > 1)
                {                
                    await Context.Channel.SendFileAsync(dir, $"here is the direct messages for {dms.Recipient}");
                    File.Delete(dir);
                    return;
                }
                await Context.Channel.SendMessageAsync("no messages w/ this loser ");
            }
            catch (Exception ex)
            {

                await Context.Channel.SendMessageAsync("this bitch too afraid to dm me lmfao");
            }

        }
    }

}

