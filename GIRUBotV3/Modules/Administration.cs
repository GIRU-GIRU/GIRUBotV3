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
    public class Administration : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUser(IGuildUser user, string reason = "cya")
        {
            if (!Helpers.IsRole("test", (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(await Insults.GetNoPerm());
                return;
            }

                string kickTargetName = user.Username;
                await user.KickAsync(reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Username} _booted_ {kickTargetName}");
            embed.WithThumbnailUrl("https://yt3.ggpht.com/a-/AJLlDp3QNvGtiRpzGAvxRx0xQLpjOw1I_knKVT9NJA=s900-mo-c-c0xffffffff-rj-k-no");
            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
           // await Context.Channel.SendMessageAsync($"{Context.User} kicked {kickTargetName} reason: {reason}");
         
            
        }

        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser user, string reason = "cya")
        {
            if (!Helpers.IsRole("test", (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(await Insults.GetNoPerm());
                return;
            }

            string kickTargetName = user.Username;
            await user.Guild.AddBanAsync(user, 0, reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Username} _booted_ {kickTargetName}");

            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
