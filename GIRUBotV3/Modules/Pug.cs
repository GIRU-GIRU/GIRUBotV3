using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Models;

namespace GIRUBotV3.Modules
{
    public class Pug : ModuleBase<SocketCommandContext>
    {
       

        [Command("pugeu")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task PugAnnounceEU([Remainder]string pugMessage)
        {

            IRole puggersEU = Helpers.ReturnRole(Context.Guild, AllowedRoles.AllowedRolesDictionary.GetValueOrDefault("PugEU"));
            await puggersEU.ModifyAsync(x => x.Mentionable = true);

            var embed = new EmbedBuilder();
            embed.WithTitle("An EU Pug is commencing... ");
            embed.WithDescription($"Hosted by {Context.Message.Author}\n{pugMessage}");
            embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/449997696682688522/451383069329588224/helmet.PNG";
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync(puggersEU.Mention, false, embed.Build());

            await puggersEU.ModifyAsync(x => x.Mentionable = false);
            return;
        }

        [Command("pugna")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task PugAnnounceNA([Remainder]string pugMessage)
        {
            IRole puggersNA = Helpers.ReturnRole(Context.Guild, AllowedRoles.AllowedRolesDictionary.GetValueOrDefault("PugNA"));
            await puggersNA.ModifyAsync(x => x.Mentionable = true);

            var embed = new EmbedBuilder();
            embed.WithTitle("An NA Pug is commencing... ");
            embed.WithDescription($"Hosted by {Context.Message.Author}\n{pugMessage}");
            embed.ThumbnailUrl = "https://i.ytimg.com/vi/-gTbxqK-4yU/maxresdefault.jpg";
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync(puggersNA.Mention, false, embed.Build());

            await puggersNA.ModifyAsync(x => x.Mentionable = false);
            return;
        }
    }
}
