using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.Api {
    public class TwitchStream {

        /// <summary>
        /// Stream ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ID of the user who is streaming
        /// </summary>
        [JsonKey("user_id")]
        public string UserID { get; set; }

        /// <summary>
        /// ID of the game being played on the stream
        /// </summary>
        [JsonKey("game_id")]
        public string GameID { get; set; }

        /// <summary>
        /// Array of community IDs
        /// </summary>
        [JsonKey("community_ids")]
        public string[] CommunityIDs { get; set; }

        /// <summary>
        /// Stream type: "live" or "" (in case of error).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Stream title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Number of viewers watching the stream at the time of the query
        /// </summary>
        [JsonKey("viewer_count")]
        public int ViewerCount { get; set; }

        /// <summary>
        /// UTC timestamp
        /// </summary>
        [JsonKey("started_at")]
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// Stream language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Thumbnail URL of the stream. All image URLs have variable width and height. You can replace {width} and {height} with any values to get that size image
        /// </summary>
        public string ThumbNailUrl { get; set; }
    }
}