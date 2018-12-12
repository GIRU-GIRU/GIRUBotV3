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
        [Command("give")]
        [Alias("add")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task AssignRoles(SocketGuildUser user, [Remainder]string inputRoles)
        {
            var insult = await Insults.GetInsult();
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser))
            {
                await Context.Channel.SendMessageAsync($"fuck off u {insult}");
                return;
            }

            string[] rolesArray = ReturnValidatedRoles(inputRoles, user);
            IRole roleToRemove = FindExistingExclusiveRoles(user);

            if (rolesArray.Count() == 0 && inputRoles.Count() > 0)
            {
                await Context.Channel.SendMessageAsync($"none of those roles are valid u fucking {insult}");
                return;
            }
            else if (rolesArray.Count() == 0)
            {
                await Context.Channel.SendMessageAsync($"not a valid role u {insult}");
                return;
            }


            List<IRole> iroleCollection = new List<IRole>();
            List<string> roleNameCollection = new List<string>();
            foreach (var role in rolesArray)
            {
                var roleObject = Helpers.ReturnRole(Context.Guild as SocketGuild, role);
                iroleCollection.Add(roleObject);
                roleNameCollection.Add(roleObject.Name);
            }
            string roleNames = String.Join(", ", roleNameCollection.ToArray());

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅   {Context.Message.Author.Username} granted {roleNames} to {user.Username}");
            if (roleToRemove != null)
            {
                await user.RemoveRoleAsync(roleToRemove);
                embed.AddField($"Replaced: ", roleToRemove.Name, true);
            }
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
            await user.AddRolesAsync(iroleCollection);
            return;
        }



        private IRole FindExistingExclusiveRoles(SocketGuildUser user)
        {
            var multiDimensionExclusiveRoles = UserRoles.ExclusiveRolesDictionary.ToArray();
            List<string> singleDimensionExclusiveRoles = new List<string>();
            foreach (var item in multiDimensionExclusiveRoles)
            {
                singleDimensionExclusiveRoles.Add(item.Key.ToLower());
            }

            var foundExclusiveRole = user.Roles.Select(x => x.Name.ToLower())
                     .Intersect(singleDimensionExclusiveRoles)
                        .FirstOrDefault();

            return Helpers.ReturnRole(user.Guild, foundExclusiveRole);
        }



        private string[] ReturnValidatedRoles(string inputRoles, SocketGuildUser user)
        {
            string[] inputRolesArray = inputRoles.ToLower().Split(' ');
            KeyValuePair<string, string>[] multiDimensionAllowedRoles = UserRoles.AllowedRolesDictionary.ToArray();
            //extract allowed roles from the user input
            List<string> singleDimensionAllowedRoles = new List<string>();
            foreach (var item in multiDimensionAllowedRoles)
            {
                singleDimensionAllowedRoles.Add(item.Key.ToLower());
            }
            var resultantRoles = inputRolesArray.Intersect(singleDimensionAllowedRoles).ToArray();


            //user input to now only contains one exclusive role
            var multiDimensionExclusiveRoles = UserRoles.ExclusiveRolesDictionary.ToArray();
            List<string> singleDimensionExclusiveRoles = new List<string>();
            foreach (var item in multiDimensionExclusiveRoles)
            {
                singleDimensionExclusiveRoles.Add(item.Key.ToLower());
            }

            var matchedExclusiveRoles = resultantRoles.Intersect(singleDimensionExclusiveRoles).ToArray();
            if (matchedExclusiveRoles.Count() > 1)
            {
                string[] rolesToRemove = matchedExclusiveRoles.Skip(1).ToArray();
                return resultantRoles = rolesToRemove.Except(resultantRoles).ToArray();
            }
            return resultantRoles;
          
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



