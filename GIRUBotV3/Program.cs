using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using GIRUBotV3.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using FaceApp;
using System.Net.Http;
using TwitchLib.Api.Services;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api;

namespace GIRUBotV3
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            var bot = program.RunBotAsync();
            bot.Wait();   
        }


        public DiscordSocketClient _client;
        private CommandService _commands;
        private OnMessage _onMessage;
        private OnExecutedCommand _onExecutedCommand;
        private IServiceProvider _services;
        public static TwitchAPI api;
        private TwitchBot _twitchBot;
        private LiveStreamMonitor _liveStreamMonitor;

        private FaceAppClient _FaceAppClient;
        public async Task RunBotAsync()
        {
            string botToken = Config.BotToken;
            api = new TwitchAPI();
            api.Settings.ClientId = Config.TwitchClientId;
            api.Settings.AccessToken = Config.TwitchAccessToken;
            _liveStreamMonitor = new LiveStreamMonitor(api, 60, true, false);

            var _HttpClient = new HttpClient();

            _twitchBot = new TwitchBot(api);
            
            _FaceAppClient = new FaceAppClient(_HttpClient);
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _onMessage = new OnMessage(_client, _FaceAppClient);
            _onExecutedCommand = new OnExecutedCommand(_client);
            
            _services = new ServiceCollection()
                 .AddSingleton(_commands)
                 .AddSingleton(_FaceAppClient)
                 .AddSingleton(_twitchBot)
                 .BuildServiceProvider();

            _client.MessageUpdated += _onMessage.UpdatedMessageContainsAsync;         
            _client.UserJoined += UserHelp.UserJoined;
         
           
            _client.MessageReceived += _onMessage.MessageContainsAsync;
            _liveStreamMonitor.OnStreamOnline += _twitchBot.NotifyMainOnStreamStart;

             _commands.CommandExecuted += _onExecutedCommand.AdminLog;
            //_client.UserVoiceStateUpdated += _onExecutedCommand.AdminLogVCMovement;
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
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition: 
                        await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetNoPerm());
                        break;
                    case CommandError.ParseFailed:
                        await context.Channel.SendMessageAsync(await ErrorReturnStrings.GetParseFailed());
                        break;
                    default:
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
