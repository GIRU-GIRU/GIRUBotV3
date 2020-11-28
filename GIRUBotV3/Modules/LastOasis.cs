using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Models;
using GIRUBotV3.AdministrativeAttributes;
using System.Linq;

namespace GIRUBotV3.Modules
{
    public class LastOasis : ModuleBase<SocketCommandContext>
    {

        [Command("emergency")]
        [IsLastOasis]
        private async Task LastOasisEmergency()
        {
            try
            {

                await Context.Channel.SendMessageAsync("Write a reason!");

            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        [Command("emergency")]
        [IsLastOasis]
        private async Task LastOasisEmergency([Remainder]string inputMessage)
        {
            try
            {
                if (Context.Message.Author.Id == 185464319641649153 || Context.Message.Author.Id == 347462177017430017) return;

                if (Context.Message.Channel.Id == 693097204675510333)
                {
                    IRole LastOasisRole = Helpers.ReturnRole(Context.Guild, "🌴 Last Oasis");
                    await LastOasisRole.ModifyAsync(x => x.Mentionable = true);

                    var embed = new EmbedBuilder();
                    embed.WithTitle("EMERGENCY IN LAST OASIS");
                    embed.WithDescription($"Called by {Context.Message.Author}\n{inputMessage}");
                    embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/693097204675510333/696842499729260575/zNID4uCK_400x400.png";
                    embed.WithColor(new Color(0, 204, 255));
                    await Context.Channel.SendMessageAsync(LastOasisRole.Mention, false, embed.Build());

                    await LastOasisRole.ModifyAsync(x => x.Mentionable = false);
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        [Command("lorecruit")]
        [IsLastOasis]
        private async Task LastOasisRecruitUser(SocketGuildUser targetUser)
        {
            try
            {
                var loLeaderRole = Helpers.ReturnRole(Context.Guild, "Last Oasis Leader");
                var author = Context.Message.Author as SocketGuildUser;

                if (author.Roles.Where(x => x.Id == loLeaderRole.Id).Any())
                {
                    var loRole = Helpers.ReturnRole(Context.Guild, "🌴 Last Oasis");

                    if (targetUser.Roles.Where(x => x.Id == loRole.Id).Any())
                    {
                        await Context.Channel.SendMessageAsync("User already is part of the crew");
                    }
                    else
                    {
                        await targetUser.AddRoleAsync(loRole);
                        await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} recruited {targetUser.Mention} to the Last Oasis team");
                    }

                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        [Command("loremove")]
        [IsLastOasis]
        private async Task LastOasisRemoveUser(SocketGuildUser targetUser)
        {
            try
            {
                var loLeaderRole = Helpers.ReturnRole(Context.Guild, "Last Oasis Leader");
                var author = Context.Message.Author as SocketGuildUser;

                if (author.Roles.Where(x => x.Id == loLeaderRole.Id).Any())
                {
                    var loRole = Helpers.ReturnRole(Context.Guild, "🌴 Last Oasis");

                    if (!targetUser.Roles.Where(x => x.Id == loRole.Id).Any())
                    {
                        await Context.Channel.SendMessageAsync("User isn't part of the crew anyway m8");
                    }
                    else
                    {
                        await targetUser.RemoveRoleAsync(loRole);
                        await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} removed {targetUser.Mention} from the Last Oasis team");
                    }

                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }


        [Command("loallyrecruit")]
        [IsLastOasis]
        private async Task LastOasisAllyRecruit(SocketGuildUser targetUser)
        {
            try
            {
                var loLeaderRole = Helpers.ReturnRole(Context.Guild, "Last Oasis Leader");
                var author = Context.Message.Author as SocketGuildUser;

                if (author.Roles.Where(x => x.Id == loLeaderRole.Id).Any())
                {
                    var loAllyRole = Helpers.ReturnRole(Context.Guild, "Last Oasis Ally");

                    if (targetUser.Roles.Where(x => x.Id == loAllyRole.Id).Any())
                    {
                        await Context.Channel.SendMessageAsync("User already is already an ally");
                    }
                    else
                    {
                        await targetUser.AddRoleAsync(loAllyRole);
                        await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} recruited {targetUser.Mention} to the Last Oasis allies");
                    }

                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        [Command("loallyremove")]
        [IsLastOasis]
        private async Task LastOasisAllyRemove(SocketGuildUser targetUser)
        {
            try
            {
                var loLeaderRole = Helpers.ReturnRole(Context.Guild, "Last Oasis Leader");
                var author = Context.Message.Author as SocketGuildUser;

                if (author.Roles.Where(x => x.Id == loLeaderRole.Id).Any())
                {
                    var loAllyRole = Helpers.ReturnRole(Context.Guild, "Last Oasis Ally");

                    if (!targetUser.Roles.Where(x => x.Id == loAllyRole.Id).Any())
                    {
                        await Context.Channel.SendMessageAsync("User isn't an ally anyway m8");
                    }
                    else
                    {
                        await targetUser.RemoveRoleAsync(loAllyRole);
                        await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} removed {targetUser.Mention} from the Last Oasis allies");
                    }

                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }



    }
}



