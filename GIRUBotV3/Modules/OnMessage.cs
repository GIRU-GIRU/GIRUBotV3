using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Models;
using GIRUBotV3.Personality;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FaceApp;
using System.Collections.Generic;

namespace GIRUBotV3.Modules
{
    public class OnMessage : ModuleBase<SocketCommandContext>
         
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
        public async Task MessageContainsAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot || Helpers.IsRole("Moderator", context.User as SocketGuildUser)) return;
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
            if (message.MentionedUsers.Count > 8)
            {
                IGuildUser targetUser = context.Guild.GetUser(message.Author.Id) as IGuildUser;
                IRole moderators = Helpers.ReturnRole(context.Guild, UtilityRoles.Moderator);
                var mutedRole = Helpers.ReturnRole(context.Guild, UtilityRoles.Muted);
                ITextChannel adminlogchannel = context.Guild.GetChannel(Config.AuditChannel) as ITextChannel;

                await targetUser.AddRoleAsync(mutedRole);
                await context.Channel.SendMessageAsync($"stay small {message.Author.Mention}, no spam in my server you little shitter");            
                await adminlogchannel.SendMessageAsync($"{targetUser.Username}#{targetUser.DiscriminatorValue} has been auto muted for mass mention, please investigate {moderators.Mention}");
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
            if (regexInviteLinkDiscord.Match(message.Content).Success & !Helpers.IsModeratorOrOwner(message.Author as SocketGuildUser))
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
            if (messageAfter.Author.IsBot || Helpers.IsModeratorOrOwner(context.User as SocketGuildUser)) return;
            if (regexInviteLinkDiscord.Match(messageAfter.Content).Success)
            {
                var insult = await Insults.GetInsult();
                await context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{context.User.Mention}, don't post invite links {insult}");
            }
            if (messageAfter.MentionedUsers.Count > 8)
            {
                IGuildUser targetUser = context.Guild.GetUser(messageAfter.Author.Id) as IGuildUser;
                IRole moderators = Helpers.ReturnRole(context.Guild, UtilityRoles.Moderator);
                var mutedRole = Helpers.ReturnRole(context.Guild, UtilityRoles.Muted);
                ITextChannel adminlogchannel = context.Guild.GetChannel(Config.AuditChannel) as ITextChannel;

                await targetUser.AddRoleAsync(mutedRole);
                await context.Channel.SendMessageAsync($"stay small {messageAfter.Author.Mention}, no spam in my server you little shitter");
                await adminlogchannel.SendMessageAsync($"{targetUser.Username}#{targetUser.DiscriminatorValue} has been auto muted for mass mention, please investigate {moderators.Mention}");
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
