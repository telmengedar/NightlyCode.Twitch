using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {

    /// <summary>
    /// response to <see cref="Channels.GetChannelSubscribers"/> request
    /// </summary>
    public class SubscriberResponse {

        [JsonKey("_total")]
        public int Total { get; set; }

        public Subscription[] Subscriptions { get; set; }
    }
}