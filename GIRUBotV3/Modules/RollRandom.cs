using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GIRUBotV3.Modules
{
    public class RollRandom : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        private async Task RandomRoll([Remainder]string message)
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            await Context.Channel.SendMessageAsync($"{rollVar}% chance of {message}");

        }

        [Command("roll")]
        private async Task RandomRoll()
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            await Context.Channel.SendMessageAsync($"{rollVar}% chance of that happening");

        }

        [Command("rate")]
        private async Task Rate([Remainder]string message)
        {
            string deusthinkEmoji = Helpers.FindEmoji((SocketGuildUser)Context.User, "deusthink");
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"{deusthinkEmoji} i would rate {message} a {rollVar}/10");
        }

        [Command("rate")]
        private async Task Rate()
        {
            string deusthinkEmoji = Helpers.FindEmoji((SocketGuildUser)Context.User, "deusthink");
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"{deusthinkEmoji} i would rate {Context.User.Mention} a {rollVar}/10");
        }

    }
}
