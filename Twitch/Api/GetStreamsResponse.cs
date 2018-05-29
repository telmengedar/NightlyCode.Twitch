namespace NightlyCode.Twitch.Api {


    public class GetStreamsResponse {

        /// <summary>
        /// streams returned by request
        /// </summary>
        public TwitchStream[] Data { get; set; }

        /// <summary>
        /// pagination data
        /// </summary>
        public Pagination Pagination { get; set; }
    }
}