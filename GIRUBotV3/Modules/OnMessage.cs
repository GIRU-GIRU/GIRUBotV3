using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Models;
using GIRUBotV3.Personality;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GIRUBotV3.Modules
{
    public class OnMessage : ModuleBase<SocketCommandContext>

    {
        private static DiscordSocketClient _client;
        private MassMentionControl _massMentionControl;
        private InviteLinkPreventation _inviteLinkPreventation;
        private Nountest _nounTest;
        public OnMessage(DiscordSocketClient client)
        {
            _client = client;
            _massMentionControl = new MassMentionControl();
            _inviteLinkPreventation = new InviteLinkPreventation();
            _nounTest = new Nountest();
        }

        private static Regex regexInviteLinkDiscord = new Regex(@"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/.+[a-z]");
        public async Task MessageContainsAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (Nountest.CheckForNountest(message.Content.Split(" ")[0]))
            {
                await _nounTest.PostNounTest(context);
            }
            if (message.Content.Contains("😃"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("😃");
                }
            }

            if (await WordFilter.CheckForNaughtyWords(message.Content)) await WordFilter.PunishNaughtyWord(context);

            


            if (message.Author.IsBot || Helpers.IsModeratorOrOwner(message.Author as SocketGuildUser)) return;

            if (Helpers.OnOffExecution(context.Message) == true) await context.Message.DeleteAsync();


            if (message.MentionedUsers.Count > 8) await _massMentionControl.MassMentionMute(context, message);

            if (regexInviteLinkDiscord.Match(message.Content).Success & !Helpers.IsModeratorOrOwner(message.Author as SocketGuildUser))
            {
                await _inviteLinkPreventation.DeleteInviteLinkWarn(context);
            }

            if(message.Author.Id == 215903917630947328)
            {
                var rnd = new Random();

                if (rnd.Next(1, 10) <= 2)
                {
                    await context.Channel.SendMessageAsync($"kuffsone is a weirdo stalker btw ({message.Author.Mention})");
                }            
            }
        }

        public async Task UpdatedMessageContainsAsync(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var messageAfter = after as SocketUserMessage;
            var context = new SocketCommandContext(_client, messageAfter);
            if (messageAfter.Content.Contains("😃"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("😃");
                }
            }

            if (await WordFilter.CheckForNaughtyWords(after.Content)) await WordFilter.PunishNaughtyWord(context);

            if (messageAfter.Author.IsBot || Helpers.IsModeratorOrOwner(context.User as SocketGuildUser)) return;
            if (regexInviteLinkDiscord.Match(messageAfter.Content).Success)
            {
                await _inviteLinkPreventation.DeleteInviteLinkWarn(context);
            }
            if (messageAfter.MentionedUsers.Count > 8) await _massMentionControl.MassMentionMute(context, messageAfter);


        }
    }
}
