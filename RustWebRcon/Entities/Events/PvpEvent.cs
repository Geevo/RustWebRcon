namespace RustWebRcon.Entities.Events
{
    public class PvpEvent
    {
        public RustPlayer Killer { get; set; }
        public RustPlayer Victim { get; set; }
    }
}
