using RustWebRcon.Entities.Events;
using RustWebRcon.Enums;
using System;
using System.Text.RegularExpressions;

namespace RustWebRcon.FeedEvents
{
    internal class NpcWorldFeedParser : IRustFeedParser
    {
        public string Pattern { get; set; } = @"^\[event] assets/.*/.*/(?<name>.*)/(?<prefab>.*)";
        public string Message { get; set; }
        public Action Callback { get; set; }
        public GroupCollection Groups { get; set; }

        public object GetFeed()
        {
            var name = Groups["name"].Value;
            NpcMapEventType type;

            switch (name)
            {
                case "patrol helicopter":
                    type = NpcMapEventType.Heli;
                    break;
                case "cargo plane":
                    type = NpcMapEventType.CargoPlane;
                    break;
                case "ch47":
                    type = NpcMapEventType.Chinook;
                    break;
                case "cargoship":
                    type = NpcMapEventType.CargoShip;
                    break;
                default:
                    type = NpcMapEventType.Unknown;
                    break;
            }

            return new NpcMapEvent()
            {
                Name = name,
                NpcType = type,
                Time = DateTime.Now
            };
        }
    }
}
