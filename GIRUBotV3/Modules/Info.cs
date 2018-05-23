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
        private async Task GetInfo(IGuildUser user)
        {
            string avatarURL = user.GetAvatarUrl();
            var userSocketGuild = user as SocketGuildUser;
         
            string userStatus = user.Status.ToString();

            if (userStatus == "")
            {
                userStatus = "Somewhere";
            }

            var embed = new EmbedBuilder();
            
            embed.WithTitle($"{user.Username}'s info");

                 embed.AddField("User ID: ", user.Id, true);
                 embed.AddField("User Tag: ", user.Discriminator, true);
                 //embed.AddField("Playing: ", userGame, true);
                 embed.AddField("Status: ", userStatus, true);
                 embed.AddField("Account Created: ", user.CreatedAt, true);
                 embed.AddField("Joined at: ", user.JoinedAt, true);
            embed.ThumbnailUrl = avatarURL;
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());     
        }

        [Command("info")]
        private async Task GetInfo()
        {
            string avatarURL = Context.User.GetAvatarUrl();
            var caller = Context.User as IGuildUser;
            var callerSocketGuild = Context.User as SocketGuildUser;
           // string userGame = callerSocketGuild.Game.ToString();
            string userStatus = callerSocketGuild.Status.ToString();

            if (userStatus == "")
            {
                userStatus = "Somewhere";
            }
            var embed = new EmbedBuilder();
            
            embed.WithTitle($"{Context.User.Username}'s info");

                 embed.WithTitle($"{caller.Username}'s info");
                 embed.AddField("User ID: ", caller.Id, true);
                 embed.AddField("User Tag: ", caller.Discriminator, true);
              // embed.AddField("Playing: ", userGame);
                 embed.AddField("Status: ", userStatus, true);
                 embed.AddField("Account Created: ", caller.CreatedAt, true);
                 embed.AddField("Joined at: ", caller.JoinedAt, true);        
            embed.ThumbnailUrl = avatarURL;
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());      
        }
    }
}
