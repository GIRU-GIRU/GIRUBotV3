using Discord.Commands;
using Discord.WebSocket;
using Discord;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;

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
            var insult = await Insults.GetInsult();
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

        [Command("say")]
        [RequireUserPermission(GuildPermission.Administrator)]
        private async Task SayInMain([Remainder]string message)
        {
            var chnl = Context.Guild.GetTextChannel(300832513595670529);
            await chnl.SendMessageAsync(message);
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.MoveMembers)]
        private async Task WarnUser(IGuildUser user)
        {

            string warningMessage = await Insults.GetWarning();
            try
            {
                await user.SendMessageAsync(warningMessage);
                await Context.Channel.SendMessageAsync($"⚠      *** {user.Username} has received a warning.      ⚠***");
            }
            catch (HttpException ex)
            {
                await Context.Channel.SendMessageAsync(user.Mention + ", " + warningMessage);
            }
        }
        [Command("bancleanse")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUserAndCleanse()
        {
            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Bans & Cleanses a {insult} from this sacred place");
            embed.WithDescription("**Usage**: .ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Chat Purge**: 24 hours. \n" +
                "**Ban length:** Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        private async Task BanUser()
        {
            var insult = await Insults.GetInsult();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Permanently ends some {insult} from this sacred place");
            embed.WithDescription("**Usage**: .ban \"user\" \"reason\"\n" +
                "**Target**: arrogant shitters \n" +
                "**Length**: Indefinite.");
            embed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }

    
}
