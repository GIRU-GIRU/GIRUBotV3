using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace GIRUBotV3.Models
{
    public static class BlacklistUser
    {
        public static List<SocketUser> BlackListedUser { get; } = new List<SocketUser>();
    }
}
