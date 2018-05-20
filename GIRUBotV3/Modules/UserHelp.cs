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
            // casting
            var guildUserIGuildUser = guildUser as IGuildUser;
            var channelID = guildUserIGuildUser.Guild.DefaultChannelId;
            var guildMainChannel = guildUser.Guild.GetChannel(channelID);
            var chnl = guildMainChannel as ITextChannel;

            Console.WriteLine($"{guildUser} {guildUser.Id}  joined the server");

            // assigning noob role
            var noobRole = Helpers.FindRole(guildUser, "noob");
            await guildUser.AddRoleAsync(noobRole);

            // welcoming
            var insult = Insults.GetInsult();
            Random rnd = new Random();
            string[] welcomeArray = new string[]
            {
               $"{guildUser.Mention} has joined Melee Slasher, everybody welcome this {insult}",
               $"{guildUser.Mention} has joined the server",
               $"what's up {guildUser.Mention} ",
               $"hi {insult}!😃 {guildUser.Mention} ",
               $"{guildUser.Mention} join server guys 😃😃😃 ",
               $"welcome {guildUser.Mention}",
               $"{guildUser.Mention} has just joined the server ",
               $"{guildUser.Mention} has connected to the server",

            };
            int pull = rnd.Next(welcomeArray.Length);
            string welcomeMessage = welcomeArray[pull].ToString();
    
            await chnl.SendMessageAsync(welcomeMessage);

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
