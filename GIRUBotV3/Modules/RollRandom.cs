﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    public class RollRandom : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        public async Task EchoAsync([Remainder]string message)
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            await Context.Channel.SendMessageAsync($"{rollVar}% chance of {message}");

        }

        [Command("roll")]
        public async Task EchoAsync()
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            await Context.Channel.SendMessageAsync($"{rollVar}% chance of that happening");

        }

        [Command("rate")]
        public async Task Rate(string message)
        {
            string deusthinkEmoji = Helpers.FindEmoji((SocketGuildUser)Context.User, "deusthink");
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"{deusthinkEmoji} i would rate {message} a {rollVar}/10");
        }

        [Command("rate")]
        public async Task Rate()
        {
            string deusthinkEmoji = Helpers.FindEmoji((SocketGuildUser)Context.User, "deusthink");
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"{deusthinkEmoji} i would rate {Context.User.Mention} a {rollVar}/10");
        }
        [Command("/^[^ ]+test/")]
        public async Task Test2()
        {
            string deusthinkEmoji = Helpers.FindEmoji((SocketGuildUser)Context.User, "deusthink");
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 11);
            await Context.Channel.SendMessageAsync($"asdsadasdsadas0");
        }
    }
}
