using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using GIRUBotV3.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            bot.Wait();
            var runTwitch = TwitchIntegration.TwitchMainAsync();        
        }

        public DiscordSocketClient _client;
        private CommandService _commands;
        private OnMessage _onMessage;
        private IServiceProvider _services;
        public async Task RunBotAsync()
        {
            string botToken = Config.BotToken;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _onMessage = new OnMessage(_client);

            _services = new ServiceCollection()
                 .AddSingleton(_commands)
                 .BuildServiceProvider();

            _client.MessageUpdated += _onMessage.UpdatedMessageContainsAsync;         
            _client.UserJoined += UserHelp.UserJoined;
           
            _client.MessageReceived += _onMessage.MessageContainsAsync;
            
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

        //Handle Commands
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            //ignore ourselves, check for null
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;

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
                    case CommandError.ParseFailed:
                        await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetParseFailed());
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
