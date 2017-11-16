namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// chat badge
    /// </summary>
    public class Badge {

        /// <summary>
        /// creates a new <see cref="Badge"/>
        /// </summary>
        /// <param name="name">name of badge</param>
        /// <param name="version">badge version</param>
        public Badge(string name, int version) {
            Name = name;
            Version = version;
        }

        /// <summary>
        /// name of badge
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// version of badge
        /// </summary>
        public int Version { get; set; } 
    }
}