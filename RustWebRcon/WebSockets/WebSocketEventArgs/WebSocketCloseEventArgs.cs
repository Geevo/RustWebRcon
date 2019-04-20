using System;

namespace RustWebRcon.WebSockets.WebSocketEventArgs
{
    public class WebSocketCloseEventArgs : EventArgs
    {
        public int Code { get; }

        public string Reason { get; }

        public bool WasClean { get; }

        public WebSocketCloseEventArgs(int code, string reason, bool wasClean)
        {
            Code = code;
            Reason = reason;
            WasClean = wasClean;
        }
    }
}
