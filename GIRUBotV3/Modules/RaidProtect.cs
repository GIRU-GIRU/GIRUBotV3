using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Preconditions;
using Discord.Net;

namespace GIRUBotV3.Modules
{
    public class RaidProtect : ModuleBase<SocketCommandContext>
    {
        OverwritePermissions allowTalk = new OverwritePermissions(sendMessages: PermValue.Allow);
        OverwritePermissions denyTalk = new OverwritePermissions(sendMessages: PermValue.Deny);

        [Command("raidprotect on")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task RaidProtectOn()
        {
            var chnl = Context.Channel as SocketGuildChannel;
            await chnl.AddPermissionOverwriteAsync(Helpers.ReturnRole(Context.Guild, "noob"), denyTalk);

            var embed = new EmbedBuilder();
              embed.WithTitle("Raid Protection Enabled      ⭕");
              embed.WithDescription($"disabled freedom of speech for noobs\n\nshitters now unable to talk in {chnl.Name}");
              embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/300832513595670529/469942372034150440/detailed_helmet_discord_embed.png";
              embed.WithColor(new Color(255, 0, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("raidprotect off")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task RaidProtectOff()
        {
            var chnl = Context.Channel as SocketGuildChannel;
            await chnl.AddPermissionOverwriteAsync(Helpers.ReturnRole(Context.Guild, "noob"), allowTalk);

            var embed = new EmbedBuilder();
              embed.WithTitle("Raid Protection Disabled  ❎");
              embed.WithDescription($"enabled freedom of speech for noobs\n\nshitters now able to talk in {chnl.Name}");
              embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/300832513595670529/469942372034150440/detailed_helmet_discord_embed.png";
              embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
