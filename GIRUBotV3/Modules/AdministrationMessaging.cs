﻿using Discord;
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
    public class AdministrationMessaging : ModuleBase<SocketCommandContext>
    {
        [Command("bancleanse")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task BanUserAndClean(SocketGuildUser user)
        {


            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        private async Task KickUser(SocketGuildUser user)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
            await Context.Channel.SendMessageAsync("write more shit for the log retard");
        }
        [Command("searchban")]
        private async Task searchBan([Remainder]string input = null)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
            if (input == null) return;

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
        [Command("warn")]
        private async Task WarnUserCustom(IGuildUser user, [Remainder]string warningMessage)
        {
            if (Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)
                || Helpers.IsSonya(Context.Message.Author as SocketGuildUser))
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
            return;
        }

        [Command("say")]
        private async Task SayCustomMessage([Remainder]string input)
        {
            try
            {
                if (Context.Message.MentionedChannels.Count == 0)
                {
                    await Context.Channel.SendMessageAsync("You must mention the channel: (#channel_name)");
                    return;
                }

                var targetChannel = Context.Message.MentionedChannels.FirstOrDefault() as ITextChannel;
                var targetChannelAsString = $"<#{targetChannel.Id}>";
                var sanitizedInput = input.Replace(targetChannelAsString, string.Empty);

                await targetChannel.SendMessageAsync(sanitizedInput);
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync(ex.Message);
            }
        }


        [Command("warn")]
        private async Task WarnUser(IGuildUser user)
        {
            if (Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)
                || Helpers.IsSonya(Context.Message.Author as SocketGuildUser))
            {
                string warningMessage = await Insults.GetWarning();
                try
                {
                    await user.SendMessageAsync(warningMessage);
                    await Context.Channel.SendMessageAsync($"⚠      *** {user.Username} has received a warning.      ⚠***");
                }
                catch (HttpException)
                {
                    await Context.Channel.SendMessageAsync(user.Mention + ", " + warningMessage);
                }
            }
            return;
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
                await user.ModifyAsync(x => x.Nickname = user.Username);
                await Context.Channel.SendMessageAsync("name reset 👍");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("no");
            }

        }

        [Command("resetallname")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        private async Task MassChangeNicknames([Remainder]string input)
        {
            if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

            try
            {
                await Context.Channel.SendMessageAsync($"Changing all names like {input}");

                var users = await Context.Channel.GetUsersAsync(CacheMode.AllowDownload).FlattenAsync();
                var socketGuildUsers = new List<SocketGuildUser>();

                foreach (var user in users)
                {
                    var socketGuildUser = user as SocketGuildUser;

                    if (socketGuildUser != null)
                    {
                        if (!string.IsNullOrEmpty(socketGuildUser.Nickname))
                        {
                            if (socketGuildUser.Nickname.ToLower().Contains(input.ToLower()))
                            {
                                socketGuildUsers.Add(socketGuildUser);
                            }
                        }
                    }
                }


                foreach (var u in socketGuildUsers)
                {
                    await u.ModifyAsync(x => x.Nickname = u.Username);
                    await Context.Channel.SendMessageAsync($"Reset {u.Username}");
                }

                await Context.Channel.SendMessageAsync("Finished resetting names");

            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"no, {ex.Message}");
            }

        }
    }
}



