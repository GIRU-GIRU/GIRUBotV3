using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using System.Linq;
using GIRUBotV3.Data;

namespace GIRUBotV3.Modules
{
  public class Avatar : ModuleBase<SocketCommandContext>
    {
        [Command("avatar")]
        private async Task PullAvatar(IGuildUser user)
        {
            string avatarURL = user.GetAvatarUrl(ImageFormat.Auto, 1024);
                
            if (avatarURL is null)
            {
                await Context.Message.Channel.SendMessageAsync($"{user.Mention} does not have a pfp");
                return;
            }
            var embed = new EmbedBuilder();
            embed.WithColor(new Color(0, 204, 255));
            embed.WithTitle($"{user.Username}'s avatar");
            embed.WithUrl(avatarURL);
            embed.WithImageUrl(avatarURL);

            await Context.Channel.SendMessageAsync("", false, embed.Build());       
        }
        [Command("avatar")]
        private async Task PullAvatar()
        {
            string avatarURL = Context.User.GetAvatarUrl(ImageFormat.Auto, 1024);
            if (avatarURL == "null")
            {
                await Context.Message.Channel.SendMessageAsync($"{Context.User.Mention} is 1 of those losers with no pfp lmao");
                return;
            }
            var embed = new EmbedBuilder();
            embed.WithColor(new Color(0, 204, 255));
            embed.WithTitle($"{Context.User.Username}'s avatar");
            embed.WithUrl(avatarURL);
            embed.WithImageUrl(avatarURL);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }    
}