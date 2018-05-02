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
    public class Info : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public async Task getInfo(IGuildUser user)
        {
            string avatarURL = user.GetAvatarUrl();
            var userSocketGuild = user as SocketGuildUser;
            string userGame = user.Game.ToString();

            if (userGame == "**")
            {
                userGame = "Nothing";
            }
            var embed = new EmbedBuilder();
            
            embed.WithTitle($"{user.Username}'s info");
            embed.WithDescription(
                $"User ID: **{user.Id}**        User Tag: **{user.Discriminator}** \n" +
                $"Playing: **{userGame}**      Status: **{user.Status}**\n" +
                $"Account Created: **{user.CreatedAt}** \n" +
                $"joined at: **{user.JoinedAt}** \n" +
                $"Roles: **{userSocketGuild.Roles}**" 
                
            );
            embed.ThumbnailUrl = avatarURL;
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed);     
        }

        [Command("info")]
        public async Task getInfo()
        {
            string avatarURL = Context.User.GetAvatarUrl();
            var caller = Context.User as IGuildUser;
            var callerSocketGuild = Context.User as SocketGuildUser;
            var embed = new EmbedBuilder();

            embed.WithTitle($"{Context.User.Username}'s info");
            embed.WithDescription(
                    $"User ID: **{caller.Id}**    User Tag: **{caller.Discriminator}** \n" +
                $"Playing: **{caller.Game}**      Status: **{caller.Status}**\n" +
                $"Account Created: **{caller.CreatedAt}** \n" +
                $"joined at: **{caller.JoinedAt}** \n" +
                $"Roles: **{callerSocketGuild.Roles}**"
            );
                embed.ThumbnailUrl = avatarURL;
                embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed);      
        }

        
    }
}
