namespace NightlyCode.Twitch.Chat {
    public class UserState {
        public string Color { get; set; }
        public string DisplayName { get; set; }
        public int[] EmoteSets { get; set; }
        public bool IsMod { get; set; }
        public bool IsSubscriber { get; set; }
        public bool IsTurbo { get; set; }
        public UserType UserType { get; set; }
    }
}