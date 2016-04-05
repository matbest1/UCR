using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using UCS.Database;
using UCS.Logic;

namespace UCS.Core
{
    internal class DatabaseManager
    {
        private static DatabaseManager singelton;
        private readonly string m_vConnectionString;

        public DatabaseManager()
        {
            m_vConnectionString = ConfigurationManager.AppSettings["databaseConnectionName"];
        }

        public static DatabaseManager Singelton
        {
            get
            {
                if (singelton == null)
                    singelton = new DatabaseManager();
                return singelton;
            }
        }

        /// <summary>
        /// This function create a new player in the database, with default parameters.
        /// </summary>
        /// <param name="l">The level of the player.</param>
        public void CreateAccount(Level l)
        {
            try
            {
                Debugger.WriteLine("[UCR][UCRDB] Saving new account to database (player id: " + l.GetPlayerAvatar().GetId() + ")");
                using (var db = new ucrdbEntities(m_vConnectionString))
                {
                    db.player.Add(
                        new player
                        {
                            PlayerId = l.GetPlayerAvatar().GetId(),
                            AccountStatus = l.GetAccountStatus(),
                            AccountPrivileges = l.GetAccountPrivileges(),
                            LastUpdateTime = l.GetTime(),
                            IPAddress = l.GetIPAddress(),
                            Avatar = l.GetPlayerAvatar().SaveToJSON(),
                            GameObjects = l.SaveToJSON()
                        }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR][UCRDB] An exception occured during CreateAccount processing :", ex);
            }
        }

        /// <summary>
        /// This function create a new alliance in the database, with default parameters.
        /// </summary>
        /// <param name="a">The alliance data.</param>
        public void CreateAlliance(Alliance a)
        {
            try
            {
                using (var db = new ucrdbEntities(m_vConnectionString))
                {
                    db.clan.Add(
                        new clan
                        {
                            ClanId = a.GetAllianceId(),
                            LastUpdateTime = DateTime.Now,
                            Data = a.SaveToJSON()
                        }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR][UCRDB] An exception occured during CreateAlliance processing :", ex);
            }
        }

        /// <summary>
        /// This function get the player data.
        /// </summary>
        /// <param name="playerId">The (int64) ID of the player.</param>
        /// <returns>The level of the player.</returns>
        public Level GetAccount(long playerId)
        {
            Level account = null;
            try
            {
                using (var db = new ucrdbEntities(m_vConnectionString))
                {
                    var p = db.player.Find(playerId);

                    if (p != null)
                    {
                        account = new Level();
                        account.SetAccountStatus(p.AccountStatus);
                        account.SetAccountPrivileges(p.AccountPrivileges);
                        account.SetTime(p.LastUpdateTime);
                        account.SetIPAddress(p.IPAddress);
                        account.GetPlayerAvatar().LoadFromJSON(p.Avatar);
                        account.LoadFromJSON(p.GameObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR][UCSDB] An exception occured during GetAccount processing :", ex);
            }
            return account;
        }

        /// <summary>
        /// This function get the alliance data.
        /// </summary>
        /// <param name="allianceId">The (Int64) ID of the alliance.</param>
        /// <returns>The Alliance of the Clan.</returns>
        public Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            try
            {
                using (var db = new ucrdbEntities(m_vConnectionString))
                {
                    var p = db.clan.Find(allianceId);
                    if (p != null)
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJSON(p.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR][UCSDB] An exception occured during GetAlliance processing :", ex);
            }
            return alliance;
        }

        /// <summary>
        /// This function return all alliances in database, in a list<>.
        /// </summary>
        /// <returns>Return a list<> containing all alliances.</returns>
        public List<Alliance> GetAllAlliances()
        {
            List<Alliance> alliances = new List<Alliance>();
            try
            {
                using (var db = new Database.ucrdbEntities(m_vConnectionString))
                {
                    var a = db.clan;

                    foreach (clan c in a)
                    {
                        Alliance alliance = new Alliance();
                        alliance.LoadFromJSON(c.Data);
                        alliances.Add(alliance);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("An exception occured during GetAlliance processing:", ex);
            }
            return alliances;
        }

        /// <summary>
        /// This function return the highest alliance id stored in database.
        /// </summary>
        /// <returns>An int64 (ID) .</returns>
        public long GetMaxAllianceId()
        {
            long max = 0;
            using (var db = new ucrdbEntities(m_vConnectionString))
                max = (from alliance in db.clan select (long?)alliance.ClanId ?? 0).DefaultIfEmpty().Max();
            return max;
        }

        /// <summary>
        /// This function get all players id in a list.
        /// </summary>
        /// <returns>A list of all players id.</returns>
        public List<long> GetAllPlayerIds()
        {
            List<long> ids = new List<long>();
            using (var db = new Database.ucrdbEntities(m_vConnectionString))
            {
                foreach (player p in db.player)
                {
                    ids.Add(p.PlayerId);
                }
            }
            return ids;
        }

        /// <summary>
        /// The function return the highest player id stored in database.
        /// </summary>
        /// <returns>An int64 long ID.</returns>
        public long GetMaxPlayerId()
        {
            long max = 0;
            using (var db = new ucrdbEntities(m_vConnectionString))
                max = (from ep in db.player select (long?)ep.PlayerId ?? 0).DefaultIfEmpty().Max();
            return max;
        }

        /// <summary>
        /// This function remove an alliance from database.
        /// </summary>
        /// <param name="alliance">The Alliance of the alliance.</param>
        public void RemoveAlliance(Alliance alliance)
        {
            using (var db = new ucrdbEntities(m_vConnectionString))
            {
                db.clan.Remove(db.clan.Find((int)alliance.GetAllianceId()));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This function save a specific player in the database.
        /// </summary>
        /// <param name="avatar">The level of the player.</param>
        public void Save(Level avatar)
        {
            Debugger.WriteLine("Starting saving player " + avatar.GetPlayerAvatar().GetAvatarName() + " from memory to database at " + DateTime.Now, null, 4);
            var context = new ucrdbEntities(m_vConnectionString);
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            var p = context.player.Find(avatar.GetPlayerAvatar().GetId());
            if (p != null)
            {
                p.LastUpdateTime = avatar.GetTime();
                p.AccountStatus = avatar.GetAccountStatus();
                p.AccountPrivileges = avatar.GetAccountPrivileges();
                p.IPAddress = avatar.GetIPAddress();
                p.Avatar = avatar.GetPlayerAvatar().SaveToJSON();
                p.GameObjects = avatar.SaveToJSON();
                context.Entry(p).State = EntityState.Modified;
            }
            else
            {
                context.player.Add(
                    new player
                    {
                        PlayerId = avatar.GetPlayerAvatar().GetId(),
                        AccountStatus = avatar.GetAccountStatus(),
                        AccountPrivileges = avatar.GetAccountPrivileges(),
                        LastUpdateTime = avatar.GetTime(),
                        IPAddress = avatar.GetIPAddress(),
                        Avatar = avatar.GetPlayerAvatar().SaveToJSON(),
                        GameObjects = avatar.SaveToJSON()
                    }
                );
            }
            context.SaveChanges();
            Debugger.WriteLine("Finished saving player " + avatar.GetPlayerAvatar().GetAvatarName() + " from memory to database at " + DateTime.Now, null, 4);
        }

        public void Save(List<Level> avatars)
        {
            try
            {
                using (var context = new ucrdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    var transactionCount = 0;
                    foreach (var pl in avatars)
                        lock (pl)
                        {
                            var p = context.player.Find(pl.GetPlayerAvatar().GetId());
                            if (p != null)
                            {
                                p.LastUpdateTime = pl.GetTime();
                                p.AccountStatus = pl.GetAccountStatus();
                                p.AccountPrivileges = pl.GetAccountPrivileges();
                                p.IPAddress = pl.GetIPAddress();
                                p.Avatar = pl.GetPlayerAvatar().SaveToJSON();
                                p.GameObjects = pl.SaveToJSON();
                                context.Entry(p).State = EntityState.Modified;
                            }
                            else
                                context.player.Add(
                                    new player
                                    {
                                        PlayerId = pl.GetPlayerAvatar().GetId(),
                                        AccountStatus = pl.GetAccountStatus(),
                                        AccountPrivileges = pl.GetAccountPrivileges(),
                                        LastUpdateTime = pl.GetTime(),
                                        IPAddress = pl.GetIPAddress(),
                                        Avatar = pl.GetPlayerAvatar().SaveToJSON(),
                                        GameObjects = pl.SaveToJSON()
                                    }
                                );
                        }
                    transactionCount++;
                    if (transactionCount >= 500)
                    {
                        context.SaveChanges();
                        transactionCount = 0;
                    }
                    context.SaveChanges();
                }
                Debugger.WriteLine("[UCR][UCSDB] All players in memory has been saved to database at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR][UCSDB] An exception occured during Save processing for avatars :", ex);
            }
        }

        /// <summary>
        /// This function save a specific alliance in the database.
        /// </summary>
        /// <param name="alliances">The Alliance of the alliance.</param>
        public void Save(List<Alliance> alliances)
        {
            try
            {
                using (var context = new ucrdbEntities(m_vConnectionString))
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;
                    var transactionCount = 0;
                    foreach (var alliance in alliances)
                        lock (alliance)
                        {
                            var c = context.clan.Find((int)alliance.GetAllianceId());
                            if (c != null)
                            {
                                c.LastUpdateTime = DateTime.Now;
                                c.Data = alliance.SaveToJSON();
                                context.Entry(c).State = EntityState.Modified;
                            }
                            else
                            {
                                context.clan.Add(
                                    new clan
                                    {
                                        ClanId = alliance.GetAllianceId(),
                                        LastUpdateTime = DateTime.Now,
                                        Data = alliance.SaveToJSON()
                                    }
                                    );
                            }
                        }
                    transactionCount++;
                    if (transactionCount >= 500)
                    {
                        context.SaveChanges();
                        transactionCount = 0;
                    }
                    context.SaveChanges();
                }
                Debugger.WriteLine("[UCR][UCSDB] All alliances in memory has been saved to database at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR][UCSDB] An exception occured during Save processing for alliances :", ex);
            }
        }
    }
}