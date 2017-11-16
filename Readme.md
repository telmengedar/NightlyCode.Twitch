# NightlyCode.Twitch

Provides access to Twitch API. Currently there are a lot of API calls missing as they were not needed until now.

## Dependencies

- [NightlyCode.Japi](https://github.com/telmengedar/japi) for JSON deserialization
- [NightlyCode.Irc](https://github.com/telmengedar/NightlyCode.Irc) for chat client

## Chat

The ChatClient connects to the irc server of twitch and provides the received messages using events.

### Connect to the chat server

The following code example shows a class which connects to the chat server and joins a channel

```

void Main() {
	ChatClient chatclient = new ChatClient();
	chatclient.ChannelJoined += OnChannelJoined;
	chatclient.ChannelLeft += OnChannelLeft;
	
	chatclient.Connect("user", "token");
	chatclient.Join("user");
}

void OnChannelJoined(ChatChannel channel) {
	channel.MessageReceived += OnMessage;
}

void OnChannelLeft(ChatChannel channel) {
	channel.MessageReceived -= OnMessage;
}

void OnMessage(ChatMessage message) {
	Console.WriteLine($"{message.User}: {message.Message}");
}

```

## Twitch Api

Some methods of the new Twitch Api are supported using the TwitchApi class.

## V5 Api

As the Twitch Api does not support everything necessary to interact with a Twitch channel, some V5 Api calls are still implemented. They are marked as obsolete but currently still work.
The following V5 endpoints are implemented with some methods.

- Channels