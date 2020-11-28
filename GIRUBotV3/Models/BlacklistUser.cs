using Discord.WebSocket;
using GIRUBotV3.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Models
{
    public static class BlacklistUser
    {
        public static List<SocketUser> BlackListedUser { get; } = new List<SocketUser>();

        public static async Task<bool> CheckBlacklist(SocketUser user)
        {
            bool userIsBlacklisted = false;
            try
            {
                if (BlackListedUser != null && BlackListedUser.Count > 0)
                {

                    userIsBlacklisted = BlackListedUser.Contains(user);
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly("Blacklist", ExceptionHandler.GetAsyncMethodName(), ex);
            }

            return userIsBlacklisted;
        }
    }
}
