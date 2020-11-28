using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIRUBotV3.Data
{
    public class Memestorage : DbContext
    {
        public DbSet<MemeStoreModel> Memestore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
      
            optionsBuilder.UseSqlite($"Data Source={Global.Config.DBLocation}MeleeSlasher.db");
        }
    }

    public class MemeStoreModel
    {
        [Key]
        public int MemeId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public ulong AuthorID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public ulong MemeUses { get; set; }
    }
}



