using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {
    public class Stream {

        [JsonKey("_id")]
        public string ID { get; set; }

        public string Game { get; set; }

        public int Viewers { get; set; }

        [JsonKey("video_height")]
        public int VideoHeight { get; set; }

        [JsonKey("average_fps")]
        public int AverageFPS { get; set; }

        public int Delay { get; set; }

        [JsonKey("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonKey("is_playlist")]
        public bool IsPlaylist { get; set; }

        public StreamPreview Preview { get; set; }
    }
}