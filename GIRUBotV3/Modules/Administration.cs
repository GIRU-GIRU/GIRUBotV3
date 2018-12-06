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
        private async Task KickUser(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
        }
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
        [Command("searchban")]
        private async Task searchBan([Remainder]string input = null)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            if (input == null)
            {
                return;
            }

            var bans = await Context.Guild.GetBansAsync();
            List<Discord.Rest.RestBan> matchedBans = new List<Discord.Rest.RestBan>();
            foreach (var ban in bans)
            {
                if (ban.User.Username.ToLower().Contains(input.ToLower()))
                {
                    matchedBans.Add(ban);
                }
            }

            if (bans.Count == 0 || matchedBans.Count == 0)
            {
                await Context.Channel.SendMessageAsync("couldn't find anyone sry");
                return;
            }

            var bannedUserNames = string.Join("\n", matchedBans.Select(x => x.User.Username).ToArray());

            var embed = new EmbedBuilder();
            embed.WithTitle($"Banned user names matching \"{input}\" ");
            embed.AddField("Username", bannedUserNames, true);
            embed.AddField("User ID: ", string.Join("\n", matchedBans.Select(x => x.User.Id)), true);
            embed.AddField("Reason for ban: ", string.Join("\n", matchedBans.Select(x => x.Reason)), true);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("ban")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task BanUser(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
            await Context.Channel.SendMessageAsync("write more shit for the retard");
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
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task BanUserAndClean(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
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

        [Command("say")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task SayInMain([Remainder]string message)
        {
            var chnl = Context.Guild.GetTextChannel(Config.MeleeSlasherMainChannel);
            await chnl.SendMessageAsync(message);
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
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
        [Command("bancleanse")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUserAndCleanse()
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Bans & Cleanses a {insult} from this sacred place");
            embed.WithDescription("**Usage**: !ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Chat Purge**: 24 hours. \n" +
                "**Ban length:** Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUser()
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Permanently ends some {insult} from this sacred place");
            embed.WithDescription("**Usage**: !ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Length**: Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        string currentName;
        [Command("name")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        private async Task SetNick(SocketGuildUser user, [Remainder]string newName)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            var userSocket = user as SocketGuildUser;
            currentName = user.Nickname;
            if (string.IsNullOrEmpty(user.Nickname)) currentName = user.Username;

            try
            {
                await user.ModifyAsync(x => x.Nickname = newName);
                await Context.Message.DeleteAsync();

                var embedReplaceRemovedRole = new EmbedBuilder();
                embedReplaceRemovedRole.WithTitle($"✅ {currentName} had their name changed successfully");
                embedReplaceRemovedRole.WithColor(new Color(0, 255, 0));
                await Context.Channel.SendMessageAsync("", false, embedReplaceRemovedRole.Build());
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("no");
            }

        }
        [Command("resetname")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        private async Task SetNick(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            try
            {
                var userSocket = user as SocketGuildUser;
                var currentName = user.Nickname;
                await user.ModifyAsync(x => x.Nickname = user.Username);
                await Context.Channel.SendMessageAsync("name reset 👍");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("no");
            }

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



