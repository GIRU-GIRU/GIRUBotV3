using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Personality
{
     class Nountest
    {
        public static bool CheckForNountest(string messageContentStart)
        {
            bool nounTestExists = false;
            return (messageContentStart[0] == '!' && messageContentStart.ToLower().EndsWith("test"));
        }

        public async Task PostNounTest(SocketCommandContext context)
        {
            string nounTest = context.Message.Content.Split(" ")[0];
            string noun = String.Concat(nounTest.Skip(1));
            var rnd = new Random();
            var testCount = rnd.Next(0, 101);
            noun = noun.Substring(0, noun.Length - 4);

            string message = String.Join(' ', context.Message.Content.Split(" ").Skip(1));

            var deusThinkEmoji = Helpers.FindEmoji(context.User as SocketGuildUser, "deusthink");

            if (message.Length < 1)
            {
                await context.Channel.SendMessageAsync($"{deusThinkEmoji} {context.User.Mention} is {testCount}% {noun}");
            }
            else
            {
                await context.Channel.SendMessageAsync($"{deusThinkEmoji} {message} is {testCount}% {noun}");
            }
        }
    }
}
