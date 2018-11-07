using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using GIRUBotV3.Modules;
using Microsoft.EntityFrameworkCore;
using Discord.Commands;

namespace GIRUBotV3.Data
{
    public class InviteRegulationStorage : DbContext
    {
        public DbSet<InviteRegulationStore> RestoreRoles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MeleeSlasher.db");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }

    public class InviteRegulationStore
    {
        [Key]
        public int InviteInstance { get; set; }
        public ulong UserID { get; set; }
        public string Username { get; set; }
        public string DateInserted { get; set; }
        public string Time { get; set; }
    }


    public class InviteRegulationStoreMethods
    {
        public async Task StoreRequestedInvites(SocketCommandContext context)
        {
            using (var db = new InviteRegulationStorage())
            {  
                await db.AddAsync(new InviteRegulationStore
                {
                    UserID = context.Message.Author.Id,
                    Username = context.Message.Author.Username,
                    DateInserted = DateTime.Now.ToShortDateString(),
                    Time = DateTime.Now.ToShortTimeString(),
                }
                     );
                await db.SaveChangesAsync();
            }
        }

    }
}