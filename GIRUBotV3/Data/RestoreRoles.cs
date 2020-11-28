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
    public class Restoreroles : DbContext
    {
        public DbSet<RestoreRoleModel> RestoreRoles { get; set; }
        public DbSet<RestoreRoleModelRoles> RoleModelRolesStore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MeleeSlasher.db");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }

    public class RestoreRoleModel
    {
        [Key]
        public ulong UserID { get; set; }
        public string Username { get; set; }
        public ulong AdminID { get; set; }
        public string DateInserted { get; set; }
        public string Time { get; set; }
    }
    public class RestoreRoleModelRoles
    {
        [Key]
        public int ID { get; set; }
        public ulong UserID { get; set; }
        public string RoleName { get; set; }
        public ulong RoleID { get; set; }

    }

    public class StoreRoleMethods
    {
        public async Task StoreUserRoles(SocketCommandContext context, SocketGuildUser target)
        {
            try
            {
                using (var db = new Restoreroles())
                {
                    RestoreRoleModelRemoveExisting(context, target, db);
                    RestoreRolesRemoveExisting(context, target, db);
                    foreach (var role in target.Roles)
                    {
                        if (RoleToStoreIsNotValid(role, context))
                        {
                            continue;
                        }

                        await db.RoleModelRolesStore.AddAsync(new RestoreRoleModelRoles
                        {
                            UserID = target.Id,
                            RoleName = role.Name,
                            RoleID = role.Id
                        });
                    }

                    await db.RestoreRoles.AddAsync(new RestoreRoleModel
                    {
                        Username = target.Username,
                        DateInserted = DateTime.Now.ToShortDateString(),
                        Time = DateTime.Now.ToShortTimeString(),
                        UserID = target.Id,
                        AdminID = context.Message.Author.Id
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await context.Channel.SendMessageAsync($"store roles table is fucked atm sry kid: {ex.Message}");
            }
        }
        private void RestoreRoleModelRemoveExisting(SocketCommandContext context, SocketGuildUser target, Restoreroles db)
        {
            var existingUser = db.RoleModelRolesStore.AsQueryable().Where(x => x.UserID == target.Id);
            if (existingUser.Any())
            {
                foreach (var item in existingUser)
                {
                    if (RoleToStoreIsNotValid(context.Guild.GetRole(item.RoleID), context))
                    {
                        continue;
                    }
                    db.RoleModelRolesStore.Remove(item);
                }
            }
        }
        private bool RoleToStoreIsNotValid(SocketRole role, SocketCommandContext context)
        {
            if (role.Id == context.Guild.EveryoneRole.Id)
            {
                return true;
            }
            return false;
        }




        public async Task RestoreUserRoles(SocketCommandContext context, SocketGuildUser target)
        {
            try
            {
                using (var db = new Restoreroles())
                {
                    

                    var IRoleCollection = new List<IRole>();
                    foreach (var item in db.RoleModelRolesStore.AsQueryable().Where(x => x.UserID == target.Id))
                    {
                        IRoleCollection.Add(context.Guild.GetRole(item.RoleID));
                    }
                    await target.AddRolesAsync(IRoleCollection);
                }
            }
            catch (Exception ex)
            {
                await context.Channel.SendMessageAsync($"restore user roles table is fucked atm sry kid {ex.Message}");
            }
        }
        private void RestoreRolesRemoveExisting(SocketCommandContext context, SocketGuildUser target, Restoreroles db)
        {
            var existingUser = db.RestoreRoles.AsQueryable().Where(x => x.UserID == target.Id);
            if (existingUser.Any())
            {
                foreach (var item in existingUser)
                {
                    db.RestoreRoles.Remove(item);
                }
            }
        }



    }
}
