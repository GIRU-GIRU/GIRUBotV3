using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;

namespace GIRUBotV3.Modules
{
    public class TwitchIntegration
    {
        public static TwitchAPI api;

        public static async Task TwitchMainAsync()
        {
            api = new TwitchAPI();
            api.Settings.ClientId = "client_id";
            api.Settings.AccessToken = "access_token";
        }
        private async Task NotifyMainOnStreamStart()
        {
            //Return bool if channel is online/offline.
            bool isStreaming = await api.Streams.v5.BroadcasterOnlineAsync("channel_id");

        }
    }
}
