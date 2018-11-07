using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using GIRUBotV3.Modules;

namespace GIRUBotV3.Data
{
    public class AdministrationStore : DbContext
    {
        public DbSet<TimerStore> TimerStore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MeleeSlasher.db");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }

    public class TimerStore
    {
        [Key]
        public int TimerID { get; set; }
        public string Username { get; set; }
        public ulong UserID { get; set; }
        public DateTime TimeInserted { get; set; }
        public DateTime TimeRemoval { get; set; }
        public Boolean Executed { get; set; }
        public ulong AdminID { get; set; }
        public Enum CaseEnum { get; set; }
        public string CaseType { get; set; }
    }


    public class StoreAdministrationTimerMethods
    {
        public async Task StoreAdminTimers(SocketCommandContext context, SocketGuildUser target, double duration, char durationType, AdministrationStorageCaseTypes caseType)
        {
            try
            {
                DateTime processedDuration = await ConvertDateInput(duration, durationType, context);
                using (var db = new AdministrationStore())
                {
                    await db.TimerStore.AddAsync(new TimerStore
                    {
                        Username = target.Username,
                        UserID = target.Id,
                        TimeInserted = DateTime.Now,
                        TimeRemoval = processedDuration,
                        AdminID = context.Message.Author.Id,
                        CaseEnum = caseType,
                        CaseType = caseType.ToString()

                    });
                    await db.SaveChangesAsync();

                    //test
                    db.TimerStore.Where(x => x.TimeRemoval > DateTime.Now)
                                                .OrderBy(x => x.TimeRemoval)
                                                   .First();
                }
            }
            catch (Exception)
            {
                await context.Channel.SendMessageAsync("something broke in admin storage");
            }
        }

        private async Task<DateTime> ConvertDateInput(double duration, char durationType, SocketCommandContext context)
        {
            var time = new DateTime();
            try
            {
                
                switch (durationType)
                {
                    case 's':
                        time.AddSeconds(duration);
                        break;
                    case 'd':
                        time.AddDays(duration);
                        break;
                    case 'm':
                        time.AddMonths((int)duration);
                        break;
                    default:
                        time.AddSeconds(duration);
                        break;
                }
               return time;
            }
            catch (Exception)
            {
                await context.Channel.SendMessageAsync("something broke in admin storage time conversion");
                return time;           
            }
            
        }

        public enum AdministrationStorageCaseTypes
        {
            Ban = 0,
            Mute = 1,
            OnOff = 2,
            PostPicPerms = 3,
        }
    }
}


    







//    catch (Exception ex)
//    {
//        await context.Channel.SendMessageAsync($"store roles table is fucked atm sry kid: {ex.Message}");
//    }
//}
//private void RestoreRoleModelRemoveExisting(SocketCommandContext context, SocketGuildUser target, Restoreroles db)
//{
//    var existingUser = db.RoleModelRolesStore.Where(x => x.UserID == target.Id);
//    if (existingUser.Any())
//    {
//        foreach (var item in existingUser)
//        {
//            if (RoleToStoreIsNotValid(context.Guild.GetRole(item.RoleID), context))
//            {
//                continue;
//            }
//            db.RoleModelRolesStore.Remove(item);
//        }
//    }
//}
//private bool RoleToStoreIsNotValid(SocketRole role, SocketCommandContext context)
//{
//    if (role.Id == context.Guild.EveryoneRole.Id)
//    {
//        return true;
//    }
//    return false;
//}




//public async Task RestoreUserRoles(SocketCommandContext context, SocketGuildUser target)
//{
//    try
//    {
//        using (var db = new Restoreroles())
//        {


//            var IRoleCollection = new List<IRole>();
//            foreach (var item in db.RoleModelRolesStore.Where(x => x.UserID == target.Id))
//            {
//                IRoleCollection.Add(context.Guild.GetRole(item.RoleID));
//            }
//            await target.AddRolesAsync(IRoleCollection);
//        }
//    }
//    catch (Exception ex)
//    {
//        await context.Channel.SendMessageAsync($"restore user roles table is fucked atm sry kid {ex.Message}");
//    }
//}
//private void RestoreRolesRemoveExisting(SocketCommandContext context, SocketGuildUser target, Restoreroles db)
//{
//    var existingUser = db.RestoreRoles.Where(x => x.UserID == target.Id);
//    if (existingUser.Any())
//    {
//        foreach (var item in existingUser)
//        {
//            db.RestoreRoles.Remove(item);
//        }
//    }
//}




