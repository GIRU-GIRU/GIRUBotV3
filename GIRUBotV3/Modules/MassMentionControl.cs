using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class MassMentionControl
    {
        public async Task MassMentionMute(SocketCommandContext context, SocketUserMessage message)
        {
            try
            {
                IGuildUser targetUser = context.Guild.GetUser(message.Author.Id) as IGuildUser;
                IRole moderators = Helpers.ReturnRole(context.Guild, UtilityRoles.Moderator);
                var mutedRole = Helpers.ReturnRole(context.Guild, "SuperMuted");
                ITextChannel adminlogchannel = context.Guild.GetChannel(Config.AuditChannel) as ITextChannel;

                await targetUser.AddRoleAsync(mutedRole);
                await context.Channel.SendMessageAsync($"stay small {message.Author.Mention}, no spam in my server you little shitter");
                await adminlogchannel.SendMessageAsync($"{targetUser.Username}#{targetUser.DiscriminatorValue} has been auto muted for mass mention, please investigate {moderators.Mention}");
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }

        }

        public async Task<bool> CheckForMentionSpam(SocketCommandContext context)
        {
            bool detectedMentionSpam = false;

            try
            {
                if (context.Message.MentionedUsers.Count > 7)
                {

                    var mentionedUsers = context.Message.MentionedUsers;


                    detectedMentionSpam = !mentionedUsers.GroupBy(x => x.Id)
                                                              .Where(g => g.Count() > 7)
                                                                  .Any();


                }              
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }

            return detectedMentionSpam;
        }
    }
}
