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
        private async Task Rate(string message)
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
 
       public async Task NounTest(string noun, SocketUserMessage messageContent)
        {
            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);

            var target = messageContent.MentionedUsers as IGuildUser;
            Regex regexNounTest = new Regex("(?s)(.(.*)test)");
            var NounTestToRemove = regexNounTest.Match(messageContent.Content).ToString();

            var messageToSend = messageContent.Content.Remove(0, n);

            if (target != null)
            {
                await messageContent.Channel.SendMessageAsync($"{target.Mention} is {rollVar}% {noun}");
                return;
            }
            else if (messageToSend.Length < 1)
            {
                await messageContent.Channel.SendMessageAsync($"{messageContent.Author.Mention} is {rollVar}% {noun}");
                return;
            } else
            {
                await messageContent.Channel.SendMessageAsync($"{messageToSend} is {rollVar}% {noun}");
            }
          
        }
    }
}
