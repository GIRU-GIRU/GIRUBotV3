using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using System.IO;
using FaceApp;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Linq;
using GIRUBotV3.Preconditions;
using GIRUBotV3.Models;

namespace GIRUBotV3.Modules
{
    public class FaceAppCommands : ModuleBase<SocketCommandContext>
    {

        private FaceAppClient _FaceAppClient;
        public FaceAppCommands(FaceAppClient FaceAppClient)
        {
            _FaceAppClient = FaceAppClient;
        }

        [Command("morphtypes")]
        public async Task FaceAppHelp()
        {
            await Context.Channel.SendMessageAsync(String.Join(", ", FilterTypes.GetMorphTypes()));
            return;
        }

        private string url;
        private bool typeAvailable;
        [Command("morph")]
        [Ratelimit(35, 10, Measure.Minutes)]
        public async Task FaceMorph([Remainder]string input)
        {
            using (Context.Channel.EnterTypingState())
            {
                var insult = await Insults.GetInsult();
                await Context.Message.Channel.TriggerTypingAsync();          
                var inputArray = Context.Message.Content.Split(" ");
                if (inputArray.Length > 3)
                {
                    await Context.Channel.SendMessageAsync("what kind of url is that u " + insult);
                }
                var type = inputArray[1];

                foreach (var item in FilterTypes.GetMorphTypes())
                {
                    if (item.ToLower() == type.ToLower())
                    {
                        typeAvailable = true;
                        break;
                    }
                }

                if (!typeAvailable)
                {
                    await Context.Channel.SendMessageAsync("not a valid type you fucking " + insult + ", use !morphtypes");
                    return;
                }

                //get previous messages because no URL was passed in
                if (inputArray.Length <= 2)
                {
                    var recentMessages = await Context.Channel.GetMessagesAsync(30).FlattenAsync();
                    foreach (var item in recentMessages)
                    {
                        if (item.Attachments.Count > 0)
                        {
                            url = item.Attachments.First().Url;
                            break;
                        }
                        else if (item.Embeds.Count > 0)
                        {
                            url = item.Embeds.First().Url;
                            break; ;
                        }
                    }
                }
                //URL was passed in? check if it's a URL or embedded image passed in
                else if (Context.Message.Attachments.Count == 0)
                {
                    url = inputArray[2];
                }
                else
                {
                    url = Context.Message.Attachments.First().Url;
                }
                if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                {
                    try
                    {
                        var code = await _FaceAppClient.GetCodeAsync(uri);
                        using (var imgStream = await _FaceAppClient.ApplyFilterAsync(code, (FilterType)Enum.Parse(typeof(FilterType), type, true)))
                        {
                            await Context.Channel.SendFileAsync(imgStream, type + ".png");
                            return;
                        }
                    }
                    catch (FaceException ex)
                    {
                        await Context.Channel.SendMessageAsync(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}



