using RustWebRcon.WebSockets.WebSocketEventArgs;
using System;
using WebSocketSharp;

namespace RustWebRcon.WebSockets
{
    internal class WebSocketConnection : IWebSocketConnection
    {
        public event EventHandler<WebSocketMessageEventArgs> MesssageReceived;
        public event EventHandler<WebSocketOpenEventArgs> SocketOpened;
        public event EventHandler<WebSocketCloseEventArgs> SocketClosed;
        public event EventHandler<WebSocketErrorEventArgs> SocketError;

        public bool IsOpen { get { return _webSocket.ReadyState == WebSocketState.Open; } }

        private WebSocket _webSocket;

        public WebSocketConnection(string ipAddress, string port, string password)
            : this($"ws://{ipAddress}:{port}/{password}")
        {

        }

        public WebSocketConnection(string connectionString)
        {
            _webSocket = new WebSocket(connectionString);
            _webSocket.OnMessage += WebSocket_OnMessage;
            _webSocket.OnOpen += WebSocket_OnOpen;
            _webSocket.OnClose += WebSocket_OnClose;
            _webSocket.OnError += WebSocket_OnError;
        }

        public void Connect()
        {
            _webSocket.ConnectAsync();
        }

        public void Disconnect()
        {
            _webSocket.CloseAsync();
        }

        public void Send(string data)
        {
            if (_webSocket.ReadyState == WebSocketState.Open)
            {
                _webSocket.SendAsync(data, null);
            }
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                MesssageReceived?.Invoke(sender,
                    new WebSocketMessageEventArgs(e.Data));
            }
        }

        private void WebSocket_OnOpen(object sender, EventArgs e)
        {
            SocketOpened?.Invoke(sender,
                new WebSocketOpenEventArgs());
        }

        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            SocketClosed?.Invoke(sender,
                new WebSocketCloseEventArgs(e.Code, e.Reason, e.WasClean));
        }

        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            SocketError?.Invoke(sender,
                new WebSocketErrorEventArgs(e.Exception, e.Message));
        }
    }
}
