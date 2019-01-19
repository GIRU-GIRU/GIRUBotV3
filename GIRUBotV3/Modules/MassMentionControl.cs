using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public  class MassMentionControl
    {
        public async Task MassMentionMute(SocketCommandContext context, SocketUserMessage message)
        {
            IGuildUser targetUser = context.Guild.GetUser(message.Author.Id) as IGuildUser;
            IRole moderators = Helpers.ReturnRole(context.Guild, UtilityRoles.Moderator);
            var mutedRole = Helpers.ReturnRole(context.Guild, UtilityRoles.Muted);
            ITextChannel adminlogchannel = context.Guild.GetChannel(Config.AuditChannel) as ITextChannel;

            await targetUser.AddRoleAsync(mutedRole);
            await context.Channel.SendMessageAsync($"stay small {message.Author.Mention}, no spam in my server you little shitter");
            await adminlogchannel.SendMessageAsync($"{targetUser.Username}#{targetUser.DiscriminatorValue} has been auto muted for mass mention, please investigate {moderators.Mention}");
        }  
    }
}
