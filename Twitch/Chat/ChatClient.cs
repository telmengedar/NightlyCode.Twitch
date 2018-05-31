using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NightlyCode.IRC;

namespace NightlyCode.Twitch.Chat {

    /// <summary>
    /// client for twitch irc chat
    /// </summary>
    /// <remarks>
    /// use this to connect to twitch chat and join channels
    /// </remarks>
    public class ChatClient {
        const int messagelimit = 20;
        readonly TimeSpan messagethreshold = TimeSpan.FromSeconds(30.0);

        readonly IrcClient ircclient = new IrcClient();

        readonly object sendlock = new object();
        readonly Queue<DateTime> messagetimes=new Queue<DateTime>();

        readonly object channellock = new object();
        readonly Dictionary<string, ChatChannel> channels = new Dictionary<string, ChatChannel>();

        readonly ManualResetEvent connectionwait = new ManualResetEvent(false);

        string user;

        /// <summary>
        /// creates a new <see cref="ChatClient"/>
        /// </summary>
        public ChatClient() {
            ircclient.MessageReceived += OnMessageReceived;
            ircclient.Disconnected += OnDisconnected;
        }

        public event Action<ChatChannel> ChannelJoined;

        public event Action<ChatChannel> ChannelLeft;

        public event Action<string> CapAcknowledged;

        void OnDisconnected() {
            lock(channellock) {
                //foreach(ChatChannel channel in channels.Values)
                //    channel.Part();
                channels.Clear();
            }
            Disconnected?.Invoke();
        }

        string GetChannelName(string name) {
            if (name.StartsWith("#"))
                name = name.Substring(1);
            return name;
        }
        /// <summary>
        /// get joined channel by name
        /// </summary>
        /// <param name="name">name of channel</param>
        /// <returns>channel or null if channel is not found</returns>
        public ChatChannel GetChannel(string name) {
            name = GetChannelName(name);
            lock(channellock) {
                ChatChannel channel;
                channels.TryGetValue(name, out channel);
                return channel;
            }
        }

        void SendChannelMessage(string channelname, IrcMessage message) {
            ChatChannel channel = GetChannel(GetChannelName(channelname));
            if(channel != null)
                channel.OnMessage(message);
            else Logger.Warning(this, $"Message for channel '{channelname}' but channel not registered in client.", message.ToString());
        }

        void ProcessMessage(IrcMessage message) {
            string channelname;
            switch (message.Command)
            {
                case "CAP":
                    if(message.Arguments.Length < 3)
                        break;

                    if(message.Arguments[1] == "ACK")
                        CapAcknowledged?.Invoke(message.Arguments[2]);
                    break;
                case "PING":
                    SendMessage(new IrcMessage("PONG", message.Arguments));
                    break;
                case "JOIN":
                    channelname = GetChannelName(message.Arguments[0]);

                    if (message.ExtractUser().ToLower() == user.ToLower())
                        JoinChannel(channelname);
                    SendChannelMessage(channelname, message);
                    break;
                case "PART":
                    channelname = GetChannelName(message.Arguments[0]);
                    SendChannelMessage(channelname, message);

                    if (message.ExtractUser() == channelname)
                    {
                        ChatChannel channel;
                        lock (channellock)
                        {
                            if (channels.TryGetValue(channelname, out channel))
                                channels.Remove(channelname);
                        }
                        if (channel != null)
                            ChannelLeft?.Invoke(channel);
                    }
                    break;
                case "001":
                case "002":
                case "003":
                case "004":
                    // connection success
                    break;
                case "372":
                case "375":
                    // message of the day
                    break;
                case "376":
                    connectionwait.Set();
                    // end of message of the day
                    break;
                case "353":
                    GetChannel(message.Arguments[2])?.OnMessage(message);
                    break;
                case "366":
                    GetChannel(message.Arguments[1])?.OnMessage(message);
                    break;
                case "ROOMSTATE":
                case "PRIVMSG":
                case "USERNOTICE":
                case "NOTICE":
                case "USERSTATE":
                    SendChannelMessage(message.Arguments[0], message);
                    break;
                case "HOSTTARGET":
                    if (!message.Arguments[1].StartsWith("-"))
                        SendChannelMessage(message.Arguments[1], message);
                    break;
                case "MODE":
                    // channel or user mode ... not that important for now
                    break;
                default:
                    Logger.Warning(this, "Unprocessed message", message.ToString());
                    break;
            }
        }

        void OnMessageReceived(IrcMessage message) {
            try {
                ProcessMessage(message);
            }
            catch(Exception e) {
                Logger.Error(this, $"Error processing irc message '{message}'", e);
            }
        }

        internal void SendMessage(IrcMessage message) {
            lock(sendlock) {
                while(messagetimes.Count >= messagelimit) {
                    DateTime now = DateTime.Now;
                    while(messagetimes.Count>0 && now - messagetimes.First() > messagethreshold)
                        messagetimes.Dequeue();
                    Thread.Sleep(10);
                }
                ircclient.SendMessage(message);
                messagetimes.Enqueue(DateTime.Now);
            }
        }

        /// <summary>
        /// triggered when the client disconnected from twitch
        /// </summary>
        public event Action Disconnected;

        void JoinChannel(string channelname) {
            ChatChannel channel = new ChatChannel(channelname, this);
            lock (channellock)
                channels[channelname] = channel;
            ChannelJoined?.Invoke(channel);
        }

        /// <summary>
        /// connects to the twitch irc server (doesn't join a channel)
        /// </summary>
        public void Connect(string user, string oauth) {
            this.user = user;

            ircclient.Connect("irc.twitch.tv");
            ircclient.SendMessage(new IrcMessage("PASS", $"oauth:{oauth}"));
            ircclient.SendMessage(new IrcMessage("NICK", user));
            connectionwait.WaitOne(TimeSpan.FromSeconds(10.0));

            ircclient.SendMessage(new IrcMessage("CAP", "REQ", "twitch.tv/membership"));
            ircclient.SendMessage(new IrcMessage("CAP", "REQ", "twitch.tv/tags"));
            ircclient.SendMessage(new IrcMessage("CAP", "REQ", "twitch.tv/commands"));
        }

        /// <summary>
        /// disconnects from the twitch irc server
        /// </summary>
        public void Disconnect() {
            ircclient.Disconnect();
        }

        /// <summary>
        /// joins a channel in twitch
        /// </summary>
        /// <param name="channelname">name of channel to join</param>
        /// <returns>channel object to use for communication</returns>
        public void Join(string channelname) {
            SendMessage(new IrcMessage($"JOIN #{channelname.ToLower()}"));
        }
    }
}