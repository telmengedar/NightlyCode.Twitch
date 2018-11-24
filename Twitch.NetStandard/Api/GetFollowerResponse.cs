namespace NightlyCode.Twitch.Api {

    /// <summary>
    /// response to <see cref="TwitchApi.GetFollowers"/> or <see cref="TwitchApi.GetFollows"/>
    /// </summary>
    public class GetFollowerResponse {

        /// <summary>
        /// follower data
        /// </summary>
        public Follow[] Data { get; set; }

        /// <summary>
        /// pagination data
        /// </summary>
        public Pagination Pagination { get; set; } 
    }
}