using System;

namespace NightlyCode.Twitch.V5 {

    /// <summary>
    /// access channel api
    /// </summary>
    /// <remarks>
    /// this only contains methods which are not yet part of official twitch api documentation
    /// and will be removed when twitch api contains such methodss
    /// </remarks>
    [Obsolete("Twitch V5 Api will be deactivated on 2018-12-31")]
    public class Channels : V5Api {

        /// <summary>
        /// creates a new access to <see cref="Channels"/> api
        /// </summary>
        /// <param name="clientid">id of client application</param>
        /// <param name="oauth">authentication token</param>
        public Channels(string clientid, string oauth)
            : base(clientid, oauth) { }

        /// <summary>
        /// Gets a list of users subscribed to a specified channel, sorted by the date when they subscribed.
        /// </summary>
        /// <param name="channelid">id of channel</param>
        /// <param name="limit">Maximum number of objects to return. Default: 25. Maximum: 100. </param>
        /// <param name="offset">Object offset for pagination of results. Default: 0.</param>
        /// <returns></returns>
        public SubscriberResponse GetChannelSubscribers(string channelid, int limit = 25, int offset = 0) {
            return Request<SubscriberResponse>($"https://api.twitch.tv/kraken/channels/{channelid}/subscriptions", new Parameter("limit", limit.ToString()), new Parameter("offset", offset.ToString()));
        }

        /// <summary>
        /// Gets a list of users who follow a specified channel, sorted by the date when they started following the channel (newest first, unless specified otherwise).
        /// </summary>
        /// <param name="channelid">id of channel</param>
        /// <param name="limit">Maximum number of objects to return. Default: 25. Maximum: 100.</param>
        /// <param name="offset">Object offset for pagination of results. Default: 0.</param>
        /// <returns></returns>
        public FollowResponse GetChannelFollowers(string channelid, int limit = 25, int offset = 0) {
            return Request<FollowResponse>($"https://api.twitch.tv/kraken/channels/{channelid}/follows", new Parameter("limit", limit.ToString()), new Parameter("offset", offset.ToString()));
        }

        /// <summary>
        /// Gets the channel object
        /// </summary>
        /// <returns>channel data</returns>
        public Channel GetChannel() {
            return Request<Channel>("https://api.twitch.tv/kraken/channel");
        }
    }
}