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

namespace GIRUBotV3.Modules
{
    public class FaceAppCommands : ModuleBase<SocketCommandContext>
    {
        private static List<string> morphtypes = Models.FilterTypes.FilterTypesStrings;

        private FaceAppClient _FaceAppClient;
        public FaceAppCommands(FaceAppClient FaceAppClient)
        {
            _FaceAppClient = FaceAppClient;
        }

        [Command("morphtypes")]
        public async Task FaceAppHelp()
        {
            string morphtypesString = String.Join(", ", morphtypes.ToArray());
            await Context.Channel.SendMessageAsync("SYNTAX: +morph type \ncurrent types are: " + morphtypesString);
            return;
        }

        private string url;
        [Command("morph")]
        [Ratelimit(45, 10, Measure.Minutes)]
        public async Task FaceMorph([Remainder]string input)
        {
            input = "+" + input;
            var inputArray = Context.Message.Content.Split(" ");
            var type = inputArray[1].TrimStart('+');

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



