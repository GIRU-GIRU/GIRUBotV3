using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using GIRUBotV3.Personality;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http;
using GIRUBotV3.Logging;
using GIRUBotV3.Models;
using Newtonsoft.Json;
using System.IO;

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
        private IServiceProvider _services;
        private OnMessage _onMessage;
        private BotInitialization _botInitialization;


        public async Task RunBotAsync()
        {
            try
            {
                await InitializeConfig();

                var _HttpClient = new HttpClient();

                DiscordSocketConfig botConfig = new DiscordSocketConfig()
                {
                  MessageCacheSize = 5000,
                  AlwaysDownloadUsers = true,
                 
                };


                _client = new DiscordSocketClient(botConfig);

                CommandServiceConfig CommandServiceConfig = new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async

                };
                _commands = new CommandService(CommandServiceConfig);

                _botInitialization = new BotInitialization(_client);
                _onMessage = new OnMessage(_client);

                _services = new ServiceCollection()
                     .AddSingleton(_commands)
                     .AddSingleton(_client)
                     .BuildServiceProvider();

                _client.MessageUpdated += _onMessage.UpdatedMessageContainsAsync;
                _client.Ready += BotInitialization.GIRUBotInitializationTasks;

                _client.Log += Log;

                await RegisterCommandAsync();
                await _client.LoginAsync(TokenType.Bot, Global.Config.BotToken);
                await _client.StartAsync();


                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running bot..\n------- EXCEPTION -------\n {ex.Message}\n------- \\EXCEPTION -------");
            }
        }


        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            try
            {
                var message = arg as SocketUserMessage;
                if (message.Author.IsBot) return;

                _ = Task.Run(() => _onMessage.MessageContainsAsync(arg));
                int argPos = 0;
                if (message.HasStringPrefix(Global.Config.CommandPrefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    var context = new SocketCommandContext(_client, message);

                    if (await WordFilter.CheckForNaughtyWords(message.Content))
                    {
                        await WordFilter.PunishNaughtyWord(context);
                        return;
                    }
                    if (await BlacklistUser.CheckBlacklist(context.Message.Author)) return;

                    var result = await _commands.ExecuteAsync(context, argPos, _services);

                    if (!result.IsSuccess) await Logger.LogToConsole(result, context);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task InitializeConfig()
        {
            using (StreamReader file = File.OpenText(@"config.json"))
            {
                JsonSerializer serializer = new JsonSerializer();

                Global.Config = (Config)serializer.Deserialize(file, typeof(Config));

                return Task.CompletedTask;
            }
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

    }
}
