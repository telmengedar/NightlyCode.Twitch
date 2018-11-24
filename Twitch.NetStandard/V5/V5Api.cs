using System;
using System.Net;
using NightlyCode.Japi.Json;

namespace NightlyCode.Twitch.V5 {

    /// <summary>
    /// base class for v5 api requests
    /// </summary>
    [Obsolete("Twitch V5 Api will be deactivated on 2018-12-31")]
    public class V5Api {
        readonly string clientid;
        readonly string oauth;

        /// <summary>
        /// creates a new <see cref="V5Api"/>
        /// </summary>
        /// <param name="clientid">client id of application</param>
        /// <param name="oauth">oauth token used to authenticate requests</param>
        public V5Api(string clientid, string oauth) {
            this.clientid = clientid;
            this.oauth = oauth;
        }

        protected T Request<T>(string url, params Parameter[] parameters) {
            using(WebClient wc = new WebClient()) {
                wc.Headers.Add("Accept", "application/vnd.twitchtv.v5+json");
                wc.Headers.Add("Client-ID", clientid);
                wc.Headers.Add("Authorization", $"OAuth {oauth}");

                foreach(Parameter parameter in parameters)
                    wc.QueryString.Add(parameter.Key, parameter.Value);

                string response=wc.DownloadString(url);
                return JSON.Read<T>(response);
            }
        }
    }
}