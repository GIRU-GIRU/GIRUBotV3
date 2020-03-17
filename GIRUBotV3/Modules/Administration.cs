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
                await ExceptionHandler.HandleExceptionPublically(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
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
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
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

        [Command("hackban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task HackBanUser([Remainder]string input)
        {
            try
            {
                if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
                if (!input.Contains(' '))
                {
                    var insult = await Personality.Insults.GetInsult();
                    await Context.Channel.SendMessageAsync($"syntax is \"{Config.CommandPrefix}hackban uintID reason\" you fucking {insult}");
                    return;
                }

                string[] inputArray = input.Split(' ');

                if (!ulong.TryParse(inputArray[0], out ulong targetID))
                {
                    var insult = await Personality.Insults.GetInsult();
                    await Context.Channel.SendMessageAsync($"Unable to parse that ID {insult} cunt");
                    return;
                }

                string reason = string.Join(' ', inputArray.Skip(1));

                await Context.Guild.AddBanAsync(targetID, 0, reason);
                await Context.Channel.SendMessageAsync($"Banned userID {targetID}, reason: {reason}");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"uhhh, somethign went wrong: {ex.Message}");
                throw;
            }
        }

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

        [Command("colour")]
        private async Task ChangeSonyaRoleColour(string inputColour)
        {
            try
            {
                var user = Context.User as SocketGuildUser;
                if (!user.Roles.Where(x => x.Name.ToLower() == "sonya").Any()) return;

                var sonyaRole = Helpers.ReturnRole(Context.Guild, "sonya");
                string conversion = "0x" + inputColour.Replace("#", "");

                var colorHex = Convert.ToUInt32(conversion, 16);
                var color = new Discord.Color(colorHex);

                if (colorHex != 0)
                {
                    await sonyaRole.ModifyAsync(x =>
                    {
                        x.Color = color;
                    });
                    await Context.Channel.SendMessageAsync($"Colour successfully changed to {inputColour}");
                }
                else
                {
                    var insult = await Personality.Insults.GetInsult();
                    await Context.Channel.SendMessageAsync($"what the FUCK is that supposed to be? retard {insult}");
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionPublically(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
                throw;
            }
        }



        [Command("vcmove")]
        private async Task VCMove(SocketGuildUser user, [Remainder]string chnlName)
        {
            try
            {
                if (!Helpers.IsVKOrModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;
                if (Context.Message.Author.Id == 161176590028505089) return;

                var insult = await Personality.Insults.GetInsult();
                IVoiceChannel targetChannel = null;

                if (user.Id == Context.Message.Author.Id)
                {
                    await Context.Channel.SendMessageAsync($"yeah nice try retard {insult}");
                    return;
                }

                if (user.VoiceChannel != null)
                {

                    var sortedVoiceList = Context.Guild.VoiceChannels.OrderBy(x => x.Position).ToArray();

                    foreach (var vc in sortedVoiceList)
                    {
                        if (vc.Name.ToLower() == chnlName.ToLower()
                            || vc.Name.ToLower().Contains(chnlName.ToLower()))
                        {
                            targetChannel = vc as IVoiceChannel;
                            break;
                        }
                    }

                    if (targetChannel == null)
                    {
                        await Context.Channel.SendMessageAsync($"thats not a real channel {insult}");
                        return;
                    }

                    var channel = Optional.Create<IVoiceChannel>(targetChannel);

                    var newChannel = channel.Value as IVoiceChannel;
                    var oldChannel = user.VoiceChannel as IVoiceChannel;

                    if (newChannel.Id == oldChannel.Id)
                    {
                        await Context.Channel.SendMessageAsync($"why would i move him to the same channel you fucking {insult}");
                        return;
                    }

                    await user.ModifyAsync(x =>
                    {
                        x.Channel = channel;
                    });

                    await Context.Channel.SendMessageAsync($"{user.Mention} moved from \"{oldChannel.Name}\" to \"{newChannel.Name}\"");
                    return;

                };

                await Context.Channel.SendMessageAsync($"{user.Mention} needs to connect to a voice channel to be moved");

            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"Move failed.. {ex.Message}");
            }
        }



        
    }
}



