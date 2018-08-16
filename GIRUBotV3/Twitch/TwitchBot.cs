using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib.Api.Models.v5.Streams;

namespace GIRUBotV3.Modules
{
    public class TwitchBot
    {
        private TwitchAPI _api;

        public TwitchBot(TwitchAPI api)
        {
            _api = api;
        }

        OverwritePermissions allowTalk = new OverwritePermissions(sendMessages: PermValue.Allow);
        OverwritePermissions denyTalk = new OverwritePermissions(sendMessages: PermValue.Deny);
        private IGuild MeleeSlasher;

        public async void NotifyMainOnStreamStart(object sender, OnStreamOnlineArgs e)
        {

            var meleeSlasherMainChannel = await MeleeSlasher.GetTextChannelAsync(Config.MeleeSlasherMainChannel);      


            //Return bool if channel is online/offline.
            var giruTwitchID = await _api.Channels.v5.GetChannelAsync("giru");
            bool isStreaming = await _api.Streams.v5.BroadcasterOnlineAsync(giruTwitchID.Id);

            IRole viewersRole = Helpers.ReturnRole(meleeSlasherMainChannel.Guild as SocketGuild, "Viewers");
            await viewersRole.ModifyAsync(x => x.Mentionable = true);
            
            var viewers = Helpers.ReturnRole(meleeSlasherMainChannel.Guild as SocketGuild, "Viewers");
            await meleeSlasherMainChannel.SendMessageAsync($"{viewers.Mention}, the stream is now online at: https://twitch.tv/giru");

            await viewersRole.ModifyAsync(x => x.Mentionable = false);

           
        }
    }
}
