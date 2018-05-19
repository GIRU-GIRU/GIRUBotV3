using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Preconditions;

namespace GIRUBotV3.Modules
{

    public class Administration : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUser(IGuildUser user, string reason = "cya")
        {
            //if (!Helpers.IsRole("test", (SocketGuildUser)Context.User))
            //{
            //    await Context.Channel.SendMessageAsync(await Insults.GetNoPerm());
            //    return;
            //}
            
           
               string kickTargetName = user.Username;
            if (Helpers.IsRole("test", (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            await user.KickAsync(reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅ {Context.User.Username} _booted_ {kickTargetName}");
            //embed.WithThumbnailUrl("https://yt3.ggpht.com/a-/AJLlDp3QNvGtiRpzGAvxRx0xQLpjOw1I_knKVT9NJA=s900-mo-c-c0xffffffff-rj-k-no");
            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
           // await Context.Channel.SendMessageAsync($"{Context.User} kicked {kickTargetName} reason: {reason}");
         
            
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser user, string reason = "cya")
        {
             string kickTargetName = user.Username;
            if (Helpers.IsRole("test", (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            await user.Guild.AddBanAsync(user, 0, reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅ {Context.User.Username} _booted_ {kickTargetName}");

            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }


        private IRole currentRoleExclusive;

        [Command("assign")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Assign(IGuildUser user, string roleSearch)
        {
            List<string> allowed_roles = new List<string>();
            allowed_roles.AddMany("Viewers", "Puggers", "Roleplayer", "Weeb", "Veterans", "Scotland", "Armenia");

            
            var userSocket = user as SocketGuildUser;
            var roleToAssign = (Helpers.FindRole(userSocket, roleSearch));
            var userCurrentRoles = user.RoleIds;

            if (Helpers.IsRole(roleSearch, userSocket))
            {
                await Context.Channel.SendMessageAsync("um ? they aldry have that role (add get insult)");
                return;
            }

            
            List<string> exclusive_roles = new List<string>();
            
            exclusive_roles.AddMany("EU", "NA", "RU", "SA", "Oceania", "noob");


            for (int i = 0; i < exclusive_roles.Count; i++)
            {
                currentRoleExclusive = Helpers.IsRoleReturn(exclusive_roles[i], userSocket);
            }

            if (currentRoleExclusive == roleToAssign)
            {

                var embedReplace = new EmbedBuilder();
                embedReplace.WithTitle($"✅ {Context.User.Username} granted {roleSearch} to {user.Username}");
                embedReplace.WithDescription($"replaced *\"{currentRoleExclusive.Name}\"* role");
                embedReplace.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplace.Build());

                await userSocket.RemoveRoleAsync(currentRoleExclusive);
                await userSocket.AddRoleAsync(roleToAssign);
                return;
            }
           
            var embed = new EmbedBuilder();
            embed.WithTitle($"✅ {Context.User.Username} granted {roleSearch} to {user.Username}");
            //embed.WithDescription($"✅ {Context.User.Mention} granted {roleSearch} to {user.Mention}");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
            await userSocket.AddRoleAsync(roleToAssign);
        }
    }  
}


