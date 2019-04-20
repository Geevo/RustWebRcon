using System;

namespace RustWebRcon.ConsoleApp.Sample
{
    class Program
    {
        private static WebRcon rcon;

        static void Main(string[] args)
        {
            // Intialise WebRcon
            rcon = new WebRcon("127.0.0.1", "28016", "password");

            // Subscribe to events
            rcon.ServerConnected += async (s, ev) =>
            {
                var serverinfo = await rcon.GetServerInfo();
                Console.WriteLine(serverinfo.Hostname);
            };

            rcon.RawMessage += (s, ev) =>
            {
                Console.WriteLine(ev);
            };

            rcon.ChatMessage += (s, ev) =>
            {
                Console.WriteLine($"{ev.Username}: {ev.Message}");
            };

            rcon.PlayerConnectionChange += (s, ev) =>
            {
                Console.WriteLine($"{ev.ConnectionType}: {ev.Player.Name}");
            };

            rcon.PvpKill += (s, ev) =>
            {
                Console.WriteLine($"{ev.Killer.Name} killed {ev.Victim.Name}");
            };

            rcon.PveDeath += (s, ev) =>
            {
                Console.WriteLine($"{ev.Victim.Name} was killed by {ev.Entity}");
            };

            rcon.NpcWorldEvent += (s, ev) =>
            {
                Console.WriteLine($"Event: {ev.Name}");
            };

            // Start the WebRcon
            rcon.Start();

            Console.ReadLine();
        }
    }
}
