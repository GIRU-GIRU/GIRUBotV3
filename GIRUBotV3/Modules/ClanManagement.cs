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
    public class ClanManagement : ModuleBase<SocketCommandContext>
    {

        [Command("clankick")]
        private async Task KickClanMember(SocketGuildUser user)
        {
            try
            {
                ClanStorageMethods clanManager = new ClanStorageMethods();

                if (await clanManager.CheckIfExistingClanLeader(Context.Message.Author.Id))
                {
                    if (await clanManager.CheckIfExistingClanmember(user.Id))
                    {
                        bool successful = await clanManager.KickClanMember(Context.Message.Author.Id, user.Id);

                        if (successful)
                        {
                            string clanName = await clanManager.GetClanName(Context.Message.Author.Id);

                            await Context.Channel.SendMessageAsync($"{user.Mention} was booted from {clanName}");
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync($"nah dont want to");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"Clan kicking user failed, {ex.Message}");
            }
        }

        [Command("clanleader")]
        private async Task AssignNewClanLeader(SocketGuildUser user, [Remainder]string inputClanName)
        {
            try
            {
                if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

                ClanStorageMethods clanManager = new ClanStorageMethods();

                if (!await clanManager.CheckIfExistingClanLeader(Context.Message.Author.Id))
                {
                    if (await clanManager.AssignNewClanleader(inputClanName, user.Username, user.Id))
                    {
                        await Context.Channel.SendMessageAsync($"{user.Mention} was successfully made leader of {inputClanName}");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"{user.Mention} was unable to be made leader of {inputClanName}");
                    }                
                }
                else
                {
                    string clanName = await clanManager.GetClanName(user.Id);

                    await Context.Channel.SendMessageAsync($"{user.Mention} is already the clan leader of {clanName}");
                }
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"Clan leader assign failed, {ex.Message}");
            }
        }



        [Command("clanrecruit")]
        private async Task AssignNewClanMember(SocketGuildUser user)
        {
            try
            {
                ClanStorageMethods clanManager = new ClanStorageMethods();

                if (await clanManager.CheckIfExistingClanLeader(Context.Message.Author.Id))
                {
                    var clanName = clanManager.GetClanName(Context.Message.Author.Id);

                    if (await clanManager.AssignNewClanMember(Context.Message.Author.Id, user.Username, user.Id))
                    {
                        await Context.Channel.SendMessageAsync($"{user.Username} was successfully recruited to {clanName}");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"{user.Username} was unable to be recruited to {clanName}");
                    }
                }
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"Error recruiting member, {ex.Message}");
            }
        }

        [Command("clancreate")]
        private async Task CreateNewClan([Remainder]string clanName)
        {
            try
            {
                if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

                ClanStorageMethods clanManager = new ClanStorageMethods();

                Clan clan = new Clan()
                {
                    ClanName = clanName,
                    DateCreated = DateTime.UtcNow,
                };

                if (await clanManager.CreateNewClan(clan))
                {
                    await Context.Channel.SendMessageAsync($"{clanName} was successfully created as a new clan!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{clanName} was unable to be created");
                }

            }
            catch (Exception ex)
            {

                await Context.Channel.SendMessageAsync($"Error creating new clan, {ex.Message} ");
            }
        }

        [Command("clandelete")]
        private async Task DeleteClan([Remainder]string clanName)
        {
            try
            {
                if (!Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser)) return;

                ClanStorageMethods clanManager = new ClanStorageMethods();

                if (await clanManager.DeleteClan(clanName))
                {
                   await Context.Channel.SendMessageAsync($"{clanName} was successfully deleted");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{clanName} was unable be deleted");
                }              
            }
            catch (Exception ex)
            {

                await Context.Channel.SendMessageAsync($"error occured {ex.Message}");
            }
        }
    }
}



