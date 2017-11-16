using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using NightlyCode.Japi.Json;
using NightlyCode.Twitch.V5;

namespace NightlyCode.Twitch.Api {

    /// <summary>
    /// 
    /// </summary>
    public class TwitchApi {
        readonly string clientid;
        readonly string oauth;

        /// <summary>
        /// creates a new <see cref="TwitchApi"/>
        /// </summary>
        /// <param name="clientid">client id of application</param>
        /// <param name="oauth">oauth token used for authentication</param>
        public TwitchApi(string clientid, string oauth) {
            this.clientid = clientid;
            this.oauth = oauth;
        }

        T Request<T>(string url, params Parameter[] parameters) {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Client-ID", clientid);
                wc.Headers.Add("Authorization", $"Bearer {oauth}");

                foreach (Parameter parameter in parameters)
                    wc.QueryString.Add(parameter.Key, parameter.Value);

                string response = wc.DownloadString(url);
                return JSON.Read<T>(response);
            }
        }

        public GetUserResponse GetUsersByID(params string[] ids) {
            return Request<GetUserResponse>("https://api.twitch.tv/helix/users", new Parameter("id", string.Join(",", ids)));
        }

        public GetUserResponse GetUsersByLogin(params string[] names) {
            return Request<GetUserResponse>("https://api.twitch.tv/helix/users", new Parameter("login", string.Join(",", names)));
        }

        public GetFollowerResponse GetFollowers(string userid, int results=20, string pagination=null) {
            if(string.IsNullOrEmpty(pagination))
                return Request<GetFollowerResponse>("https://api.twitch.tv/helix/users/follows", new Parameter("to_id", userid), new Parameter("first", results.ToString()));
            return Request<GetFollowerResponse>("https://api.twitch.tv/helix/users/follows", new Parameter("to_id", userid), new Parameter("first", results.ToString()), new Parameter("after", pagination));
        }

        public GetFollowerResponse GetFollows(string userid, int results = 20, string pagination=null) {
            if (string.IsNullOrEmpty(pagination))
                return Request<GetFollowerResponse>("https://api.twitch.tv/helix/users/follows", new Parameter("from_id", userid), new Parameter("first", results.ToString()));
            return Request<GetFollowerResponse>("https://api.twitch.tv/helix/users/follows", new Parameter("from_id", userid), new Parameter("first", results.ToString()), new Parameter("after", pagination));
        }
    }
}