using Discord.Commands;
using Discord.WebSocket;
using Discord;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class UserHelp : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync()
        {
            await Context.Channel.SendMessageAsync("dont be so fucking WEAK");
        }

        public static async Task UserJoined(SocketGuildUser guildUser)
        {
            Console.WriteLine(guildUser + " joined the server");

            var guildUserIGuildUser = guildUser as IGuildUser;

            Console.WriteLine(guildUserIGuildUser.Guild.DefaultChannelId);
            var channelID = guildUserIGuildUser.Guild.DefaultChannelId;
            var guildMainChannel = guildUser.Guild.GetChannel(channelID);
            guildUser.Guild.GetChannel(channelID);







        }
        //  var userSocketGuild = guildUser as SocketGuildUser; //allows more manipulation of user

        ////  var roleIRole = Helpers.FindRole(guildUser, "noob");



        // var channelID  = await guildUser.Guild.GetDefaultChannelAsync() as IMessageChannel;
        //  var channelID2 = userSocketGuild.Guild.DefaultChannel as IMessageChannel;
        //  var mainChannelID = "390097690203258882" as IMessageChannel;
        //  Console.WriteLine($"Iguilduser{channelID.ToString()}, userSocketGuild{channelID2}");

        //await userOfGuild.AddRoleAsync(roleIRole);
        //await channelID.SendMessageAsync("32131232");

    }

    
}
