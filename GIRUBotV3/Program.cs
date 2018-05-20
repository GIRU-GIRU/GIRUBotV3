using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using GIRUBotV3.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace GIRUBotV3
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            var bot = program.RunBotAsync();

            // Wait for bot to exit
            bot.Wait();
        }
   

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public async Task RunBotAsync()
        {
            string botToken = "NDQwMjE1NDgzODU4NDE5NzIy.Dcx42Q.YbIWye4Q-5O59hrNZRfVm5Ajq2Y";
            //ConfigurationManager.AppSettings["creds"];

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

        private async Task MessageContainsAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;
            var context = new SocketCommandContext(_client, message);

            if (message.Content.Contains("😃"))
            {
                var r = new Random();
                int chance = r.Next(1, 15);

                if (chance <= 15)
                {
                    await context.Channel.SendMessageAsync("😃");
                }                     
            }
            if (message.Content.Contains(" help "))
            {
                var r = new Random();
                int chance = r.Next(1, 15);

                if (chance <= 15)
                {
                    await context.Channel.SendMessageAsync("stop crying for help");
                }

               Regex nounTest = new Regex("(?s)(.(.*)test)");
                var nountTestFound = nounTest.Match(message.Content);
                if (nountTestFound.Success)
                {
                   
                    Task.Run(NounTest)
                }
              
               
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
