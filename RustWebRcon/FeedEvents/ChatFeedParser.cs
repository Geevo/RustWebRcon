using Newtonsoft.Json;
using RustWebRcon.Entities.Events;
using System;
using System.Text.RegularExpressions;

namespace RustWebRcon.FeedEvents
{
    internal class ChatFeedParser : IRustFeedParser
    {
        public string Pattern { get; set; } = "({.*\"Message\".*\"UserId\".*\"Username\".*\"Color\".*\"Time\".*})";
        public string Message { get; set; }
        public Action Callback { get; set; }
        public GroupCollection Groups { get; set; }

        public object GetFeed()
        {
            return JsonConvert.DeserializeObject<ChatEvent>(Message);
        }
    }
}
