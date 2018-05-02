using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class Echo : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task EchoAsync([Remainder]string message)
        {
            //var embed = new EmbedBuilder();
            //embed.WithTitle(Context.User.Username + "'s info");
            //embed.WithDescription("test");
            //embed.WithColor(new Color(0, 255, 0));
            //string insult = "asddsada " + await Insults.GetInsult();
        
                await Context.Channel.SendMessageAsync(message);
         
            
        }
    }
}
