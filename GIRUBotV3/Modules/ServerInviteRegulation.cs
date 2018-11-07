using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Preconditions;
using Discord.Net;
using System.Linq;
using GIRUBotV3.Models;
using GIRUBotV3.Data;

namespace GIRUBotV3.Modules
{
    public class ServerInviteRegulation : ModuleBase<SocketCommandContext>
    {
        [Command("invite")]
        private async Task GrantSingleInvite()
        {
            var guildChannel = Context.Channel as IGuildChannel;
            var theInvite = await guildChannel.CreateInviteAsync(86400, null, true, true);


            try
            {
                await Context.Message.Author.SendMessageAsync("Here's your invite: " + theInvite.Url);
                return;
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Turn ur DMs on fucking autist, here's your invite: " + theInvite.Url);
                return;
            }
           

        }

    }
}
