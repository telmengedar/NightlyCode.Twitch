namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// sent when a user subscribes to a channel
    /// </summary>
    public class Subscription {
        public string Channel { get; set; }
        public string DisplayName { get; set; }
        public string Color { get; set; }
        public string Message { get; set; }
        public Badge[] Badges { get; set; }
        public Emote[] Emotes { get; set; }
        public bool IsMod { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int Months { get; set; }
        public string Plan { get; set; }
        public string PlanName { get; set; }
        public string RoomID { get; set; }
        public bool IsSubscriber { get; set; }
        public string SystemMessage { get; set; }
        public bool IsTurbo { get; set; }
        public string User { get; set; }
        public string UserID { get; set; }
        public UserType UserType { get; set; }

    }
}