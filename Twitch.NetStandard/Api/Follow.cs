using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.Api {
    public class Follow {

        [JsonKey("from_id")]
        public string FromID { get; set; }

        [JsonKey("to_id")]
        public string ToID { get; set; }

        [JsonKey("followed_at")]
        public DateTime FollowedAt { get; set; } 
    }
}