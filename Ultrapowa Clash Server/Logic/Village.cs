namespace UCS.Logic
{
    internal class RespawnVars
    {
        public uint obstacleClearCounter { get; set; }
        public uint respawnSeed { get; set; }
        public uint secondsFromLastRespawn { get; set; }
        public uint time_in_gembox_period { get; set; }
        public uint time_to_gembox_drop { get; set; }
        public uint time_to_special_period { get; set; }
        public uint time_to_special_drop { get; set; }
    }

    internal class Cooldown { }

    internal class Village
    {
        public Village()
        {
        }

        public Village(long playerId)
        {
            android_client = true;
            active_layout = 0;
            layout_state = new uint[] { 0, 0, 0, 0, 0, 0 };

            respawnVars = new RespawnVars
            {
                secondsFromLastRespawn = 10,
                respawnSeed = 10,
                obstacleClearCounter = 10,
                time_to_gembox_drop = 10,
                time_in_gembox_period = 10,
                time_to_special_drop = 248205,
                time_to_special_period = 97079
            };

            cooldown = new Cooldown { };
            newShopBuildings = new uint[] { 1, 0, 1, 1, 1, 1, 1, 0, 2, 0, 0, 0, 0, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            newShopTraps = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            newShopDecos = new uint[] { 1, 4, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            last_league_rank = 0;
            last_alliance_level = 1;
            last_league_shuffle = 0;
            last_season_seen = -1;
            last_news_seen = 13;
            edit_mode_shown = false;
            war_tutorials_seen = 0;
            war_base = false;
            help_opened = false;
            bool_layout_edit_shown_erase = false;
        }

        public bool android_client { get; set; }
        public uint active_layout { get; set; }
        public uint last_alliance_level { get; set; }
        public bool help_opened { get; set; }
        public int last_season_seen { get; set; }
        public bool bool_layout_edit_shown_erase { get; set; }
        public bool edit_mode_shown { get; set; }
        public uint last_league_rank { get; set; }
        public uint last_league_shuffle { get; set; }
        public uint last_news_seen { get; set; }
        public uint[] newShopBuildings { get; set; }
        public uint[] newShopDecos { get; set; }
        public uint[] newShopTraps { get; set; }
        public RespawnVars respawnVars { get; set; }
        public Cooldown cooldown { get; set; }
        public uint[] layout_state { get; set; }
        public bool war_base { get; set; }
        public uint war_tutorials_seen { get; set; }
    }
}