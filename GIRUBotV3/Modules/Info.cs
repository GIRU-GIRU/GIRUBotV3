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
    public class Info : ModuleBase<SocketCommandContext>
    {
        ImageFormat png = ImageFormat.Png;
        [Command("info")]
        private async Task GetInfo(IGuildUser user)
        {

            var userSocketGuild = user as SocketGuildUser;
            string userAvatarURL = user.GetAvatarUrl(png, 1024);
            string userStatus = user.Status.ToString();
            var userCreatedAtString = user.CreatedAt.ToString("yyyy/MM/dd hh:mm");
            var userJoinedAtString = user.JoinedAt.Value.ToString("yyyy/MM/dd hh:mm");
            var userDiscriminator = user.Discriminator;
            string userActivity = user.Activity == null ? "nothing" : user.Activity.Name;


            var embed = new EmbedBuilder();
            embed.WithTitle($"{user.Username}{userDiscriminator}");
            embed.AddField("Account Created: ", userCreatedAtString, true);
            embed.AddField("Joined Melee Slasher: ", userJoinedAtString, true);
            embed.AddField("User ID: ", user.Id, false);
            embed.AddField("Currently playing ", userActivity, true);
            embed.AddField("Status: ", userStatus, true);
            embed.WithThumbnailUrl(userAvatarURL);
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("info")]
        private async Task GetInfo()
        {
            string avatarURL = Context.User.GetAvatarUrl();
            var user = Context.User as IGuildUser;
            var callerSocketGuild = Context.User as SocketGuildUser;
            var userSocketGuild = user as SocketGuildUser;
            string userAvatarURL = user.GetAvatarUrl(png, 1024);
            string userStatus = user.Status.ToString();
            var userCreatedAtString = user.CreatedAt.ToString("yyyy/MM/dd  hh:mm");
            var userJoinedAtString = user.JoinedAt.Value.ToString("yyyy/MM/dd  hh:mm");
            var userDiscriminator = user.Discriminator;
            string userActivity = user.Activity == null ? "nothing" : user.Activity.Name;

            var embed = new EmbedBuilder();
            embed.WithTitle($"{user.Username}{userDiscriminator}");
            embed.AddField("Account Created: ", userCreatedAtString, true);
            embed.AddField("Joined Melee Slasher: ", userJoinedAtString, true);
            embed.AddField("User ID: ", user.Id, false);
            embed.AddField("Currently playing ", userActivity, true);
            embed.AddField("Status: ", userStatus, true);
            embed.WithThumbnailUrl(userAvatarURL); ;
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("serverinfo")]
        private async Task GetServerInfo()
        {
            var g = Context.Guild;

            var createdAt = g.CreatedAt.ToString("yyyy/MM/dd  hh:mm");
            var roleCount = g.Roles.Count;
            var mmbrCount = g.MemberCount;
            var image = g.IconUrl;
            var owner = g.Owner.Username;
            var channels = g.TextChannels.Count;
            var guildID = g.Id;

            var embed = new EmbedBuilder();
            embed.WithTitle(g.Name);
            embed.AddField("Member Count: ", mmbrCount, true);
            embed.AddField("Created at: ", createdAt, true);
            embed.AddField("Role Count: ", roleCount, true);
            embed.AddField("Text Channel Count: ", channels, true);
            embed.AddField("Guild ID: ", guildID, true);
            embed.AddField("Owner: ", owner, true);
            embed.WithThumbnailUrl(image);
            embed.WithColor(new Color(0, 204, 255));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }


        [Command("stream")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task StreamAnnounce()
        {
            if (!Helpers.IsSonya(Context.Message.Author as SocketGuildUser)) return;
            IRole viewerRole = Helpers.ReturnRole(Context.Guild, UtilityRoles.Viewers);
            await viewerRole.ModifyAsync(x => x.Mentionable = true);
            await Context.Channel.SendMessageAsync(viewerRole.Mention + ", Stream is now live! https://www.twitch.tv/giru");
            await viewerRole.ModifyAsync(x => x.Mentionable = false);
            return;
        }

        [Command("github")]
        private async Task GithubURLPost()
        {
            await Context.Channel.SendMessageAsync("https://github.com/GIRU-GIRU/GIRUBotV3");
            return;
        }
    }
}
