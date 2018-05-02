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

            var embed = new EmbedBuilder();
            embed.WithTitle($"{user.Username}'s info");

            embed.WithDescription(
                $"User ID: **{user.Id}** \n" +
                $"Playing: **{user.Game}** \n" +
                $"Account Created: **{user.CreatedAt}** \n" +
                $"joined at: **{user.JoinedAt}** \n" +
                $"User Tag: **{user.Discriminator}** \n" +
                $"Status: **{user.Status}**"
            );
            embed.ThumbnailUrl = avatarURL;
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
            // await Context.Channel.SendMessageAsync($"{Context.User} kicked {kickTargetName} reason: {reason}");       
        }

        [Command("info")]
        public async Task getInfo()
        {
            string avatarURL = Context.User.GetAvatarUrl();
            var caller = Context.User as IGuildUser;

            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Username}'s info");

            embed.WithDescription(
                $"User ID: **{caller.Id}** \n" +
                $"Playing: **{Context.User.Game}** \n" +
                $"Account Created: **{Context.User.CreatedAt}** \n" +
                $"joined at: **{caller.JoinedAt}** \n" +
                $"User Tag: **{Context.User.Discriminator}** \n" +
                $"Status: **{Context.User.Status}**"
            );
                embed.ThumbnailUrl = avatarURL;
                embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
           // await Context.Channel.SendMessageAsync($"{Context.User} kicked {kickTargetName} reason: {reason}");       
        }

        
    }
}
