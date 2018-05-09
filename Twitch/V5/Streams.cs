using System;

namespace NightlyCode.Twitch.V5 {

    /// <summary>
    /// access streams api
    /// </summary>
    [Obsolete("Twitch V5 Api will be deactivated on 2018-12-31")]
    public class Streams : V5Api {

        /// <summary>
        /// creates a new <see cref="Streams"/> api access
        /// </summary>
        /// <param name="clientid">id of client application</param>
        /// <param name="oauth">authentication token</param>
        public Streams(string clientid, string oauth)
            : base(clientid, oauth) {}

        public GetStreamResponse GetStreamByUser(string channelid, StreamType type = StreamType.All) {
            return Request<GetStreamResponse>($"https://api.twitch.tv/kraken/streams/{channelid}", new Parameter("stream_type", type.ToString()));
        }
    }
}