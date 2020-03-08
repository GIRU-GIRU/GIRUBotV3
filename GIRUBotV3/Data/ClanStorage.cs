using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GIRUBotV3.Data
{
    public class ClanStorage : DbContext
    {
        public DbSet<Clan> Clan { get; set; }
        public DbSet<ClanUser> ClanUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MeleeSlasherClans.db");
        }
    }

    public class Clan
    {
        [Key]
        public int ClanID { get; set; }
        public string ClanName { get; set; }
        public string LeaderName { get; set; }
        public ulong LeaderID { get; set; }

        public DateTime DateCreated { get; set; }
    }

    public class ClanUser
    {
        [Key]
        public ulong GeneratedID { get; set; }
        public ulong UserID { get; set; }
        public string UserName { get; set; }
        public int ClanID { get; set; }

        public DateTime DateRecruited { get; set; }
    }

    public class ClanStorageMethods
    {
        public async Task<bool> CreateNewClan(Clan clan)
        {
            try
            {
                bool wasSuccess = false;
                using (var db = new ClanStorage())
                {
                    if (await db.Clan.AnyAsync(x => x.ClanName.ToLower() == clan.ClanName.ToLower()))
                    {
                        wasSuccess = false;
                    }
                    else
                    {
                        await db.Clan.AddAsync(clan);
                        await db.SaveChangesAsync();
                        wasSuccess = true;
                    }

                    return wasSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AssignNewClanleader(string clanName, string leaderName, ulong leaderID)
        {
            try
            {
                bool wasSuccess = false;
                using (var db = new ClanStorage())
                {
                    var clan = await db.Clan.FirstOrDefaultAsync(x => x.ClanName.ToLower() == clanName.ToLower());

                    if (clan != null & leaderID > 0 & !string.IsNullOrEmpty(leaderName))
                    {
                        clan.LeaderID = leaderID;
                        clan.LeaderName = leaderName;
                        await db.SaveChangesAsync();
                        wasSuccess = true;

                    }
                    else
                    {
                        wasSuccess = false;
                    }

                    return wasSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> AssignNewClanMember(ulong clanLeaderID, string userName, ulong userID)
        {
            try
            {

                bool wasSuccess = false;
                using (var db = new ClanStorage())
                {

                    var targetClan = await db.Clan.FirstOrDefaultAsync(x => x.LeaderID == clanLeaderID);

                    if (targetClan.ClanID > 0)
                    {
                        ClanUser clanUser = new ClanUser
                        {
                            ClanID = targetClan.ClanID,
                            DateRecruited = DateTime.UtcNow,
                            UserID = userID
                        };

                        await db.ClanUser.AddAsync(clanUser);
                        await db.SaveChangesAsync();

                        wasSuccess = true;
                    }

                    return wasSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> KickClanMember(ulong clanLeaderID, ulong userID)
        {
            try
            {

                bool wasSuccess = false;
                using (var db = new ClanStorage())
                {

                    var targetClan = await db.Clan.FirstOrDefaultAsync(x => x.LeaderID == clanLeaderID);

                    if (targetClan.ClanID > 0)
                    {
                        var targetUser = await db.ClanUser.FirstOrDefaultAsync(x => x.UserID == userID);

                        if (targetUser != null)
                        {
                            db.ClanUser.Remove(targetUser);
                            await db.SaveChangesAsync();
                            wasSuccess = true;
                        }
                    }

                    return wasSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetClanName(ulong clanLeaderID)
        {
            try
            {
                string clanName = string.Empty;
                using (var db = new ClanStorage())
                {

                    var clan = await db.Clan.FirstOrDefaultAsync(x => x.LeaderID == clanLeaderID);

                    if (clan != null)
                    {
                        clanName = clan.ClanName;
                    }

                }

                return clanName;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> CheckIfExistingClanmember(ulong userID)
        {
            try
            {

                bool wasSuccess = false;
                using (var db = new ClanStorage())
                {

                    wasSuccess = await db.ClanUser.AnyAsync(x => x.UserID == userID);

                    return wasSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CheckIfExistingClanLeader(ulong clanleaderID)
        {
            try
            {
                bool userAlreadyClanLeader = false;
                using (var db = new ClanStorage())
                {
                    userAlreadyClanLeader = await db.Clan.AnyAsync(x => x.LeaderID == clanleaderID);
                }

                return userAlreadyClanLeader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteClan(string clanName)
        {
            try
            {
                bool wasSuccess = false;
                using (var db = new ClanStorage())
                {
                    var clan = await db.Clan.FirstOrDefaultAsync(x => x.ClanName.ToLower() == clanName);

                    if (clan != null)
                    {
                        db.Clan.Remove(clan);
                        await db.SaveChangesAsync();
                        wasSuccess = true;
                    }

                    return wasSuccess;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CheckIfSameClan(ulong clanleaderID, ulong playerID)
        {
            try
            {
                bool clanIsSame = false;

                using (var db = new ClanStorage())
                {
                    var clanLeaderClan = await db.Clan.FirstOrDefaultAsync(x => x.LeaderID == clanleaderID);

                    var playerClan = await db.ClanUser.FirstOrDefaultAsync(x => x.UserID == playerID);

                    if (clanLeaderClan != null && playerClan != null)
                    {
                        if (playerClan.ClanID == clanLeaderClan.ClanID)
                        {
                            clanIsSame = true;
                        }
                    }

                }

                return clanIsSame;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}




