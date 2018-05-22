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
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
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
            try
            {
                await Context.Guild.RemoveBanAsync(user);
                await Context.Channel.SendMessageAsync($"✅    *** {user.Nickname} has been unbanned ***");
            }
            catch (HttpException ex)
            {
                await Context.Channel.SendMessageAsync("they're not even banned weirdo");
            }
        }

    

    [Command("warn")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task WarnUser(IGuildUser user, string warningMessage)
        {
            try
            {
                await user.SendMessageAsync(warningMessage);
                await Context.Channel.SendMessageAsync($"✅    *** {Context.User.Username} has been warned ***");
            }
            catch (HttpException ex)
            {

                await Context.Channel.SendMessageAsync(warningMessage);
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
                await Context.Channel.SendMessageAsync($"✅    *** {Context.User.Username} has been warned ***");
            }
            catch (HttpException ex)
            {

                await Context.Channel.SendMessageAsync(warningMessage);
            }
        }



        private IRole currentRoleExclusive;

        [Command("assign")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task Assign(IGuildUser user, string roleSearch)
        {
            // validation, is that role assignable, does the user already have it ?
            List<string> allowed_roles = new List<string>();
            allowed_roles.AddMany(
                "Viewers", 
                "Puggers", 
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

            
            var userSocket = user as SocketGuildUser;
            var roleToAssign = (Helpers.FindRole(userSocket, roleSearch));
            var userCurrentRoles = user.RoleIds;

            if (Helpers.IsRole(roleSearch, userSocket))
            {
                await Context.Channel.SendMessageAsync("um ? they aldry have that role (add get insult)");
                return;
            }

            for (int i = 0; i < allowed_roles.Count; i++)
            {
                var isAllowedRole = Helpers.IsRoleReturn(allowed_roles[i], userSocket);
                if (roleToAssign != isAllowedRole)
                {
                    await Context.Channel.SendMessageAsync("no ur not allowed to give " + roleSearch);
                    return;
                }              
            }

            //user cant have these roles together
            List<string> exclusive_roles = new List<string>();    
            exclusive_roles.AddMany("EU", "NA", "RU", "SA", "Oceania", "noob");

            for (int i = 0; i < exclusive_roles.Count; i++)
            {
                currentRoleExclusive = Helpers.IsRoleReturn(exclusive_roles[i], userSocket);
            }

            if (currentRoleExclusive == roleToAssign)
            {

                var embedReplace = new EmbedBuilder();
                embedReplace.WithTitle($"✅   {Context.User.Username} granted {roleSearch} to {user.Username}");
                embedReplace.WithDescription($"replaced *\"{currentRoleExclusive.Name}\"* role");
                embedReplace.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplace.Build());

                await userSocket.RemoveRoleAsync(currentRoleExclusive);
                await userSocket.AddRoleAsync(roleToAssign);
                return;
            }
           
            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} granted {roleSearch} to {user.Username}");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
            await userSocket.AddRoleAsync(roleToAssign);
        }
    }  
}


