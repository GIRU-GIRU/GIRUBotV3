using System;
using System.Collections.Generic;
using System.Text;

namespace GIRUBotV3
{
    static class Global
    {
        public static Config Config = new Config();
    }

    class Config
    {
        public string BotToken;
        public string TwitchClientId;
        public string TwitchAccessToken;
        public ulong AuditChannel;
        public ulong EliteDiscourseChannel;
        public ulong MeleeSlasherMainChannel;
        public ulong MeleeSlasherGuild;
        public ulong TheNoobGateChannel;
        public ulong DeletedMessageLog;
        public ulong OwnerID;
        public string CommandPrefix;
        public string DBLocation;

    }
}
