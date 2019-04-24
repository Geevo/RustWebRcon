# RustWebRcon
Simple Rust WebRcon c# library for capturing in-game events

The code was originally ripped out of much bigger project we had that had a website, repo and discord bot.
At the time of developing we couldn't find anything that worked for us.


Inspiration for the parsers comes from SourceRCON:  https://github.com/ScottKaye/CoreRCON

We make use of websocket-sharp: https://github.com/sta/websocket-sharp

## Examples
###  Listen for pvp and pve events
```cs
var rcon = new WebRcon("127.0.0.1", "28016", "password");

rcon.PvpKill += (s, ev) =>
{
    Console.WriteLine($"{ev.Killer.Name} killed {ev.Victim.Name}");
};

rcon.PveDeath += (s, ev) =>
{
    Console.WriteLine($"{ev.Victim.Name} was killed by {ev.Entity}");
};

rcon.Start();
```

### Welcome a player
```cs
rcon.PlayerConnectionChange += (s, ev) =>
{
    if (ev.ConnectionType == PlayerConnectionType.Connected)
    {
        SendMessageAsync($"Welcome {ev.Player.Name}");
    }
};
```
