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
    public class test : ModuleBase<SocketCommandContext>
    {


        [Command("unusued")]
        public async Task Test()
        {

          string testmessage = Helpers.FindEmoji((SocketGuildUser)Context.User, "deusthink");
            await Context.Channel.SendMessageAsync(testmessage.ToString());


        }
        //    var embed = new EmbedBuilder();


        //    if (!Helpers.IsRole("test", (SocketGuildUser)Context.User))
        //    {
                
        //        embed.WithTitle(Context.User.Username + "'s test");
        //        embed.WithDescription("test");
        //        embed.WithColor(new Color(0, 255, 0));
        //        string heNotMod = "asddsada " + await Insults.GetInsult();
        //        await Context.Channel.SendMessageAsync("", false, embed);
        //        return;
        //    }

        //    string heIsMod = "asddsada " + await Insults.GetInsult();

        //    embed.WithTitle(Context.User.Username + "'s test");
        //    embed.WithDescription("test");
        //    embed.WithColor(new Color(0, 255, 0));
            
        //    await Context.Channel.SendMessageAsync("", false, embed);
            
        //}
    }
}
