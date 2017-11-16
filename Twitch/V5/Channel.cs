using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {
    public class Channel {
        public bool Mature { get; set; }
        public string Status { get; set; }

        [JsonKey("broadcaster_language")]
        public string BroadcasterLanguage { get; set; }

        [JsonKey("display_name")]
        public string DisplayName { get; set; }

        public string Game { get; set; }

        public string Language { get; set; }

        [JsonKey("_id")]
        public string ID { get; set; }

        public string Name { get; set; }

        [JsonKey("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonKey("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public bool Partner { get; set; }

        public string Logo { get; set; }

        [JsonKey("video_banner")]
        public string VideoBanner { get; set; }

        [JsonKey("profile_banner")]
        public string ProfileBanner { get; set; }

        [JsonKey("profile_banner_background_color")]
        public string ProfileBannerBackgroundColor { get; set; }

        public string Url { get; set; }

        public int Views { get; set; }

        public int Followers { get; set; }

        [JsonKey("broadcaster_type")]
        public string BroadcasterType { get; set; }

        [JsonKey("stream_key")]
        public string StreamKey { get; set; }

        public string Email { get; set; }
    }
}