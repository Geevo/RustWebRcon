namespace RustWebRcon.Entities.Events
{
    public class ChatEvent
    {
        public string Message { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Color { get; set; }
        public string Time { get; set; }
    }
}
