namespace RustWebRcon.Entities.Server
{
    public class RustPlayerInfo
    {
        public string SteamID { get; set; }
        public string OwnerSteamID { get; set; }
        public string DisplayName { get; set; }
        public int Ping { get; set; }
        public string Address { get; set; }
        public int ConnectedSeconds { get; set; }
        public float VoiationLevel { get; set; }
        public float CurrentLevel { get; set; }
        public float UnspentXp { get; set; }
        public float Health { get; set; }
    }
}
