using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {
    public class FollowResponse {

        [JsonKey("_cursor")]
        public string Cursor { get; set; }

        [JsonKey("_total")]
        public int Total { get; set; }

        public Follow[] Follows { get; set; }
    }
}