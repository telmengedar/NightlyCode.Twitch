namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// information about a host action
    /// </summary>
    public class HostInformation {

        /// <summary>
        /// creates a new <see cref="HostInformation"/>
        /// </summary>
        /// <param name="host">name of hoster</param>
        /// <param name="channel">hosted channel</param>
        /// <param name="viewers">number of viewers</param>
        public HostInformation(string host, string channel, int viewers) {
            Host = host;
            Channel = channel;
            Viewers = viewers;
        }

        /// <summary>
        /// name of hoster
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// hosted channel
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// number of viewers
        /// </summary>
        public int Viewers { get; } 
    }
}