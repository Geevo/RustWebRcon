using RustWebRcon.Enums;

namespace RustWebRcon.Entities.Events
{
    public class PlayerConnectionEvent
    {
        public RustPlayer Player { get; set; }
        public PlayerConnectionType ConnectionType { get; set; }
    }
}
