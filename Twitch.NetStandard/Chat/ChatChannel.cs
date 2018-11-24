using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NightlyCode.IRC;

namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// channel where users can chat
    /// </summary>
    public class ChatChannel {
        readonly string name;
        readonly ChatClient client;
        readonly HashSet<string> users = new HashSet<string>();

        /// <summary>
        /// creates a new <see cref="ChatChannel"/>
        /// </summary>
        /// <param name="name">name of channel</param>
        /// <param name="client">client used for communication</param>
        public ChatChannel(string name, ChatClient client) {
            this.name = name;
            this.client = client;
        }

        /// <summary>
        /// triggered when the known userlist has changed
        /// </summary>
        public event Action<ChatChannel> UsersChanged;

        /// <summary>
        /// triggered when a user has joined
        /// </summary>
        public event Action<ChatChannel, string> UserJoined;

        /// <summary>
        /// triggered when a user has left
        /// </summary>
        public event Action<ChatChannel, string> UserLeft;

        /// <summary>
        /// triggered when a chat message was received
        /// </summary>
        public event Action<ChatMessage> MessageReceived;

        /// <summary>
        /// triggered when settings of the channel were changed
        /// </summary>
        public event Action<ChatChannel> SettingsChanged;

        /// <summary>
        /// triggered when a subscription notice was received
        /// </summary>
        public event Action<Subscription> Subscription;

        /// <summary>
        /// triggered when some channel is hosting this channel
        /// </summary>
        public event Action<HostInformation> Host;

        /// <summary>
        /// triggered when this channel is hosting some other channel
        /// </summary>
        public event Action<HostInformation> Hosting;

        /// <summary>
        /// triggered when some channel is raiding this channel
        /// </summary>
        public event Action<RaidNotice> Raid;

        /// <summary>
        /// triggered when a notice from server was received
        /// </summary>
        public event Action<Notice> Notice;

        /// <summary>
        /// triggered when a userstate was received
        /// </summary>
        public event Action<UserState> UserState;

        /// <summary>
        /// name of channel
        /// </summary>
        public string Name => name;

        /// <summary>
        /// language of chat channel (can be null)
        /// </summary>
        public CultureInfo Language { get; private set; }

        public bool R9K { get; private set; }
        public int Slow { get; private set; }
        public bool SubscribersOnly { get; set; }

        internal void OnMessage(IrcMessage message) {
            switch(message.Command) {
                case "353":
                    foreach(string user in message.Arguments[3].Split(' '))
                        users.Add(user);
                    break;
                case "366":
                    UsersChanged?.Invoke(this);
                    break;
                case "PRIVMSG":
                    ReceiveMessage(message);
                    break;
                case "ROOMSTATE":
                    ReceiveRoomState(message);
                    break;
                case "USERNOTICE":
                    switch(message.Tags.FirstOrDefault(t => t.Key == "msg-id")?.Value) {
                        case "raid":
                            ReceiveRaidNotice(message);
                            break;
                        case "sub":
                        case "resub":
                        case "charity":
                            ReceiveUserNotice(message);
                            break;
                        default:
                            Logger.Warning(this, "Unknown user message type", message.ToString());
                            break;
                    }
                    break;
                case "JOIN":
                    ReceiveJoin(message);
                    break;
                case "PART":
                    ReceivePart(message);
                    break;
                case "HOSTTARGET":
                    ReceiveHostTarget(message);
                    break;
                case "NOTICE":
                    ReceiveNotice(message);
                    break;
                case "USERSTATE":
                    ReceiveUserState(message);
                    break;
            }
        }

        void ReceiveUserState(IrcMessage message) {
            UserState userstate = new UserState();

            foreach (IrcTag attribute in message.Tags)
            {
                if (string.IsNullOrEmpty(attribute.Value))
                    continue;

                switch (attribute.Key)
                {
                    case "color":
                        userstate.Color = attribute.Value;
                        break;
                    case "display-name":
                        userstate.DisplayName = attribute.Value;
                        break;
                    case "emote-sets":
                        userstate.EmoteSets = attribute.Value.Split(',').Select(int.Parse).ToArray();
                        break;
                    case "mod":
                        userstate.IsMod = attribute.Value == "1";
                        break;
                    case "subscriber":
                        userstate.IsSubscriber = attribute.Value == "1";
                        break;
                    case "turbo":
                        userstate.IsTurbo = attribute.Value == "1";
                        break;
                    case "user-type":
                        userstate.UserType = ChatParser.ParseUserType(attribute.Value);
                        break;
                    default:
                        Logger.Info(this, $"Unknown tag '{attribute.Key}' with value '{attribute.Value}' in user state message");
                        break;
                }
            }

            UserState?.Invoke(userstate);
        }

        void ReceivePart(IrcMessage message) {
            string user = message.ExtractUser();
            users.Remove(user);
            UserLeft?.Invoke(this, user);
        }

        void ReceiveJoin(IrcMessage message) {
            string user = message.ExtractUser();
            users.Add(user);
            UserJoined?.Invoke(this, user);
        }

        void ReceiveNotice(IrcMessage message) {
            Notice?.Invoke(new Notice(name, message.Tags.FirstOrDefault(t => t.Key == "msg-id")?.Value, message.Arguments[1]));
        }

        void ReceiveHostTarget(IrcMessage message) {
            HostInformation hostinformation = CreateHostInformation(message);

            if(hostinformation.Channel != "-") {
                if(hostinformation.Host == name)
                    Hosting?.Invoke(hostinformation);
                else Host?.Invoke(hostinformation);
            }
        }

        HostInformation CreateHostInformation(IrcMessage message) {
            if(message.Arguments.Length == 3)
                return new HostInformation(message.Arguments[0].Substring(1), message.Arguments[1], int.Parse(message.Arguments[2]));

            if(message.Arguments[1].Contains(' ')) {
                string[] split = message.Arguments[1].Split(' ');
                int viewers;
                if(!int.TryParse(split[1], out viewers))
                    viewers = -1;
                return new HostInformation(message.Arguments[0].Substring(1), split[0], viewers);
            }

            return new HostInformation(message.Arguments[0].Substring(1), message.Arguments[1], -1);
        }

        void ReceiveUserNotice(IrcMessage message) {
            Subscription subscription = new Subscription {
                Channel = name
            };

            if(message.Arguments.Length > 1)
                subscription.Message = message.Arguments[1];

            foreach(IrcTag attribute in message.Tags) {
                if (string.IsNullOrEmpty(attribute.Value))
                    continue;

                switch(attribute.Key) {
                    case "badges":
                        subscription.Badges = ExtractBadges(attribute.Value).ToArray();
                        break;
                    case "color":
                        subscription.Color = attribute.Value;
                        break;
                    case "id=raid;msg-param-displayName":
                    case "display-name":
                        subscription.DisplayName = attribute.Value;
                        break;
                    case "emotes":
                        subscription.Emotes = ExtractEmotes(attribute.Value).OrderBy(e=>e.FirstIndex).ToArray();
                        break;
                    case "mod":
                        subscription.IsMod = attribute.Value == "1";
                        break;
                    case "msg-id":
                        subscription.SubscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), attribute.Value, true);
                        break;
                    case "msg-param-months":
                        subscription.Months = int.Parse(attribute.Value);
                        break;
                    case "msg-param-sub-plan":
                        subscription.Plan = attribute.Value;
                        break;
                    case "msg-param-sub-plan-name":
                        subscription.PlanName = attribute.Value;
                        break;
                    case "room-id":
                        subscription.RoomID = attribute.Value;
                        break;
                    case "subscriber":
                        subscription.IsSubscriber = attribute.Value == "1";
                        break;
                    case "system-msg":
                        subscription.SystemMessage = attribute.Value;
                        break;
                    case "turbo":
                        subscription.IsTurbo = attribute.Value == "1";
                        break;
                    case "user":
                    case "login":
                    case "msg-param-login":
                        subscription.User = attribute.Value;
                        break;
                    case "user-id":
                        subscription.UserID = attribute.Value;
                        break;
                    case "user-type":
                        subscription.UserType = (UserType)Enum.Parse(typeof(UserType), attribute.Value.Replace("_", ""), true);
                        break;
                    case "emote-only":
                        // this basically just tells us that the user has sent a message containing only emotes ... whatever ...
                        break;
                    default:
                        Logger.Info(this, $"Unknown tag '{attribute.Key}' with value '{attribute.Value}' in user notice message");
                        break;
                }
            }

            Subscription?.Invoke(subscription);
        }

        void ReceiveRaidNotice(IrcMessage message)
        {
            RaidNotice raid=new RaidNotice();

            foreach(IrcTag attribute in message.Tags) {
                if(string.IsNullOrEmpty(attribute.Value))
                    continue;

                switch(attribute.Key) {
                    case "color":
                        raid.Color = attribute.Value;
                        break;
                    case "msg-param-displayName":
                    case "display-name":
                        raid.DisplayName = attribute.Value;
                        break;
                    case "msg-param-login":
                    case "login":
                        raid.Login = attribute.Value;
                        break;
                    case "msg-param-profileImageURL":
                        raid.Avatar = attribute.Value;
                        break;
                    case "msg-param-viewerCount":
                        raid.RaiderCount = int.Parse(attribute.Value);
                        break;
                    case "room-id":
                        raid.RoomID = attribute.Value;
                        break;
                    case "system-msg":
                        raid.SystemMessage = attribute.Value;
                        break;
                }
            }

            Raid?.Invoke(raid);
        }

        void ReceiveRoomState(IrcMessage message) {
            foreach(IrcTag attribute in message.Tags) {
                if(string.IsNullOrEmpty(attribute.Value))
                    continue;

                switch(attribute.Key) {
                    case "broadcaster-lang":
                        try {
                            Language = CultureInfo.GetCultureInfo(attribute.Value);
                        }
                        catch(Exception e) {
                            Logger.Error(this, $"Unable to convert '{attribute.Value}' to a valid culture.", e);
                        }
                        break;
                    case "r9k":
                        R9K = attribute.Value == "1";
                        break;
                    case "slow":
                        Slow = int.Parse(attribute.Value);
                        break;
                    case "subs-only":
                        SubscribersOnly = attribute.Value == "1";
                        break;
                    case "emote-only":
                    case "followers-only":
                    case "rituals":
                    case "room-id":
                        break;
                    default:
                        Logger.Info(this, $"Unknown tag '{attribute.Key}' with value '{attribute.Value}' in room state message");
                        break;
                }
            }

            SettingsChanged?.Invoke(this);
        }

        IEnumerable<Emote> ExtractEmotes(string value) {
            foreach(string emotecollection in value.Split('/')) {
                string[] split = emotecollection.Split(':');
                int id = int.Parse(split[0]);
                foreach(string indexinformation in split[1].Split(',')) {
                    string[] indexsplit = indexinformation.Split('-');
                    yield return new Emote(id, int.Parse(indexsplit[0]), int.Parse(indexsplit[1]));
                }
            }
        }

        IEnumerable<Badge> ExtractBadges(string value) {
            return value.Split(',').Select(a => {
                string[] badgesplit = a.Split('/');
                return new Badge(badgesplit[0], int.Parse(badgesplit[1]));
            });
        }

        void ReceiveMessage(IrcMessage message) {
            ChatMessage chatmessage = new ChatMessage {
                User = message.ExtractUser(),
                Channel = message.Arguments[0].Substring(1),
                Message = message.Arguments[1]
            };

            if(message.Tags != null) {
                foreach(IrcTag attribute in message.Tags) {
                    if(string.IsNullOrEmpty(attribute.Value))
                        continue;

                    switch(attribute.Key) {
                        case "badges":
                            chatmessage.Badges = ExtractBadges(attribute.Value).ToArray();
                            break;
                        case "bits":
                            chatmessage.Bits = int.Parse(attribute.Value);
                            break;
                        case "color":
                            chatmessage.Color = attribute.Value;
                            break;
                        case "display-name":
                            chatmessage.DisplayName = attribute.Value;
                            break;
                        case "emotes":
                            chatmessage.Emotes = ExtractEmotes(attribute.Value).OrderBy(e => e.FirstIndex).ToArray();
                            break;
                        case "id":
                            chatmessage.ID = attribute.Value;
                            break;
                        case "mod":
                            chatmessage.IsMod = attribute.Value == "1";
                            break;
                        case "room-id":
                            chatmessage.RoomID = attribute.Value;
                            break;
                        case "subscriber":
                            chatmessage.IsSubscriber = attribute.Value == "1";
                            break;
                        case "turbo":
                            chatmessage.IsTurbo = attribute.Value == "1";
                            break;
                        case "user-id":
                            chatmessage.UserID = attribute.Value;
                            break;
                        case "user-type":
                            chatmessage.UserType = (UserType)Enum.Parse(typeof(UserType), attribute.Value.Replace("_", ""), true);
                            break;
                        case "sent-ts":
                        case "tmi-sent-ts":
                            // not too interested in the timestamps for now
                            break;
                        case "emote-only":
                            // this basically just tells us that the user has sent a message containing only emotes ... whatever ...
                            break;
                        default:
                            Logger.Info(this, $"Unknown tag '{attribute.Key}' with value '{attribute.Value}' in chat message");
                            break;
                    }
                }
            }
            MessageReceived?.Invoke(chatmessage);
        }

        /// <summary>
        /// list of joined users
        /// </summary>
        public IEnumerable<string> Users => users;

        /// <summary>
        /// leaves the channel
        /// </summary>
        public void Part() {
            client.SendMessage(new IrcMessage("PART", $"#{name}"));
        }

        /// <summary>
        /// sends a message to the channel
        /// </summary>
        /// <param name="message">message to send</param>
        public void SendMessage(string message) {
            client.SendMessage(new IrcMessage("PRIVMSG", $"#{name}", message));
        }
    }
}