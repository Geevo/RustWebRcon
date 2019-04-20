using System;

namespace RustWebRcon.WebSockets.WebSocketEventArgs
{
    public class WebSocketMessageEventArgs : EventArgs
    {
        public WebSocketMessageEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }
}
