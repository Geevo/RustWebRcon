using RustWebRcon.Entities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RustWebRcon
{
    internal class WebRconMessageHandler
    {
        List<IRustFeedParser> rustEvents = new List<IRustFeedParser>();

        public void Listen<TRustFeedParser, TResult>(Action<TResult> result) where TRustFeedParser : IRustFeedParser, new()
        {
            TRustFeedParser rustEvent = new TRustFeedParser();
            rustEvent.Callback = () => result((TResult)rustEvent.GetFeed());
            rustEvents.Add(rustEvent);
        }

        public void OnMesssageReceived(WebRconResponse rconResponse)
        {
            foreach (IRustFeedParser rustEvent in rustEvents)
            {
                var match = Regex.Match(rconResponse.Message, rustEvent.Pattern);
                if (match.Success)
                {
                    rustEvent.Message = rconResponse.Message;
                    rustEvent.Groups = match.Groups;
                    rustEvent.Callback.Invoke();
                    break;
                }
            }
        }
    }
}
