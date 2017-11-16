using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {
    public class User {

        [JsonKey("_id")]
        public string ID { get; set; }

        public string Bio { get; set; }

        [JsonKey("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonKey("display_name")]
        public string DisplayName { get; set; }

        public string Logo { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [JsonKey("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}