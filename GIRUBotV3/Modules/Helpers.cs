using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;

namespace GIRUBotV3.Modules
{
    public static class Helpers
    {
        public static bool IsRole(string role, SocketGuildUser user)
        {
            var result = from r in user.Guild.Roles
                         where r.Name.ToLower() == role.ToLower()
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            //first or default NEVER returns null
            if (roleID == 0)
            {
                return false;
            }
            else
            {
                var targetRole = user.Guild.GetRole(roleID);
                return user.Roles.Contains(targetRole);
            }
        }
        public static IRole ReturnRole(SocketGuild guild, string role)
        {
            return guild.GetRole(guild.Roles.FirstOrDefault(x => x.Name.ToLower() == role.ToLower()).Id);
        }
        public static IRole IsRoleReturn(string role, SocketGuildUser user)
        {
            //return user.Guild.GetRole(user.Guild.Roles.FirstOrDefault(x => x.Name == role).Id);     
            var result = from r in user.Roles
                         where r.Name.ToLower() == role.ToLower()
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
          
            if (roleID == 0)
            {
            
                return user.Guild.GetRole(0);
            }
            else
            {
                
                return user.Guild.GetRole(roleID);
            }
        }

        public static string FindEmoji(SocketGuildUser user, string emojiName)
        {
            var result = from r in user.Guild.Emotes
                         where r.Name == emojiName
                         select r.Id;
            ulong emojiID = result.FirstOrDefault();
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

        public static IRole FindRole(SocketGuildUser user, string roleName)
        {
            var result = from r in user.Guild.Roles
                         where r.Name == roleName
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            var roleIRole = user.Guild.GetRole(roleID);
            if (roleID == 0)
            {
                Console.WriteLine($"Could not find {roleName} role");
                return roleIRole;
            }

            return roleIRole;
        }
        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> sequence, T item)
        {
            return sequence.Concat(new[] { item });
        }

        public static bool OnOffExecution(IMessage msg)
        {
            if (OnOffUser.TurnedOffUsers == null)
            {
                return false;
            }
            var list = OnOffUser.TurnedOffUsers;
            var selectedList = list.Select(x => x.Id == msg.Author.Id).ToList();
            return list.Where(x => x.Id == msg.Author.Id).Any();         
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
