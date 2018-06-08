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
    public class Administration : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task KickUser(IGuildUser user, string reason = "cya")
        {

               string kickTargetName = user.Username;
            if (Helpers.IsRole("Moderator", (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            await user.KickAsync(reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} _booted_ {kickTargetName}");
            //embed.WithThumbnailUrl("https://yt3.ggpht.com/a-/AJLlDp3QNvGtiRpzGAvxRx0xQLpjOw1I_knKVT9NJA=s900-mo-c-c0xffffffff-rj-k-no");
            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
           // await Context.Channel.SendMessageAsync($"{Context.User} kicked {kickTargetName} reason: {reason}");
         
            
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUser(IGuildUser user, string reason = "cya")
        {
             string kickTargetName = user.Username;
            if (Helpers.IsRole("Moderator", (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            await user.Guild.AddBanAsync(user, 0, reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} _booted_ {kickTargetName}");      
            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("unban")]
        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task UnbanUser(IGuildUser user)
        {
            var insult = await Insults.GetInsult();
            try
            {
                await Context.Guild.RemoveBanAsync(user);
                await Context.Channel.SendMessageAsync($"✅    *** {user.Nickname} has been unbanned ***");
            }
            catch (HttpException ex)
            {
                await Context.Channel.SendMessageAsync("they're not even banned" + insult);
            }
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task WarnUser(IGuildUser user, [Remainder]string warningMessage)
        {
            try
            {
                await user.SendMessageAsync("You have been warned in Melee Slasher for: " + warningMessage);
                await Context.Channel.SendMessageAsync($"⚠      *** {user.Username} has received a warning.      ⚠***");
            }
            catch (HttpException ex)
            {

                await Context.Channel.SendMessageAsync($"{user.Mention}, {warningMessage}");
            }
        }
 
        [Command("warn")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task WarnUser(IGuildUser user)
        {
               string warningMessage = await Insults.GetWarning();
            try
            {
                await user.SendMessageAsync(warningMessage);
                await Context.Channel.SendMessageAsync($"⚠      *** {user.Username} has received a warning.      ⚠***");
            }
            catch (HttpException ex)
            {
                await Context.Channel.SendMessageAsync(user.Mention + ", " + warningMessage);
            }
        }

        private IRole currentRoleExclusive;
        private IRole roleToAssign;

        [Command("assign")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task Assign(IGuildUser user, string roleSearch)
        {
            var userSocket = user as SocketGuildUser;
            var userCurrentRoles = user.RoleIds;
            string insult = await Insults.GetInsult();

            // validation, is that role assignable, does the user already have it ?
            List<string> allowed_roles = new List<string>();
            allowed_roles.AddMany(
                "Viewers", 
                "PuggersEU",
                "PuggersNA",
                "Roleplayer", 
                "Weeb", 
                "Veterans",
                "Scotland",
                "Armenia",
                "EU", 
                "NA", 
                "RU", 
                "SA", 
                "Oceania", 
                "noob"
                );

            //is it an allowed role ?
            for (int i = 0; i < allowed_roles.Count; i++)
            {
                // if (string.Equals(roleSearch, allowed_roles[i], StringComparison.CurrentCultureIgnoreCase) == true)
                if (roleSearch == allowed_roles[i])
                    {                   
                     roleToAssign = Helpers.ReturnRole(Context.Guild, roleSearch);
                    if (roleToAssign != null)
                    {
                        break;
                    }
                    }
                }
                if (roleToAssign is null)
                {
                    await Context.Channel.SendMessageAsync($"not a valid role, {insult}");
                    return;
                }
         
            if (Helpers.IsRole(roleSearch, userSocket))
            {
                await Context.Channel.SendMessageAsync($"nice? they alrdy have that role {insult}");
                return;
            }

            //user cant have these roles together, finding role to rmeove
            List<string> exclusive_roles = new List<string>();    
                exclusive_roles.AddMany("EU", "NA", "RU", "SA", "Oceania", "noob");

            for (int i = 0; i < exclusive_roles.Count; i++)
            {
                currentRoleExclusive = Helpers.IsRoleReturn(exclusive_roles[i], userSocket);
              //  if (currentRoleExclusive == roleToAssign)
                  if(exclusive_roles.Contains(roleToAssign.Name) && currentRoleExclusive != null)
                {                  
                    var embedReplaceRemovedRole = new EmbedBuilder();
                    embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} granted {roleSearch} to {user.Username}");
                    embedReplaceRemovedRole.WithDescription($"replaced **{currentRoleExclusive.Name}** role");
                    embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                    await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                    await userSocket.RemoveRoleAsync(currentRoleExclusive);
                    await userSocket.AddRoleAsync(roleToAssign);
                    return;
                }
            }
            var embedReplace = new EmbedBuilder();
            embedReplace.WithTitle($"✅   {Context.User.Username} granted {roleSearch} to {user.Username}");
            embedReplace.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplace.Build());
            await userSocket.AddRoleAsync(roleToAssign);
            return;
        }

        private IRole roleToRemove;
        [Command("unassign")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task UnAssign(IGuildUser user, string roleSearch)
        {
            var userSocket = user as SocketGuildUser;
            roleToRemove = Helpers.FindRole(userSocket, roleSearch);

            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} removed {roleSearch} from {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.RemoveRoleAsync(roleToRemove);
            return;
        }

        [Command("mute")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task Mute(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var mutedRole = Helpers.FindRole(userSocket, "Muted");
            if (mutedRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find muted role !");
                return;
            }
            var insult = await Insults.GetInsult();
                if (Helpers.IsRole("Muted", userSocket))
                {
                    await Context.Channel.SendMessageAsync("they already muted u dumbass");
                    return;
                }
                if (Helpers.IsRole("Moderator", userSocket))
                {
                    await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                    return;
                }
            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} successfully muted {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.AddRoleAsync(mutedRole);
            return;
        }

        [Command("unmute")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task UnMute(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var mutedRole = Helpers.FindRole(userSocket, "Muted");
            if (mutedRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find muted role !");
                return;
            }
            var insult = await Insults.GetInsult();

            if (!Helpers.IsRole("Muted", userSocket))
            {
                await Context.Channel.SendMessageAsync("theyre not even muted u " + insult);
                return;
            }
            if (Helpers.IsRole("Moderator", userSocket))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }
            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} successfully unmuted {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.RemoveRoleAsync(mutedRole);
            return;
        }

        [Command("name")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        private async Task SetNick(IGuildUser user, [Remainder]string newName)
        {
            var userSocket = user as SocketGuildUser;
            var currentName = user.Nickname;
            await user.ModifyAsync(x => x.Nickname = newName);
            await Context.Channel.SendMessageAsync("👍");
        }
        [Command("resetname")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        private async Task SetNick(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var currentName = user.Nickname;
            await user.ModifyAsync(x => x.Nickname = user.Username);
            await Context.Channel.SendMessageAsync("name reset 👍");
        }
        [Command("say")]
        [RequireUserPermission(GuildPermission.Administrator)]
        private async Task SayInMain([Remainder]string message)
        {
            var chnl = Context.Guild.GetTextChannel(300832513595670529);          
            await chnl.SendMessageAsync(message);
           //await Context.Channel.SendMessageAsync(message);
            
        }
    }  
}


