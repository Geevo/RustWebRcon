using Newtonsoft.Json;
using RustWebRcon.Entities;
using RustWebRcon.Entities.Events;
using RustWebRcon.Entities.Server;
using RustWebRcon.Enums;
using RustWebRcon.FeedEvents;
using RustWebRcon.WebSockets;
using RustWebRcon.WebSockets.WebSocketEventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RustWebRcon
{
    public class WebRcon
    {
        private readonly IWebSocketConnection webSocketConnection;
        private WebRconMessageHandler webRconMessageHandler;

        public event EventHandler<PvpEvent> PvpKill;
        public event EventHandler<PveEvent> PveDeath;
        public event EventHandler<NpcMapEvent> NpcWorldEvent;
        public event EventHandler<ServerRestartEvent> ServerRestarting;
        public event EventHandler<PlayerConnectionEvent> PlayerConnectionChange;
        public event EventHandler<ChatEvent> ChatMessage;
        public event EventHandler<string> RawMessage;
        public event EventHandler ServerConnected;
        public event EventHandler<int> ServerDisconnected;


        public bool IsConnected { get => webSocketConnection.IsOpen; }

        public WebRcon(string ipAddress, string port, string password)
        {
            webSocketConnection = new WebSocketConnectionFactory(ipAddress, port, password).Create();
            webSocketConnection.SocketError += WebSocketConnection_SocketError;
            webSocketConnection.SocketClosed += WebSocketConnection_SocketClosed;
            webSocketConnection.SocketOpened += WebSocketConnection_SocketOpened;
        }

        public void Start()
        {
            if (!webSocketConnection.IsOpen)
            {
                webSocketConnection.Connect();
                ListenToMessages();
            }
        }

        public void Stop()
        {
            if (webSocketConnection.IsOpen)
            {
                webSocketConnection.Disconnect();
                webRconMessageHandler = null;
            }
        }

        public async void Reconnect()
        {
            if (!webSocketConnection.IsOpen)
            {
                await Task.Delay(5000);
                webSocketConnection.Connect();
            }
        }

        public void SendMessageAsync(string message)
        {
            SendCommandAsync($"say {message}", RconIdentifiers.Chat);
        }

        public void SendCommandAsync(string command, RconIdentifiers identifier = RconIdentifiers.Generic)
        {
            var request = JsonConvert.SerializeObject(
            new WebRconRequest()
            {
                Message = command,
                Identifier = Convert.ToInt32(identifier),
                Name = "WebRcon"
            });

            webSocketConnection.Send(request);
        }

        public async Task<WebRconResponse> SendAndReceiveCommandAsync(string command, RconIdentifiers identifier)
        {
            bool resultReturned = false;
            int timeOut = 0;
            WebRconResponse rconResponse = null;

            EventHandler<WebSocketMessageEventArgs> handler = (s, e) =>
            {
                if (!resultReturned)
                {
                    rconResponse = JsonConvert.DeserializeObject<WebRconResponse>(e.Data);
                    resultReturned = (RconIdentifiers)rconResponse.Identifier == identifier;
                }
            };

            webSocketConnection.MesssageReceived += handler;
            SendCommandAsync(command, identifier);

            while (!resultReturned)
            {
                if (timeOut > 10)
                {
                    rconResponse = new WebRconResponse() { Message = "Response timed out" };
                    break;
                }
                timeOut++;
                await Task.Delay(500);
            }

            webSocketConnection.MesssageReceived -= handler;

            return rconResponse;
        }

        public async Task<List<RustPlayerInfo>> GetPlayerList()
        {
            var playerList = new List<RustPlayerInfo>();
            var response = await SendAndReceiveCommandAsync("playerlist", RconIdentifiers.PlayerList);

            if ((RconIdentifiers)response.Identifier == RconIdentifiers.PlayerList)
            {
                playerList = JsonConvert.DeserializeObject<List<RustPlayerInfo>>(response.Message);
            }

            return playerList;
        }

        public async Task<RustServerInfo> GetServerInfo()
        {
            var serverInfo = new RustServerInfo();
            var response = await SendAndReceiveCommandAsync("serverinfo", RconIdentifiers.ServerInfo);

            if ((RconIdentifiers)response.Identifier == RconIdentifiers.ServerInfo)
            {
                serverInfo = JsonConvert.DeserializeObject<RustServerInfo>(response.Message);
            }

            return serverInfo;
        }

        public async Task<RustServerVersion> GetServerVersion()
        {
            // The version command doesn't return a json which is kinda annoying!
            var serverVersion = new RustServerVersion();
            var response = await SendAndReceiveCommandAsync("version", RconIdentifiers.ServerVersion);

            if ((RconIdentifiers)response.Identifier == RconIdentifiers.ServerVersion)
            {
                if (!string.IsNullOrEmpty(response.Message))
                {
                    using (var reader = new StringReader(response.Message))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var splitLine = line.Split(':');
                            switch (splitLine[0])
                            {
                                case "Protocol":
                                    serverVersion.Protocol = splitLine[1].Trim();
                                    break;
                                case "Build Date":
                                    serverVersion.BuildDate = splitLine[1].Trim();
                                    break;
                                case "Unity Version":
                                    serverVersion.UnityVersion = splitLine[1].Trim();
                                    break;
                                case "Changeset":
                                    serverVersion.Changeset = splitLine[1].Trim();
                                    break;
                                case "Branch":
                                    serverVersion.Branch = splitLine[1].Trim();
                                    break;
                            }
                        }
                    }
                }
            }

            return serverVersion;
        }

        private void ListenToMessages()
        {
            webRconMessageHandler = new WebRconMessageHandler();

            webSocketConnection.MesssageReceived += (s, e) =>
            {
                var response = JsonConvert.DeserializeObject<WebRconResponse>(e.Data);
                RawMessage?.Invoke(this, response.Message);

                webRconMessageHandler.OnMesssageReceived(response);
            };

            webRconMessageHandler.Listen<ChatFeedParser, ChatEvent>(chatMessage =>
            {
                ChatMessage?.Invoke(this, chatMessage);
            });

            webRconMessageHandler.Listen<PlayerConnectionFeedParser, PlayerConnectionEvent>(player =>
            {
                PlayerConnectionChange?.Invoke(this, player);
            });

            webRconMessageHandler.Listen<PvpFeedParser, PvpEvent>(pvpKill =>
            {
                PvpKill?.Invoke(this, pvpKill);
            });

            webRconMessageHandler.Listen<PveFeedParser, PveEvent>(pveDeath =>
            {
                PveDeath?.Invoke(this, pveDeath);
            });

            webRconMessageHandler.Listen<NpcWorldFeedParser, NpcMapEvent>(worldEvent =>
            {
                NpcWorldEvent?.Invoke(this, worldEvent);
            });

            webRconMessageHandler.Listen<RestartFeedParser, ServerRestartEvent>(restart =>
            {
                ServerRestarting?.Invoke(this, restart);
            });
        }

        private void WebSocketConnection_SocketOpened(object sender, WebSocketOpenEventArgs e)
        {
            ServerConnected?.Invoke(this, EventArgs.Empty);
        }

        private void WebSocketConnection_SocketClosed(object sender, WebSocketCloseEventArgs e)
        {
            ServerDisconnected?.Invoke(this, e.Code);
        }

        private void WebSocketConnection_SocketError(object sender, WebSocketErrorEventArgs e)
        {
            // TODO
        }
    }
}
