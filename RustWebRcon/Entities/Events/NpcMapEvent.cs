using RustWebRcon.Enums;
using System;

namespace RustWebRcon.Entities.Events
{
    public class NpcMapEvent
    {
        public string Name { get; set; }
        public NpcMapEventType NpcType { get; set; }
        public DateTime Time { get; set; }
    }
}
