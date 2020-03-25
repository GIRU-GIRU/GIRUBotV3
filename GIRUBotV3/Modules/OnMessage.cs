using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Models;
using GIRUBotV3.Personality;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

            await NountestCheck(context);

            await AdditionalChatSmileyCheck(context);

            await NaughtyWordFilter(context);



            if (message.Author.IsBot || Helpers.IsModeratorOrOwner(message.Author as SocketGuildUser)) return;

            await TurnedOffMessagesCheck(context);

            await MassMentionCheck(context);

            await InviteLinkCheck(context);

            await KuffsoneWeirdoCheck(context);
        }

        public async Task UpdatedMessageContainsAsync(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var messageAfter = after as SocketUserMessage;
            var context = new SocketCommandContext(_client, messageAfter);


            await AdditionalChatSmileyCheck(context);

            await NaughtyWordFilter(context);


            if (messageAfter.Author.IsBot || Helpers.IsModeratorOrOwner(context.User as SocketGuildUser)) return;

            await InviteLinkCheck(context);

            await MassMentionCheck(context);
        }




        private async Task KuffsoneWeirdoCheck(SocketCommandContext context)
        {
            try
            {
                if (context.Message.Author.Id == 215903917630947328)
                {
                    var rnd = new Random();

                    if (rnd.Next(1, 25) <= 2)
                    {
                        await context.Channel.SendMessageAsync($"kuffsone is a weirdo stalker btw ({context.Message.Author.Mention})");
                    }
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        private async Task InviteLinkCheck(SocketCommandContext context)
        {

            try
            {
                if (regexInviteLinkDiscord.Match(context.Message.Content).Success & !Helpers.IsModeratorOrOwner(context.Message.Author as SocketGuildUser))
                {
                    await _inviteLinkPreventation.DeleteInviteLinkWarn(context);
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        private async Task MassMentionCheck(SocketCommandContext context)
        {
            try
            {
                if (context.Message.MentionedUsers.Count > 8) await _massMentionControl.MassMentionMute(context, context.Message);
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }

        }

        private async Task TurnedOffMessagesCheck(SocketCommandContext context)
        {
            try
            {
                if (Helpers.OnOffExecution(context.Message) == true) await context.Message.DeleteAsync();
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        private async Task NaughtyWordFilter(SocketCommandContext context)
        {
            try
            {
                if (context.Message.Content != null)
                {
                    if (await WordFilter.CheckForNaughtyWords(context.Message.Content)) await WordFilter.PunishNaughtyWord(context);
                }
                
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        private async Task AdditionalChatSmileyCheck(SocketCommandContext context)
        {
            try
            {
                if (context.Message.Content.Contains("😃"))
                {
                    var r = new Random();
                    if (r.Next(1, 15) <= 2)
                    {
                        await context.Channel.SendMessageAsync("😃");
                    }
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }

        }

        private async Task NountestCheck(SocketCommandContext context)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(context.Message.Content))
                {
                    if (Nountest.CheckForNountest(context.Message.Content.Split(" ")[0]))
                    {
                        await _nounTest.PostNounTest(context);
                    }
                }

            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }


    }
}

