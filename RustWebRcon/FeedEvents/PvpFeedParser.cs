using RustWebRcon.Entities;
using RustWebRcon.Entities.Events;
using System;
using System.Text.RegularExpressions;

namespace RustWebRcon.FeedEvents
{
    internal class PvpFeedParser : IRustFeedParser
    {
        public string Pattern { get; set; } = @"(?<victimName>.*?)\[(?<victimRustId>\d+?)\/(?<victimSteamId>\d{16,})\] was killed by (?<killerName>.*?)\[(?<killerRustId>\d+?)\/(?<killerSteamId>\d{16,})\]";
        public string Message { get; set; }
        public Action Callback { get; set; }
        public GroupCollection Groups { get; set; }

        public object GetFeed()
        {
            var victim = new RustPlayer()
            {
                Name = Groups["victimName"].Value,
                RustId = Groups["victimRustId"].Value,
                SteamId = Groups["victimSteamId"].Value
            };

            var killer = new RustPlayer()
            {
                Name = Groups["killerName"].Value,
                RustId = Groups["killerRustId"].Value,
                SteamId = Groups["killerSteamId"].Value
            };

            return new PvpEvent() { Killer = killer, Victim = victim };
        }
    }
}
