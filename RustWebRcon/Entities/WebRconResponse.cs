namespace RustWebRcon.Entities
{
    public class WebRconResponse
    {
        public string Message { get; set; }
        public int Identifier { get; set; }
        public string Type { get; set; }
        public string Stacktrace { get; set; }
    }
}
