using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Models;
using GIRUBotV3.Personality;
using GIRUBotV3.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FaceApp;
using System.Collections.Generic;

namespace GIRUBotV3.Modules
{
    public class OnMessage
    {
        private static DiscordSocketClient _client;
        private FaceAppClient _FaceAppClient;
        public OnMessage(DiscordSocketClient client, FaceAppClient FaceAppClient)
        {
            _client = client;
            _FaceAppClient = FaceAppClient;
        }

        private static Regex regexNounTest = new Regex(@"^\![^ ]+test");
        private static Regex regexInviteLinkDiscord = new Regex(@"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/.+[a-z]");
        private static List<string> FaceAppFilterTypes = FilterTypes.FilterTypesStrings;
        public async Task MessageContainsAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot || Helpers.IsRole("Moderator", context.User as SocketGuildUser)) return;

            //for (int i = 0; i < FaceAppFilterTypes.Count; i++)
            //{
            //    //does the message contain an attempted URL, if not grab past 20 msgs ?
            //    if (message.Content.Split(" ")[0].ToLower() == "+" + FaceAppFilterTypes[i].ToLower() && message.Content.Split(" ").Length <= 1)
            //    {
            //        var FaceApp = new FaceAppCommands(_FaceAppClient);
            //        await FaceApp.FaceMorphPreviousMessages(context);
            //        break;
           
            //    }
            //    else if (message.Content.Split(" ")[0].ToLower() == "+" + FaceAppFilterTypes[i].ToLower())
            //    {
            //        var FaceApp = new FaceAppCommands(_FaceAppClient);
            //        await FaceApp.ApplyFaceApp(message);
            //        break;
            //    }
            //}
            if (Helpers.OnOffExecution(context.Message) == true)
            {
                await context.Message.DeleteAsync();
            }
            if (message.Content.Contains("😃"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("😃");
                }
            }
            //if (message.Content.Contains("help"))
            //{
            //    var r = new Random();
            //    if (r.Next(1, 15) <= 2)
            //    {
            //        string insultHelp = await Insults.GetInsult();
            //        await context.Channel.SendMessageAsync("stop crying for help " + insultHelp);
            //    }
            //}
            if (regexNounTest.Match(message.Content).Success)
            {
                var noun = regexNounTest.Match(message.Content).Groups[2].ToString();
                var nounTestTask = new RollRandom();
                await nounTestTask.NounTest(noun, message);
            }
            if (regexInviteLinkDiscord.Match(message.Content).Success & !Helpers.IsRole("Moderator", message.Author as SocketGuildUser))
            {

                var insult = await Insults.GetInsult();
                await context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{context.User.Mention}, don't post invite links {insult}");
            }




        }
        public async Task UpdatedMessageContainsAsync(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var messageAfter = after as SocketUserMessage;

            var context = new SocketCommandContext(_client, messageAfter);
            if (messageAfter.Author.IsBot || Helpers.IsRole("Moderator", context.User as SocketGuildUser)) return;

            if (regexInviteLinkDiscord.Match(messageAfter.Content).Success)
            {
                var insult = await Insults.GetInsult();
                await context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{context.User.Mention}, don't post invite links {insult}");
            }
            if (messageAfter.Content.Contains("😃"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("😃");
                }
            }
        }
    }
}
