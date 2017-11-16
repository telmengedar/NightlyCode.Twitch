using System;
using System.Threading;
using NightlyCode.IRC;

namespace NightlyCode.Twitch.Chat {
    public class WaitState {
        readonly object state = new object();
        readonly Func<IrcMessage, bool> predicate;

        public WaitState(Func<IrcMessage, bool> predicate) {
            this.predicate = predicate;
        }

        /// <summary>
        /// message received by server
        /// </summary>
        public IrcMessage Message { get; private set; }

        public void Wait() {
            lock(state)
                Monitor.Wait(state, TimeSpan.FromSeconds(10.0));
        }

        public bool Continue(IrcMessage message) {
            if(!predicate(message))
                return false;
                
            Message = message;
            lock(state)
                Monitor.Pulse(state);
            return true;
        }
    }
}