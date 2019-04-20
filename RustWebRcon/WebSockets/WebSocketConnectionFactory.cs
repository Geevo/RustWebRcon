namespace RustWebRcon.WebSockets
{
    internal class WebSocketConnectionFactory
    {
        private readonly string ipAddress;
        private readonly string port;
        private readonly string password;

        public WebSocketConnectionFactory(string ipAddress, string port, string password)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.password = password;
        }

        public IWebSocketConnection Create()
        {
            return new WebSocketConnection(ipAddress, port, password);
        }
    }
}
