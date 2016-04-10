namespace UCS.GameFiles
{
    internal class AllianceLevelsData : Data
    {
        public AllianceLevelsData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
        public int ExpPoints { get; set; }
        public bool IsVisible { get; set; }
        public int TroopRequestCooldown { get; set; }
        public int TroopDonationLimit { get; set; }
        public int TroopDonationRefund { get; set; }
        public int TroopDonationUpgrade { get; set; }
        public int WarLootCapacityPercent { get; set; }
        public int WarLootMultiplierPercent { get; set; }
        public int BadgeLevel { get; set; }
        public string BannerSWF { get; set; }
    }
}