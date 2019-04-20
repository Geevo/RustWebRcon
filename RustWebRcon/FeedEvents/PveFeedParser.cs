using RustWebRcon.Entities;
using RustWebRcon.Entities.Events;
using System;
using System.Text.RegularExpressions;

namespace RustWebRcon.FeedEvents
{
    internal class PveFeedParser : IRustFeedParser
    {
        public string Pattern { get; set; } = @"(?<victimName>.*?)\[(?<victimRustId>\d+?)\/(?<victimSteamId>\d{16,})\] was (suicide|killed) by (?<entityName>(?!\[\d+/\d+])(?!((.*)(\[(\d{5,})\/(\d{5,})\])))(.*))";
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

            var entityName = Groups["entityName"].Value;

            return new PveEvent() { Entity = entityName, Victim = victim };
        }
    }
}
