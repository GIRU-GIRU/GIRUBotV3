﻿using Discord;
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
     
        private IDMChannel dms;
        [Command("download")]
        private async Task Download(SocketUser user)
        {
            if (user.Id != Context.Message.Author.Id)
            {
                await Context.Channel.SendMessageAsync("u have to ask them if they want their DMs posted publicly (gay privacy laws)");
            }
            try
            {
                var dms = await user.GetOrCreateDMChannelAsync();
                var allMessagesFlattened = await dms.GetMessagesAsync(300).FlattenAsync();
                string dir = $@"{System.Environment.CurrentDirectory}/{dms.Name}.txt";

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
                    File.Delete(dir);
                    await Context.Channel.SendFileAsync(dir, $"here is the direct messages for {dms.Recipient}");
                    return;
                }
                await Context.Channel.SendMessageAsync("no messages w/ this loser ");
            }
            catch (Exception)
            {

                await Context.Channel.SendMessageAsync("this bitch too afraid to dm me lmfao");
            }

        }
    }

}

