namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// message received in chat
    /// </summary>
    public class ChatMessage {

        /// <summary>
        /// user which sent the message
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// channel in which message was sent
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// message text
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// user badges
        /// </summary>
        public Badge[] Badges { get; set; }

        /// <summary>
        /// user color in hex (can be null)
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// displayname of user (can be null)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// emotes in message text
        /// </summary>
        public Emote[] Emotes { get; set; }

        /// <summary>
        /// message id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// room id
        /// </summary>
        public string RoomID { get; set; }

        /// <summary>
        /// whether user is mod
        /// </summary>
        public bool IsMod { get; set; }

        /// <summary>
        /// whether user is subscriber
        /// </summary>
        public bool IsSubscriber { get; set; }

        /// <summary>
        /// whether user has turbo badge
        /// </summary>
        public bool IsTurbo { get; set; }

        /// <summary>
        /// id of user
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// usertype
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// bits sent with message
        /// </summary>
        public int Bits { get; set; }
    }
}