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
    public class Administration : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task KickUser(IGuildUser user)
        {
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
        }
        [Command("kick")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task KickUser(IGuildUser user, [Remainder]string reason)
        {

            if (reason.Length < 1)
            {
                reason = "cya";
            }
            string kickTargetName = user.Username;
            if (Helpers.IsRole(UtilityRoles.Moderator, (SocketGuildUser)user))
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

            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, user as SocketGuildUser);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("couldn't be arsed storing their roles b4 i kicked");
            }
        }


        [Command("ban")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task BanUser(IGuildUser user)
        {
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUser(IGuildUser user, [Remainder]string reason)
        {
            string kickTargetName = user.Username;
            if (Helpers.IsRole(UtilityRoles.Moderator, (SocketGuildUser)user))
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

            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, user as SocketGuildUser);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("couldn't be arsed storing their roles b4 i kicked");
            }
        }

        [Command("bancleanse")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task BanUserAndClean(IGuildUser user)
        {
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
        }
        [Command("bancleanse")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUserAndClean(IGuildUser user, [Remainder]string reason)
        {
            string kickTargetName = user.Username;
            if (Helpers.IsRole(UtilityRoles.Moderator, (SocketGuildUser)user))
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

            try
            {
                var storeRoles = new StoreRoleMethods();
                await storeRoles.StoreUserRoles(Context, user as SocketGuildUser);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("couldn't be arsed storing their roles b4 i kicked");
            }
        }

        //[Command("hackban")]
        //[RequireUserPermission(GuildPermission.ViewAuditLog)]
        //[RequireBotPermission(GuildPermission.BanMembers)]
        //private async Task HackBanUser(string input)
        //{
        //    ulong userID = Convert.ToUInt64(input);
        //    try
        //    {
        //        if (Helpers.IsRole(UtilityRoles.Moderator, Context.Guild.GetUser(userID)))
        //        {
        //            await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
        //            return;
        //        }
        //    }
        //    catch (Exception)
        //    { }

        //    try
        //    {
        //      var targetedUser =  _client.GetUser(userID);
        //        await Context.Guild.AddBanAsync(userID);
        //        var embed = new EmbedBuilder();
        //        embed.WithTitle($"✅     {Context.User.Username} hackbanned {targetedUser.Username +"#"+ targetedUser.Discriminator}");
        //        embed.WithColor(new Color(0, 255, 0));
        //        await Context.Channel.SendMessageAsync("", false, embed.Build());
        //    }
        //    catch (Exception)
        //    {
        //        await Context.Channel.SendMessageAsync("Invalid userID");
        //        throw;
        //    }
        //}

        bool existingBan;
        string bannedUserName;
        [Command("unban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
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

            if (existingBan == false)
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
     
        string currentName;
        [Command("name")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        private async Task SetNick(IGuildUser user, [Remainder]string newName)
        {
            var userSocket = user as SocketGuildUser;
            currentName = user.Nickname;
            if (string.IsNullOrEmpty(user.Nickname))
            {
                currentName = user.Username;
            }
            await user.ModifyAsync(x => x.Nickname = newName);
            await Context.Message.DeleteAsync();

            var embedReplaceRemovedRole = new EmbedBuilder();
            embedReplaceRemovedRole.WithTitle($"✅ {currentName} had their name changed successfully");
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
    }
}



