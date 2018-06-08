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
            
            var userSocketGuild = user as SocketGuildUser;
            string userAvatarURL = user.GetAvatarUrl();
            string userStatus = user.Status.ToString();
            var userCreatedAtString = user.CreatedAt.ToString("yyyy/MM/dd hh:mm");
            var userJoinedAtString = user.JoinedAt.Value.ToString("yyyy/MM/dd hh:mm");
            var userDiscriminator = user.Discriminator;
            string userActivity;
                 if (user.Activity is null)
                 {
                     userActivity = "nothing";
                 }
                 else
                 {
                     userActivity = user.Activity.Name;
                 }
          //  var userPerms = userSocketGuild.GetPermissions(Context.Channel as IGuildChannel);
           // var joinedAtString = user.JoinedAt.ToString().Substring(user.JoinedAt.ToString().Length, -5);

            var embed = new EmbedBuilder();
            
            embed.WithTitle($"{user.Username}{userDiscriminator}");
            embed.AddField("Account Created: ", userCreatedAtString, true);
            embed.AddField("Joined Melee Slasher: ", userJoinedAtString, true);
            embed.AddField("User ID: ", user.Id, false);
            embed.AddField("Currently playing ", userActivity, true);
            embed.AddField("Status: ", userStatus, true);
            embed.ThumbnailUrl = userAvatarURL;
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());     
        }

        [Command("info")]
        private async Task GetInfo()
        {
            string avatarURL = Context.User.GetAvatarUrl();
            var user = Context.User as IGuildUser;
            var callerSocketGuild = Context.User as SocketGuildUser;
            // string userGame = callerSocketGuild.Game.ToString();
            var userSocketGuild = user as SocketGuildUser;
            string userAvatarURL = user.GetAvatarUrl();
            string userStatus = user.Status.ToString();
            var userCreatedAtString = user.CreatedAt.ToString("yyyy/MM/dd  hh:mm");
            var userJoinedAtString = user.JoinedAt.Value.ToString("yyyy/MM/dd  hh:mm");
            var userDiscriminator = user.Discriminator;
            string userActivity;
                  if (user.Activity is null)
                  {
                      userActivity = "nothing";
                  }
                  else
                  {
                      userActivity = user.Activity.Name;
                  }

            var embed = new EmbedBuilder();

            embed.WithTitle($"{user.Username}{userDiscriminator}");
            embed.AddField("Account Created: ", userCreatedAtString, true);
            embed.AddField("Joined Melee Slasher: ", userJoinedAtString, true);
            embed.AddField("User ID: ", user.Id, false);
            embed.AddField("Currently playing ", userActivity, true);
            embed.AddField("Status: ", userStatus, true);
            embed.ThumbnailUrl = userAvatarURL;
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        //[Command("emoji")]
        //private async Task EmojiURL(IGuildUser user, Emoji emoteInput)
        //{
        //    var emoteURL = emoteInput.
        //    var embed = new EmbedBuilder();
        //    embed.WithTitle($"{emoteInput.Name} URL");
        //    embed.WithImageUrl(emoteURL);
        //    embed.WithColor(new Color(0, 204, 255));
        //    await Context.Channel.SendMessageAsync("", false, embed.Build());
        //}
    }
}
