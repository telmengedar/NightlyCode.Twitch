namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// emote in chat message
    /// </summary>
    public class Emote {

        public Emote(int id, int firstIndex, int lastIndex) {
            ID = id;
            FirstIndex = firstIndex;
            LastIndex = lastIndex;
        }

        /// <summary>
        /// emote id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// start index in message
        /// </summary>
        public int FirstIndex { get; set; }

        /// <summary>
        /// end index in message
        /// </summary>
        public int LastIndex { get; set; }

        /// <summary>
        /// get url to the emote pic
        /// </summary>
        /// <param name="size">size of emote pic to use</param>
        /// <returns>url which can be used to download the emote (or link to it)</returns>
        public string GetUrl(int size) {
            return $"http://static-cdn.jtvnw.net/emoticons/v1/{ID}/{size}.0";
        } 
    }
}