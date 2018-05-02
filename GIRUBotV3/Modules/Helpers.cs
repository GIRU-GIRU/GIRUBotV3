using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;

namespace GIRUBotV3.Personality
{
    public static class Helpers
    {
        //IS ROLE - is target moderator, etc. ?
        public static bool IsRole(string role, SocketGuildUser user)
        {
        
            
            var result = from r in user.Guild.Roles
                         where r.Name == role
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            //first or default NEVER returns null
            if (roleID == 0)
            {
                return false;
            } else
            {
                var targetRole = user.Guild.GetRole(roleID);
                return user.Roles.Contains(targetRole);
            }
            
        }

        public static string FindEmoji(SocketGuildUser user, string emojiName)
        {
 

            var result = from r in user.Guild.Emotes
                         where r.Name == emojiName
                         select r.Id;
            ulong emojiID = result.FirstOrDefault();
            //first or default NEVER returns null
            if (emojiID == 0)
            {
                Console.WriteLine($"Could not find {emojiName} emoji");
                return "🤔";
            }
            else
            {
                
               string finalEmoji = $"<:{emojiName}:{emojiID}>";
                return finalEmoji;
            }

        }
    }
}
//var result = from r in user.Guild.Emotes
//             where r.Name == emojiName
//             select r.Name;
//var emojiID = result.ToString();
//first or default NEVER returns null
//if (emojiID is null)
//{
//    Console.WriteLine($"Could not find {emojiName} emoji");
//    return "🤔";
//}
//else
//{
//    return emojiID.ToString();
//}
