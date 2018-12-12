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
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task KickUser(SocketGuildUser user, [Remainder]string reason)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            if (reason.Length < 1) reason = "cya";
            string kickTargetName = user.Username;

            if (Helpers.IsRole(UtilityRoles.Moderator, (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }

            try
            {
                await user.KickAsync(reason);
                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} _booted_ {kickTargetName}");
                embed.WithDescription($"reason: **{reason}**");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync("unable to kick user ! " + ex.Message);
            }

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
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUser(SocketGuildUser user, [Remainder]string reason)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            string kickTargetName = user.Username;
            if (Helpers.IsRole(UtilityRoles.Moderator, (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }

            try
            {
                await user.Guild.AddBanAsync(user, 0, reason);

                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} banned {kickTargetName}");
                embed.WithDescription($"reason: _{reason}_");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync("unable to ban user ! " + ex.Message);
            }

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
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUserAndClean(SocketGuildUser user, [Remainder]string reason)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            string kickTargetName = user.Username;
            if (Helpers.IsRole(UtilityRoles.Moderator, (SocketGuildUser)user))
            {
                await Context.Channel.SendMessageAsync("stop fighting urselves u retards");
                return;
            }

            try
            {
                await user.Guild.AddBanAsync(user, 1, reason);

                var embed = new EmbedBuilder();
                embed.WithTitle($"✅     {Context.User.Username} banned & cleansed {kickTargetName}");
                embed.WithDescription($"reason: _{reason}_");
                embed.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync("unable to bancleanse user ! " + ex.Message);
            }

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

        private bool existingBan;
        private string bannedUserName;
        [Command("unban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task UnbanUser(ulong userID)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

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


        [Command("off")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task TurnOffUser(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            OnOffUser.TurnedOffUsers.Add(user);
            await Context.Channel.SendMessageAsync($"*turned off {user.Mention}*");
            return;
        }

        [Command("on")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task TurnOnUserAsync(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            var userToRemove = OnOffUser.TurnedOffUsers.Find(x => x.Id == user.Id);
            OnOffUser.TurnedOffUsers.Remove(userToRemove);
            await Context.Channel.SendMessageAsync($"*{user.Mention} turned back on*");
            return;
        }
    }
}



