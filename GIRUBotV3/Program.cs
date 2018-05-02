using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Reflection;
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
            string botToken = "";
            //ConfigurationManager.AppSettings["creds"];

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                 .AddSingleton(_client)
                 .AddSingleton(_commands)
                 .BuildServiceProvider();


            _client.Log += Log;
            //register modules and login bot with auth credentials
            await RegisterCommandAsync();
            await _client.LoginAsync(TokenType.Bot, botToken);
            //starting client and continue forever
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            
        }

       


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
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }

            }
        }
    }
}
