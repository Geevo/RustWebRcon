using RustWebRcon.WebSockets.WebSocketEventArgs;
using System;

namespace RustWebRcon.WebSockets
{
    public interface IWebSocketConnection
    {
        event EventHandler<WebSocketMessageEventArgs> MesssageReceived;
        event EventHandler<WebSocketOpenEventArgs> SocketOpened;
        event EventHandler<WebSocketCloseEventArgs> SocketClosed;
        event EventHandler<WebSocketErrorEventArgs> SocketError;

        bool IsOpen { get; }

        void Connect();
        void Disconnect();
        void Send(string data);
    }
}
