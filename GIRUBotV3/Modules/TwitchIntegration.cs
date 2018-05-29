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
            api.Settings.ClientId = "5dqjaoh13oxbzgwzvi05vuov0c02zk";
            api.Settings.AccessToken = "oauth:z8b81ov1bu6xkvf2vgc0diwzrfh1hr";
        }

        private IGuild meleeSlasher;
        private async Task NotifyMainOnStreamStart()
        {
            //Return bool if channel is online/offline.
            var giruTwitchID = await api.Channels.v5.GetChannelAsync("giru");
            bool isStreaming = await api.Streams.v5.BroadcasterOnlineAsync(giruTwitchID.Id);
           
            var meleeSlasherMainChannel = await meleeSlasher.GetChannelAsync(300832513595670529);
            var chnl = meleeSlasherMainChannel as ITextChannel;
            await chnl.SendMessageAsync("he live");
        }        
    }
}
