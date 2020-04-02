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
using GIRUBotV3.AdministrativeAttributes;

namespace GIRUBotV3.Modules
{
    public class AdministrationMessaging : ModuleBase<SocketCommandContext>
    {
        [Command("bancleanse")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [IsModerator]
        private async Task BanUserAndClean(SocketGuildUser user)
        {
            string insult = await Personality.Insults.GetInsult();
            await Context.Channel.SendMessageAsync($"write more shit for the log {insult}");
        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [IsModerator]
        private async Task KickUser(SocketGuildUser user)
        {
            string insult = await Personality.Insults.GetInsult();
            await Context.Channel.SendMessageAsync($"write more shit for the log {insult}");
        }

        [Command("searchban")]
        [IsModerator]
        private async Task searchBan([Remainder]string input = null)
        {
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
        [IsModerator]
        private async Task BanUser(SocketGuildUser user)
        {
            string insult = await Personality.Insults.GetInsult();
            await Context.Channel.SendMessageAsync($"write more shit for the log {insult}");
        }


        [Command("warn")]
        [IsModerator]
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
        [IsModerator]
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
                await ExceptionHandler.HandleExceptionPublically(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }


        [Command("warn")]
        [IsModerator]
        private async Task WarnUser(IGuildUser user)
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


        [Command("bancleanse")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [IsModerator]
        private async Task BanUserAndCleanse()
        {
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
        [IsModerator]
        private async Task BanUser()
        {
            

            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Permanently ends some {insult} from this sacred place");
            embed.WithDescription("**Usage**: !ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Length**: Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        
        [Command("name")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        [IsModerator]
        private async Task SetNick(SocketGuildUser user, [Remainder]string newName)
        {
            string currentName = user.Nickname;

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
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }

        }
        [Command("resetname")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        [IsModerator]
        private async Task SetNick(SocketGuildUser user)
        {
            try
            {
                await user.ModifyAsync(x => x.Nickname = user.Username);
                await Context.Channel.SendMessageAsync("name reset 👍");
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        [Command("resetallname")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        [IsModerator]
        private async Task MassChangeNicknames([Remainder]string input)
        {
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
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }

        }
    }
}



