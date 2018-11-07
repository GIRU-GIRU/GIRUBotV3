﻿using Discord.Commands;
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
            var guildMainChannel = guildUser.Guild.GetChannel(Config.MeleeSlasherMainChannel);
            var chnl = guildMainChannel as ITextChannel;
            // assigning noob role
            var noobRole = Helpers.FindRole(guildUser, "noob");
            await guildUser.AddRoleAsync(noobRole);


            if (CommandToggles.WelcomeMessages)
            {
                // welcoming
                var insult = await Insults.GetInsult();
                Random rnd = new Random();
                string[] welcomeArray = new string[]
                {
               $"{guildUser.Mention} has joined Melee Slasher, the {insult} is sitting in the shitter lobby",
               $"{guildUser.Mention} has joined the server, they are now waiting in the noob gate",
               $"{guildUser.Mention} is now waiting in the noob gate",
               $"{guildUser.Mention} join server guys 😃😃😃, they now wait in the noob gate",
               $"some {insult} called {guildUser.Mention} is now sitting in the noob gate",
               $"{guildUser.Mention} has just joined the server, waiting in the noob gate for attending to",
               $"{guildUser.Mention} has connected to the server, they're now sat in the noob gate",

                };
                int pull = rnd.Next(welcomeArray.Length);
                string welcomeMessage = welcomeArray[pull].ToString();
                await chnl.SendMessageAsync(welcomeMessage);
            }

            //log it
            ITextChannel logChannel = guildUser.Guild.GetChannel(492381877630402572) as ITextChannel;
            await logChannel.SendMessageAsync($"{guildUser.Username}#{guildUser.Discriminator} joined Melee Slasher. UserID = {guildUser.Id}");

        }

        [Command("say")]
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        private async Task SayInMain([Remainder]string message)
        {
            var chnl = Context.Guild.GetTextChannel(Config.MeleeSlasherMainChannel);
            await chnl.SendMessageAsync(message);
        }


        //[Command("saytest")]
        //private async Task SaTest([Remainder]string message)
        //{
        //    var guildUser = Context.User as IGuildUser;
        //    ITextChannel logChannel = await guildUser.Guild.GetChannelAsync(492381877630402572) as ITextChannel;

        //    if (guildUser.JoinedAt.HasValue)
        //    {
        //        string[] dateArray = guildUser.JoinedAt.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm").Split(" ");
        //        userJoinedDate = dateArray[0] + ", at " + dateArray[1];
        //    }
        //    await logChannel.SendMessageAsync($"{guildUser.Username}#{guildUser.Discriminator} joined Melee Slasher on {userJoinedDate}. UserID = {guildUser.Id}");
        //}


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
