using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using GIRUBotV3.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Models.Helix.Users.GetUsersFollows;
using TwitchLib.Api.Models.v5.Subscriptions;




namespace GIRUBotV3
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            var bot = program.RunBotAsync();
            bot.Wait();
            var runTwitch = TwitchIntegration.TwitchMainAsync();        
        }

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public async Task RunBotAsync()
        {
            string botToken = "";
            //string botToken = ConfigurationManager.AppSettings["AuthToken"];

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                 .AddSingleton(_client)
                 .AddSingleton(_commands)
                 .BuildServiceProvider();

            _client.UserJoined += UserHelp.UserJoined;
            _client.MessageReceived += MessageContainsAsync;
            _client.Log += Log;
            //register modules and login bot with auth credentials
            await RegisterCommandAsync();
            await _client.LoginAsync(TokenType.Bot, botToken);
            //starting client and continue forever
            await _client.StartAsync();

            await Task.Delay(-1);
        }


        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private Regex regexNounTest = new Regex("(?s)(.(.*)test)");
        private async Task MessageContainsAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;
            var context = new SocketCommandContext(_client, message);

            if (message.Content.Contains("😃"))
            {
                var r = new Random();
                if (r.Next(1, 15) <= 2)
                {
                    await context.Channel.SendMessageAsync("😃");
                }
            }
            if (message.Content.Contains(" help "))
            {
                var r = new Random();
                if (r.Next(1, 15) >= 2)
                {
                    await context.Channel.SendMessageAsync("stop crying for help");
                }
            }
            if (regexNounTest.Match(message.Content).Success)
            {
                Console.WriteLine("regex code reached");
                var noun = regexNounTest.Match(message.Content).Groups[2].ToString();
                var nounTestTask = new RollRandom();
                await nounTestTask.NounTest(noun, message);
            }
        }

        //Handle Commands
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;
            //if (message is null || message.ToString().Length < 2)
            //{
            //    //string noPerm = await Insults.GetNoPerm();
            //   await arg.Channel.SendMessageAsync("asdsad");

            //    return;
            //}
           
            
            int argPos = 0;
            //does the message start with ! ? || is someone tagged in message at start ?
            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                //execute commands, pass in context and and look for cmd prefix, inject dependancies
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                //error log
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition: 
                        await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetNoPerm());
                        break;
                    default:
                     //   await context.Channel.SendMessageAsync($"ummmmm, \"{result.ErrorReason}\" <@150764876258607105> fix me");
                       Console.WriteLine(result.ErrorReason);
                        break;
                }
                
                

            }
            
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
  
    }
}
