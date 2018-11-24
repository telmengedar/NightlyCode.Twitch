using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.Api {
    public class User {

        /// <summary>
        /// User's ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// User's login name.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's display name.
        /// </summary>
        [JsonKey("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// User’s type: "staff", "admin", "global_mod", or "".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// User’s broadcaster type: "partner", "affiliate", or ""
        /// </summary>
        [JsonKey("broadcaster_type")]
        public string BroadcasterType { get; set; }

        /// <summary>
        /// User's channel description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the user's profile image.
        /// </summary>
        [JsonKey("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        /// <summary>
        /// URL of the user's offline image.
        /// </summary>
        [JsonKey("offline_image_url")]
        public string OfflineImageUrl { get; set; }

        /// <summary>
        /// Total number of views of the user’s channel.
        /// </summary>
        [JsonKey("view_count")]
        public int ViewCount { get; set; }

        /// <summary>
        /// User’s email address. Returned if the request includes the user:read:edit scope.
        /// </summary>
        public string Email { get; set; }
    }
}