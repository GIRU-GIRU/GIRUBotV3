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

namespace GIRUBotV3.Modules
{
    public class Administration : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task KickUser(IGuildUser user, [Remainder]string reason)
        {
            if (reason.Length < 3)
            {
                reason = "cya";
            }
            string kickTargetName = user.Username;
            if (Helpers.IsRole("Moderator", (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            await user.KickAsync(reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} _booted_ {kickTargetName}");
         
            embed.WithDescription($"reason: **{reason}**");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
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
            embed.WithTitle($"✅     {Context.User.Username} banned {kickTargetName}");
            embed.WithDescription($"reason: _{reason}_");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("bancleanse")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUserAndClean(IGuildUser user, string reason = "cya")
        {
            string kickTargetName = user.Username;
            if (Helpers.IsRole("Moderator", (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            await user.Guild.AddBanAsync(user, 1, reason);

            var embed = new EmbedBuilder();
            embed.WithTitle($"✅     {Context.User.Username} banned & cleansed {kickTargetName}");
            embed.WithDescription($"reason: _{reason}_");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("hackban")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task HackBanUser(string input)
        {
            ulong userID = Convert.ToUInt64(input);
            if (Helpers.IsRole("Moderator", Context.Guild.GetUser(userID)))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }
            try
            {
                await Context.Guild.AddBanAsync(userID);
                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} hackbanned userID {userID.ToString()}");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Invalid userID");
                throw;
            }


        }

        bool existingBan;
        string bannedUserName;
        [Command("unban")]
        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task UnbanUser(ulong userID)
        {
            var insult = await Insults.GetInsult();
            var allBans = await Context.Guild.GetBansAsync();
            foreach (var item in allBans)
            {
                if (item.User.Id == userID)
                {
                    existingBan = true;
                    bannedUserName = item.User.Username;
                    break;
                }
                else
                {
                    existingBan = false;
                }
            }

            if (existingBan != true)
            {
                await Context.Channel.SendMessageAsync("that's not a valid ID " + insult);
            }
            await Context.Guild.RemoveBanAsync(userID);
            await Context.Channel.SendMessageAsync($"✅    *** {bannedUserName} has been unbanned ***");
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task WarnUserCustom(IGuildUser user, [Remainder]string warningMessage)
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
            var allowedRoles = typeof(Models.AllowedRoles).GetProperties();

            foreach (var item in allowedRoles)
            {
                if (roleSearch.ToLower() == item.Name.ToLower())
                {
                    roleToAssign = Helpers.ReturnRole(Context.Guild, roleSearch);
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

            //user cant have these roles together, finding role to rmeove
            var exclusiveRoles = typeof(Models.ExclusiveRoles).GetProperties();

            foreach(var singleExclusiveRole in exclusiveRoles)
                {
                    currentRoleExclusive = Helpers.IsRoleReturn(singleExclusiveRole.Name, userSocket);
                    //  if (currentRoleExclusive == roleToAssign)
                    if (currentRoleExclusive != null)
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
        

        
        [Command("del")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task UnAssign(IGuildUser user, string roleSearch)
        {
            var insult = await Insults.GetInsult();
            var userSocket = user as SocketGuildUser;
            var roleToRemove = Helpers.ReturnRole(userSocket.Guild, roleSearch);

            if (roleToRemove==null)
            {
                await Context.Channel.SendMessageAsync("how am i supposed to remove a role that dosen't exist you " + insult);
                return;
            }   

            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅   {Context.User.Username} removed {roleToRemove.Name} from {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            await userSocket.RemoveRoleAsync(roleToRemove);
            return;
        }
         
        [Command("mute")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
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
        [RequireUserPermission(GuildPermission.ManageMessages)]
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
            await Context.Message.DeleteAsync();
         
            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅ {Context.Message.Author} namechanged {currentName} => {user.Username}");
            embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
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



        [Command("off")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task TurnOffUser(IGuildUser user)
        {
            OnOffUser.TurnedOffUsers.Add(user);
            await Context.Channel.SendMessageAsync($"*turned off {user.Mention}*");
            return;  
        }

        [Command("on")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task TurnOnUserAsync(IGuildUser user)
        {
            var userToRemove = OnOffUser.TurnedOffUsers.Find(x => x.Id == user.Id);
            OnOffUser.TurnedOffUsers.Remove(userToRemove);
            await Context.Channel.SendMessageAsync($"*{user.Mention} turned back on*");
            return;
        }
        [Command("cant")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        private async Task CantPostPics(IGuildUser user)
        {
            var userSocket = user as SocketGuildUser;
            var picsRole = Helpers.FindRole(userSocket, "Can't post pics");
            if (picsRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find cpp role !");
                return;
            }
            var insult = await Insults.GetInsult();
            if (Helpers.IsRole("Can't post pics", userSocket))
            {
                await Context.Channel.SendMessageAsync("they already cant u dumbass");
                return;
            }
            if (Helpers.IsRole("Moderator", userSocket))
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
            var picsRole = Helpers.FindRole(userSocket, "Can't post pics");
            if (picsRole is null)
            {
                await Context.Channel.SendMessageAsync("cant find cant post pics role !");
                return;
            }
            var insult = await Insults.GetInsult();

            if (!Helpers.IsRole("Can't post pics", userSocket))
            {
                await Context.Channel.SendMessageAsync("they can post pics u " + insult);
                return;
            }
            if (Helpers.IsRole("Moderator", userSocket))
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
    }
}

