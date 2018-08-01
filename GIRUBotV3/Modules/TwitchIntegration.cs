using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace GIRUBotV3.Modules
{
    public class TwitchIntegration
    {
        public static TwitchAPI api;

        public static async Task TwitchMainAsync()
        {
            api = new TwitchAPI();
            api.Settings.ClientId = Config.TwitchClientId;
            api.Settings.AccessToken = Config.TwitchAccessToken;
        }

        private IGuild MeleeSlasher;
        private async Task NotifyMainOnStreamStart()
        {
            //Return bool if channel is online/offline.
            var giruTwitchID = await api.Channels.v5.GetChannelAsync("giru");
            bool isStreaming = await api.Streams.v5.BroadcasterOnlineAsync(giruTwitchID.Id);

            var meleeSlasherMainChannel = await MeleeSlasher.GetTextChannelAsync(300832513595670529);
            var viewers = Helpers.ReturnRole(meleeSlasherMainChannel.Guild as SocketGuild, "Viewers");
            await meleeSlasherMainChannel.SendMessageAsync($"{viewers.Mention}, the stream is now online at: https://twitch.tv/giru");
        }        
    }
}
