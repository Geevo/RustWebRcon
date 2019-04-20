using System;

namespace RustWebRcon.WebSockets.WebSocketEventArgs
{
    public class WebSocketErrorEventArgs : EventArgs
    {
        public WebSocketErrorEventArgs(Exception exception, string message)
        {
            Exception = exception;
            Message = message;
        }

        public Exception Exception { get; }

        public string Message { get; }
    }
}
