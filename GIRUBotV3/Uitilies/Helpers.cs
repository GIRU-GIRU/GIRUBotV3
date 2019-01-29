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

        public static bool IsModeratorOrOwner(SocketGuildUser user)
        {
            if (user.Id == user.Guild.OwnerId) return true;
            return user.Roles.Where(x => x.Name.ToLower() == Models.UtilityRoles.Moderator.ToLower()).Any();
        }

        public static bool IsOrganizerOrAbove(SocketGuildUser user)
        {
            if (user.Id == user.Guild.OwnerId) return true;
            if (user.Roles.Where(x => x.Name.ToLower() == Models.UtilityRoles.Moderator.ToLower()).Any()) return true;
            if (user.Roles.Where(x => x.Name.ToLower() == Models.UtilityRoles.Organizer.ToLower()).Any()) return true;
            return false;
        }

        public static IRole ReturnRole(SocketGuild guild, string role)
        {
            return guild.GetRole(guild.Roles.Where(x => x.Name.ToLower() == role.ToLower()).FirstOrDefault().Id);
        }

        public static string FindEmoji(SocketGuildUser user, string emojiName)
        {

            ulong emojiID = user.Guild.Emotes.Where(x => x.Name.ToLower() == emojiName.ToLower()).FirstOrDefault().Id;
            if (emojiID == 0)
            {
                Console.WriteLine($"Could not find {emojiName} emoji");
                return "🤔";
            }
            return $"<:{emojiName}:{emojiID}>";

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

        public static bool IsSonya(SocketGuildUser user)
        {
            return user.Roles.Where(x => x.Name == "Sonya").Any();
        }

        public static async Task<string> GetUsernameListFromIDs(IReadOnlyCollection<ulong> collection, IGuild guild)
        {
            List<string> MentionedUsers = new List<string>();
            foreach (var userID in collection)
            {
                var user = await guild.GetUserAsync(userID);

                MentionedUsers.Add(user.Username);
            }
            return String.Join(", ", MentionedUsers.ToArray());
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
