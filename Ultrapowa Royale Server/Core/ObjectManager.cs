using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using UCS.GameFiles;
using UCS.Logic;
using Timer = System.Threading.Timer;

namespace UCS.Core

{
    internal class ObjectManager : IDisposable
    {
        private static readonly object m_vDatabaseLock = new object();
        private static Dictionary<long, Alliance> m_vAlliances;
        private static long m_vAllianceSeed;
        private static long m_vAvatarSeed;
        private static DatabaseManager m_vDatabase;
        private static string m_vHomeDefault;
        private static Random m_vRandomSeed;
        private static string[] m_vBannedIPs;
        public bool m_vTimerCanceled;
        public Timer TimerReference;

        /// <summary>
        /// Loader of the ObjectManager class, which is neccessary for all UCS functionality.
        /// </summary>
        public ObjectManager()
        {
            m_vTimerCanceled = false;
            m_vDatabase = new DatabaseManager();
            NpcLevels = new Dictionary<int, string>();
            DataTables = new DataTables();
            m_vAlliances = new Dictionary<long, Alliance>();

            if (!File.Exists("banned-ip.txt"))
                File.Create("banned-ip.txt");

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
                LoadFingerPrint();

            using (var sr = new StreamReader(@"gamefiles/starting_home.json"))
                m_vHomeDefault = sr.ReadToEnd();

            m_vAvatarSeed = m_vDatabase.GetMaxPlayerId() + 1;
            m_vAllianceSeed = m_vDatabase.GetMaxAllianceId() + 1;
            GetAllAlliancesFromDB();
            LoadGameFiles();
            LoadBannedIPs();

            TimerCallback TimerDelegate = Save;
            var TimerItem = new Timer(TimerDelegate, null, 30000, 15000);
            TimerReference = TimerItem;

            Console.WriteLine("[UCR]    Database Sync started successfully");
            m_vRandomSeed = new Random();
        }

        public static DataTables DataTables { get; set; }
        public static FingerPrint FingerPrint { get; set; }
        public static Dictionary<int, string> NpcLevels { get; set; }

        /// <summary>
        /// This function store a new alliance in the database.
        /// </summary>
        /// <param name="seed">The seed of the client.</param>
        /// <returns>The alliance data.</returns>
        /// <seealso cref="Alliance"/>
        public static Alliance CreateAlliance(long seed)
        {
            Alliance alliance;
            lock (m_vDatabaseLock)
            {
                if (seed == 0)
                    seed = m_vAllianceSeed;
                alliance = new Alliance(seed);
                m_vAllianceSeed++;
            }
            m_vDatabase.CreateAlliance(alliance);
            m_vAlliances.Add(alliance.GetAllianceId(), alliance);
            return alliance;
        }

        /// <summary>
        /// This function store all banned ips in a variable.
        /// </summary>
        public static void LoadBannedIPs()
        {
            m_vBannedIPs = File.ReadAllLines("banned-ip.txt");
        }

        /// <summary>
        /// This function return all banned ip.
        /// </summary>
        /// <returns>An array containing all banned ip.</returns>
        public static string[] GetBannedIPs()
        {
            return m_vBannedIPs;
        }

        /// <summary>
        /// This function create a new player in the database.
        /// </summary>
        /// <param name="seed">The seed of the client.</param>
        /// <returns>The level() of the player.</returns>
        /// <seealso cref="Level"/>
        public static Level CreateAvatar(long seed)
        {
            Level pl;
            lock (m_vDatabaseLock)
            {
                if (seed == 0)
                    seed = m_vAvatarSeed;
                pl = new Level(seed);
                m_vAvatarSeed++;
            }
            pl.LoadFromJSON(m_vHomeDefault);
            m_vDatabase.CreateAccount(pl);
            return pl;
        }

        /// <summary>
        /// This function store all alliances in the database,
        /// in a list<> variable, named as m_vAlliances.
        /// </summary>
        public static void GetAllAlliancesFromDB()
        {
            foreach (var a in m_vDatabase.GetAllAlliances())
            {
                if (!m_vAlliances.ContainsKey(a.GetAllianceId()))
                    m_vAlliances.Add(a.GetAllianceId(), a);
            }
        }

        /// <summary>
        /// This function get the info of an alliance.
        /// </summary>
        /// <param name="allianceId">The (Int64) ID of the alliance.</param>
        /// <returns>Return data about the alliance.</returns>
        public static Alliance GetAlliance(long allianceId)
        {
            Alliance alliance = null;
            if (m_vAlliances.ContainsKey(allianceId))
            {
                alliance = m_vAlliances[allianceId];
            }
            else
            {
                alliance = m_vDatabase.GetAlliance(allianceId);
                if (alliance != null)
                    m_vAlliances.Add(alliance.GetAllianceId(), alliance);
            }
            return alliance;
        }

        /// <summary>
        /// This function return all in-memory alliances.
        /// </summary>
        /// <returns>All alliances in-memory</returns>
        public static List<Alliance> GetInMemoryAlliances()
        {
            var alliances = new List<Alliance>();
            alliances.AddRange(m_vAlliances.Values);
            return alliances;
        }

        /// <summary>
        /// This function return the data of a random player, in memory.
        /// </summary>
        /// <returns>Random player data.</returns>
        public static Level GetRandomOnlinePlayer()
        {
            var index = m_vRandomSeed.Next(0, ResourcesManager.GetInMemoryLevels().Count);//accès concurrent KO
            return ResourcesManager.GetInMemoryLevels().ElementAt(index);
        }

        /// <summary>
        ///  This function return the data of a random player in database.
        /// </summary>
        /// <returns>Data of a random player.</returns>
        public static Level GetRandomPlayerFromAll()
        {
            var index = m_vRandomSeed.Next(0, ResourcesManager.GetAllPlayerIds().Count);//accès concurrent KO
            return ResourcesManager.GetPlayer(ResourcesManager.GetAllPlayerIds()[index]);
        }

        /// <summary>
        /// This function store the content of the fingerprint file in a variable.
        /// </summary>
        public static void LoadFingerPrint()
        {
            FingerPrint = new FingerPrint(@"gamefiles/fingerprint.json");
        }

        /// <summary>
        /// This function load all gamefiles in the csv/logic/ folder.
        /// </summary>
        public static void LoadGameFiles()
        {
            var gameFiles = new List<Tuple<string, string, int>>();

            Console.WriteLine("[UCR]    Loading server gamefiles & data...");
            foreach (var data in gameFiles)
            {
                Console.Write("             ->  " + data.Item1);
                DataTables.InitDataTable(new CSVTable(data.Item2), data.Item3);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" done");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// This function save someuser (Need to implement).
        /// </summary>
        /// <param name="state"></param>
        private void Save(object state)
        {
            m_vDatabase.Save(ResourcesManager.GetInMemoryLevels());
            m_vDatabase.Save(m_vAlliances.Values.ToList());
            if (m_vTimerCanceled)
                TimerReference.Dispose();
        }

        /// <summary>
        /// This function dispose the class.
        /// </summary>
        public void Dispose()
        {
            if (TimerReference != null)
            {
                TimerReference.Dispose();
                TimerReference = null;
            }
        }
    }
}