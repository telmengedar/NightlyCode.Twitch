namespace NightlyCode.Twitch.Chat {
    public class HostInformation {

        public HostInformation(string host, string channel, int viewers) {
            Host = host;
            Channel = channel;
            Viewers = viewers;
        }

        public string Host { get; private set; }
        public string Channel { get; private set; }
        public int Viewers { get; private set; } 
    }
}