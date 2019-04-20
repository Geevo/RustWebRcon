namespace RustWebRcon.Entities.Events
{
    public class PveEvent
    {
        public string Entity { get; set; }
        public RustPlayer Victim { get; set; }
    }
}
