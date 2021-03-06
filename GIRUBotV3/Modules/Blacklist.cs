﻿using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.AdministrativeAttributes;
using GIRUBotV3.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class Blacklist : ModuleBase<SocketCommandContext>
    {
        [Command("blacklist")]
        [IsModerator]
        public async Task GetBlacklistedUsers()
        {
            var BlacklistedUserList = String.Join(", ", Models.BlacklistUser.BlackListedUser);

            await Context.Channel.SendMessageAsync("Currently blacklisted: " + BlacklistedUserList);
        }

        [Command("blacklist")]
        [IsModerator]
        public async Task BlacklistUser(SocketUser user)
        {
            if (user.Id == Global.Config.OwnerID)
            {
                await Context.Channel.SendMessageAsync("i wont blacklist myself retard");
                return;
            }

            if (Models.BlacklistUser.BlackListedUser.Contains(user))
            {
                Models.BlacklistUser.BlackListedUser.Remove(user);
                await Context.Channel.SendMessageAsync("unblacklisted " + user.Username);
            }
            else
            {
                Models.BlacklistUser.BlackListedUser.Add(user);
                await Context.Channel.SendMessageAsync("blacklisted " + user.Username);
            }
        }
    }
}