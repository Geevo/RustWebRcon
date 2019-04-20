using RustWebRcon.Entities.Events;
using System;
using System.Text.RegularExpressions;

namespace RustWebRcon.FeedEvents
{
    internal class RestartFeedParser : IRustFeedParser
    {
        public string Pattern { get; set; } = @"^Restarting in (?<time>\d+) seconds";
        public string Message { get; set; }
        public Action Callback { get; set; }
        public GroupCollection Groups { get; set; }

        public object GetFeed()
        {
            return new ServerRestartEvent() { TimeRemaining = Groups["time"].Value };
        }
    }
}
