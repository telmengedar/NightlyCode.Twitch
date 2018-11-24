namespace NightlyCode.Twitch.Chat {
    public class Notice {

        public Notice(string channel, string messageId, string message) {
            Channel = channel;
            MessageID = messageId;
            Message = message;
        }

        public string Channel { get; }
        public string MessageID { get; }
        public string Message { get; } 
    }
}