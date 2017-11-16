using System;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {
    public class Subscription {

        [JsonKey("_id")]
        public string ID { get; set; }

        [JsonKey("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonKey("sub_plan")]
        public string SubPlan { get; set; }

        [JsonKey("sub_plan_name")]
        public string SubPlanName { get; set; }

        public User User { get; set; }
    }
}