using System.Text.RegularExpressions;
using NightlyCode.IRC;

namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// extensions for message
    /// </summary>
    public static class MessageExtensions {

        /// <summary>
        /// extracts username from messages source
        /// </summary>
        /// <param name="message">message to extract username from</param>
        /// <returns>username of user who sent message</returns>
        public static string ExtractUser(this IrcMessage message) {
            Match match = Regex.Match(message.Source, "^(?<user>[a-zA-Z0-9_]+)![a-zA-Z0-9_]+@[a-zA-Z0-9_]+\\.tmi\\.twitch\\.tv$");
            return match.Groups["user"].Value;
        }
    }
}