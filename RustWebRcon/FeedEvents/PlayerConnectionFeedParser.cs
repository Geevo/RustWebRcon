using RustWebRcon.Entities;
using RustWebRcon.Entities.Events;
using RustWebRcon.Enums;
using System;
using System.Text.RegularExpressions;

namespace RustWebRcon.FeedEvents
{
    internal class PlayerConnectionFeedParser : IRustFeedParser
    {
        public string Pattern { get; set; } = @"(?<ipAddress>[0-9.]+):(?<port>[0-9]+)\/(?<playerSteamId>\d+)\/(?<playerName>.*) ((?<type>disconnecting:) (?<reason>.*)|(?<type>joined) \[(?<os>.*)\/(?<ownerSteamId>\d+)\])";
        public string Message { get; set; }
        public Action Callback { get; set; }
        public GroupCollection Groups { get; set; }

        public object GetFeed()
        {
            var player = new RustPlayer()
            {
                Name = Groups["playerName"].Value,
                RustId = Groups["playerRustId"].Value,
                SteamId = Groups["playerSteamId"].Value,
                OwnerSteamId = Groups["ownerSteamId"].Value
            };

            var type = (Groups["type"].Value.ToLower() == "joined")
                ? PlayerConnectionType.Connected
                : PlayerConnectionType.Disconnected;

            return new PlayerConnectionEvent() { Player = player, ConnectionType = type };
        }
    }
}
