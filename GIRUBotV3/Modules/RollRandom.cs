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

        public async Task NounTest(string noun, SocketUserMessage messageContent)
        {
            var chnl = messageContent.Channel as ITextChannel;
            var target = messageContent.MentionedUsers as IGuildUser;

            //match the regex
            Regex regex = new Regex(@"^\![^ ]+test");
            var regexMatch = regex.Match(messageContent.Content);
            var nounTest = regexMatch.ToString();

            //remove the . and the "test" so just noun is remaining
            nounTest = nounTest.Remove(0, 1);
            var nounTestArray = nounTest.Split(' ');
            nounTest = nounTestArray[0];
            nounTest = nounTest.Remove(nounTest.Length - 4);
 
            // grab the passed in string
            var fullMessage = messageContent.Content;
            var fullMessageArray = fullMessage.Split(' ');
            var stringBuilder = new StringBuilder();
            for (int i = 1; i < fullMessageArray.Length; i++)
            {
                stringBuilder.Append(fullMessageArray[i]);
                stringBuilder.Append(" ");
            }

            Random rnd = new Random();
            int rollVar = rnd.Next(0, 101);
            if (target != null)
            {
                //.stupidtest @user
                await messageContent.Channel.SendMessageAsync($"{target.Mention} is {rollVar}% {nounTest}");
                return;
            }
            else if (stringBuilder.ToString().Length < 1)
            {
                //.stupidtest
                await messageContent.Channel.SendMessageAsync($"{messageContent.Author.Mention} is {rollVar}% {nounTest}");
                return;
            }
            else
            {
                //.stupidtest ur mum
                await messageContent.Channel.SendMessageAsync($"{stringBuilder.ToString()}is {rollVar}% {nounTest}");
            }
        }
    }
}
