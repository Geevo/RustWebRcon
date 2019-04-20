namespace RustWebRcon.Entities.Server
{
    public class RustServerVersion
    {
        public string Protocol { get; set; }
        public string BuildDate { get; set; }
        public string UnityVersion { get; set; }
        public string Changeset { get; set; }
        public string Branch { get; set; }
    }
}
