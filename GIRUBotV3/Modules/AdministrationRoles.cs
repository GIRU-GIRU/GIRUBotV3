using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Preconditions;
using Discord.Net;
using System.Linq;
using GIRUBotV3.Models;
using GIRUBotV3.Data;

namespace GIRUBotV3.Modules
{
    public class AdministrationRoles : ModuleBase<SocketCommandContext>
    {     
        List<string> RolesNamesList = new List<string>();
        List<IRole> RolesToAdd = new List<IRole>();
        [Command("give")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task AssignMultiple(IGuildUser user, [Remainder]string inputRoles)
        {
            string insult = await Insults.GetInsult();
            var userSocket = user as SocketGuildUser;
            var userCurrentRoles = user.RoleIds;
           
            var inputRolesArray = inputRoles.ToLower().Split(' ');

            List<string> allowedRolesList = new List<string>();
            foreach (var item in AllowedRoles.AllowedRolesDictionary)
            {
                allowedRolesList.Add(item.Key.ToLower());
            }

            //validate matching roles
            var succesfullyMatchingList = inputRolesArray.Where(x => allowedRolesList.Contains(x, StringComparer.InvariantCultureIgnoreCase)).ToList();

            //remove regional roles from bulk add           
            foreach (var item in Models.ExclusiveRoles.Exclusive_roles)
            {
                succesfullyMatchingList.Remove(item.ToLower());
            }

            if (succesfullyMatchingList.Count == 0)
            {
                await Context.Channel.SendMessageAsync($"not gonna let you give that you {insult}");
                return;
            }


            List<string> succesfullyMatchingListValues = new List<string>();
            //grab role values rahter than alias
            foreach (var item in AllowedRoles.AllowedRolesDictionary)
            {
                for (int i = 0; i < succesfullyMatchingList.Count; i++)
                {
                    if (item.Key.ToLower() == succesfullyMatchingList[i].ToLower())
                    {
                        succesfullyMatchingListValues.Add(item.Value);
                    }
                }

            }
            //grab the IRole objects and populate the return string
            List<IRole> roleList = new List<IRole>();
            for (int i = 0; i < succesfullyMatchingListValues.Count; i++)
            {
                var returnedRole = Helpers.ReturnRole(Context.Guild, succesfullyMatchingListValues[i]);
                roleList.Add(returnedRole);
                RolesNamesList.Add(returnedRole.Name);
            }

            var rolesAsString = string.Join(", ", RolesNamesList.ToArray());
            var embedReplace = new EmbedBuilder();
            embedReplace.WithTitle($"✅   {Context.User.Username} granted {rolesAsString} to {user.Username}");
            embedReplace.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplace.Build());
            await userSocket.AddRolesAsync(roleList);
            return;
        }

        private IRole currentRoleExclusive;
        private IRole roleToAssign;

        [Command("add")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task Assign(IGuildUser user, string roleSearch)
        {
            var userSocket = user as SocketGuildUser;
            var userCurrentRoles = user.RoleIds;
            string insult = await Insults.GetInsult();

            //is it an allowed role ? 
            foreach (var item in AllowedRoles.AllowedRolesDictionary)
            {
                if (roleSearch.ToLower() == item.Key.ToLower())
                {
                    roleToAssign = Helpers.ReturnRole(Context.Guild, item.Value);
                    break;
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

            //user cant have these roles together, finding role to remove
            List<string> exclusive_roles = Models.ExclusiveRoles.Exclusive_roles;
            for (int i = 0; i < exclusive_roles.Count; i++)
            {
                currentRoleExclusive = Helpers.IsRoleReturn(exclusive_roles[i], userSocket);
                if (exclusive_roles.Contains(roleToAssign.Name) && currentRoleExclusive != null)
                {
                    var embedReplaceRemovedRole = new EmbedBuilder();
                    embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} granted {roleToAssign.Name} to {user.Username}");
                    embedReplaceRemovedRole.WithDescription($"replaced **{currentRoleExclusive.Name}** role");
                    embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                    await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                    await userSocket.RemoveRoleAsync(currentRoleExclusive);
                    await userSocket.AddRoleAsync(roleToAssign);
                    return;
                }
            }
            var embedReplace = new EmbedBuilder();
            embedReplace.WithTitle($"✅   {Context.User.Username} granted {roleToAssign.Name} to {user.Username}");
            embedReplace.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplace.Build());
            await userSocket.AddRoleAsync(roleToAssign);
            return;
        }

        List<IRole> RolesToRemove = new List<IRole>();
        List<string> RolesToRemoveNames = new List<string>();
        [Command("del")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task UnAssign(IGuildUser user, [Remainder]string roleSearch)
        {
            var insult = await Insults.GetInsult();
            var userSocket = user as SocketGuildUser;
            var embedReplaceRemovedRole = new EmbedBuilder();

            if (!roleSearch.Contains(' '))
            {
                var roleToRemove = Helpers.ReturnRole(userSocket.Guild, roleSearch);
                embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} removed {roleToRemove.Name} from {user.Username}");
                embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
                await userSocket.RemoveRoleAsync(roleToRemove);
                return;

            }
            var inputRolesArray = roleSearch.ToLower().Split(' ');
            foreach (var item in inputRolesArray)
            {
                var returnedRole = Helpers.ReturnRole(userSocket.Guild, item);
                RolesToRemove.Add(returnedRole);
                RolesToRemoveNames.Add(returnedRole.Name);
            }

            var rolesAsString = string.Join(", ", RolesToRemoveNames.ToArray());
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} removed {rolesAsString} from {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.RemoveRolesAsync(RolesToRemove);
            return;
        }

        [Command("mute")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task Mute(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var mutedRole = Helpers.FindRole(userSocket, UtilityRoles.Muted);
            if (mutedRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find muted role !");
                return;
            }
            var insult = await Insults.GetInsult();
            if (Helpers.IsRole(UtilityRoles.Muted, userSocket))
            {
                await Context.Channel.SendMessageAsync("they already muted u dumbass");
                return;
            }
            if (Helpers.IsRole(UtilityRoles.Moderator, userSocket))
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
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task UnMute(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var mutedRole = Helpers.FindRole(userSocket, UtilityRoles.Muted);
            if (mutedRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find muted role !");
                return;
            }
            var insult = await Insults.GetInsult();

            if (!Helpers.IsRole(UtilityRoles.Muted, userSocket))
            {
                await Context.Channel.SendMessageAsync("theyre not even muted u " + insult);
                return;
            }
            if (Helpers.IsRole(UtilityRoles.Moderator, userSocket))
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
       
        [Command("cant")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task CantPostPics(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var picsRole = Helpers.FindRole(userSocket, UtilityRoles.PicPermDisable);
            if (picsRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find cpp role !");
                return;
            }
            var insult = await Insults.GetInsult();
            if (Helpers.IsRole(UtilityRoles.PicPermDisable, userSocket))
            {
                await Context.Channel.SendMessageAsync("they already cant u dumbass");
                return;
            }
            if (Helpers.IsRole(UtilityRoles.Moderator, userSocket))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }
            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} removed the pic perms of {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.AddRoleAsync(picsRole);
            return;
        }

        [Command("can")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task CanPostPics(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var picsRole = Helpers.FindRole(userSocket, UtilityRoles.PicPermDisable);
            if (picsRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find cant post pics role !");
                return;
            }
            var insult = await Insults.GetInsult();

            if (!Helpers.IsRole(UtilityRoles.PicPermDisable, userSocket))
            {
                await Context.Channel.SendMessageAsync("they can post pics u " + insult);
                return;
            }
            if (Helpers.IsRole(UtilityRoles.Moderator, userSocket))
            {
                await Context.Channel.SendMessageAsync("stop beefing with eachother fucking bastards");
                return;
            }
            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} returned pic perms for {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.RemoveRoleAsync(picsRole);
            return;
        }
        [Command("storeroles")]
        private async Task StoreRoles(SocketGuildUser target)
        {
            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, target);
                await Context.Channel.SendMessageAsync($"{target.Username} successfully had their roles stored");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"smth went wrong {ex.Message}");
            }
        }
        [Command("storeroles")]
        private async Task StoreRoles(ulong ID)
        {
            try
            {
                SocketGuildUser target = Context.Guild.GetUser(ID);
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, target);
                await Context.Channel.SendMessageAsync($"{target.Username} successfully had their roles stored");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"smth went wrong {ex.Message}");
            }
        }

        [Command("restoreroles")]
        private async Task RestoreRoles(SocketGuildUser target)
        {
            try
            {
                var restoreRoles = new StoreRoleMethods();
                await restoreRoles.RestoreUserRoles(Context, target);
                await Context.Channel.SendMessageAsync($"FINE..  {target.Username} successfully had their roles restored");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"smth went wrong {ex.Message}");
            }
        }

    }
}



