using System;
using System.Text.RegularExpressions;

namespace RustWebRcon
{
    public interface IRustFeedParser
    {
        string Pattern { get; set; }
        string Message { get; set; }
        Action Callback { get; set; }
        GroupCollection Groups { get; set; }

        object GetFeed();
    }
}
