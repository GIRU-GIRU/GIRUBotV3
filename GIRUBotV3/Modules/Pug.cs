using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class Pug : ModuleBase<SocketCommandContext>
    {

        [Command("eupug")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task PugAnnounceEU([Remainder]string pugMessage)
        {
            var PuggersRole = Context.Guild.GetRole(Helpers.ReturnRole(Context.Guild, "PuggersEU").Id);
            PuggersRole.Permissions.Modify(mentionEveryone: true);

  
                IRole puggersEU = Helpers.ReturnRole(Context.Guild, "PuggersEU");
                var embed = new EmbedBuilder();
                embed.WithTitle("An EU Pug is commencing... ");
                embed.WithDescription($"Hosted by {Context.Message.Author}\n{pugMessage} \n \n{puggersEU.Mention}");
                embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/449997696682688522/451383069329588224/helmet.PNG";
                embed.WithColor(new Color(0, 204, 255));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                return;          
        }

        [Command("napug")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task PugAnnounceNA([Remainder]string pugMessage)
        {
                IRole puggersNA = Helpers.ReturnRole(Context.Guild, "PuggersNA");
                var embed = new EmbedBuilder();
                embed.WithTitle("An NA Pug is commencing... ");
                embed.WithDescription($"Hosted by {Context.Message.Author}\n{pugMessage} \n \n{puggersNA.Mention}");
                embed.ThumbnailUrl = "https://i.ytimg.com/vi/-gTbxqK-4yU/maxresdefault.jpg";
                embed.WithColor(new Color(0, 204, 255));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                return;
        }
    }
}
