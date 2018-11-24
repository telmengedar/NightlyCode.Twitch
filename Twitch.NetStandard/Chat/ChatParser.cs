using System;

namespace NightlyCode.Twitch.Chat {
    public static class ChatParser {
        public static UserType ParseUserType(string value) {
            switch(value) {
                case "empty":
                    return UserType.Empty;
                case "mod":
                    return UserType.Mod;
                case "global_mod":
                    return UserType.GlobalMod;
                case "admin":
                    return UserType.Admin;
                case "staff":
                    return UserType.Staff;
                default:
                    throw new Exception($"Unknown usertype '{value}'");
            }
        }
    }
}