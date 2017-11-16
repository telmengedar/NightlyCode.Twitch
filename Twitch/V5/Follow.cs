using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {
    public class Follow {

        [JsonKey("created_at")]
        public DateTime CreatedAt { get; set; }

        public bool Notifications { get; set; }

        public User User { get; set; }
    }
}