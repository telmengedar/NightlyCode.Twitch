using System;
using NightlyCode.Core.Logs;
using NightlyCode.Twitch.Chat;

namespace NightlyCode.Twitch.Client
{
    class Program {
        static void Main(string[] args)
        {
            Logger.EnableConsoleLogging();
            ChatClient chatclient = new ChatClient();

            chatclient.Disconnected += OnDisconnected;
            chatclient.ChannelJoined += OnChannelJoined;
            chatclient.ChannelLeft += OnChannelLeft;

            try
            {
                chatclient.Connect(args[0], args[1]);
            }
            catch(Exception e) {
                Logger.Error(typeof(Program), "Unable to connect to twitch", e);
            }

            Logger.Info("Program", "Connected");
            chatclient.Join(args[0]);

            string line = "";
            do {
                line = Console.ReadLine();
                if(line.ToLower() != "quit") {
                    
                    int index = line.IndexOf(' ');
                    if(index > -1) {
                        ChatChannel channel=chatclient.GetChannel(line.Substring(0, index));
                        if(channel != null)
                            channel.SendMessage(line.Substring(index + 1));
                        else Logger.Error("Program", $"Channel '{line.Substring(0, index)}' not found");
                    }
                    else Logger.Error("Program", "Invalid syntax");
                }
            }
            while(line.ToLower() != "quit");
            
        }

        static void OnChannelLeft(ChatChannel channel) {
            Logger.Info("Program", $"Left channel '{channel.Name}'");
            channel.Subscription -= OnSubscription;
            channel.Host -= OnHost;
            channel.MessageReceived -= OnMessage;
            channel.Notice -= OnNotice;
            channel.SettingsChanged -= OnSettingsChanged;
            channel.UserJoined -= OnUserJoined;
            channel.UserLeft -= OnUserLeft;
            channel.UsersChanged -= OnUsersChanged;
        }

        static void OnSettingsChanged(ChatChannel channel) {
            Logger.Info(channel.Name, "Settings changed", $"Language={channel.Language?.Name}, R9K={channel.R9K}, Slow={channel.Slow}, SubscribersOnly={channel.SubscribersOnly}");
        }

        static void OnUsersChanged(ChatChannel channel) {
            Logger.Info(channel.Name, "Users changed", string.Join(", ", channel.Users));
        }

        static void OnUserLeft(ChatChannel channel, string user) {
            Logger.Info(channel.Name, $"{user} has left channel");
        }

        static void OnUserJoined(ChatChannel channel, string user) {
            Logger.Info(channel.Name, $"{user} has joined channel");
        }

        static void OnNotice(Notice notice) {
            Logger.Info("Twitch", notice.Message);
        }

        static void OnMessage(ChatMessage message) {
            Logger.Info(message.Channel, $"{message.User}: {message.Message}");
        }

        static void OnHost(HostInformation host) { 
            Logger.Info(host.Channel, $"New host from {host.Host} with {host.Viewers} viewers");
        }

        static void OnChannelJoined(ChatChannel channel) {
            Logger.Info("Program", $"Joined channel '{channel.Name}'");
            channel.Subscription += OnSubscription;
            channel.Host += OnHost;
            channel.MessageReceived += OnMessage;
            channel.Notice += OnNotice;
            channel.SettingsChanged += OnSettingsChanged;
            channel.UserJoined += OnUserJoined;
            channel.UserLeft += OnUserLeft;
            channel.UsersChanged += OnUsersChanged;
        }

        static void OnSubscription(Subscription obj) {
            Logger.Info(typeof(Program), $"New {obj.SubscriptionType} subscription from {obj.User}");
        }

        static void OnDisconnected() {
            Logger.Info(typeof(Program), "Twitch disconnected");
        }
    }
}
