using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace GIRUBotV3.Modules
{
    public static class ExceptionHandler
    {

        private static DiscordSocketClient _client;
        private static SocketGuild _meleeSlasherGuild;
        private static ITextChannel _logChannel;
        private static ITextChannel _mainChannel;

        public static bool InitContext(DiscordSocketClient client)
        {
            bool successful = false;

            _client = client;

            _meleeSlasherGuild = _client.GetGuild(Config.MeleeSlasherGuild);

            if (_client != null && _meleeSlasherGuild != null)
            {

                _logChannel = _meleeSlasherGuild.GetChannel(Config.AuditChannel) as ITextChannel;
                _mainChannel = _meleeSlasherGuild.GetChannel(Config.MeleeSlasherMainChannel) as ITextChannel;

                if (_logChannel != null && _mainChannel != null)
                {
                    successful = true;
                }
            }

            return successful;
        }


        public static async Task HandleExceptionQuietly(string ErrorClass, string ErrorMethod, Exception Exception)
        {
            try
            {
                await _logChannel.SendMessageAsync($"Error in {ErrorClass} - {ErrorMethod}: {GetInnermostException(Exception)}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Exception Handler: {ex.Message}");
                throw ex;
            }
        }

        public static async Task HandleExceptionPublically(string ErrorClass, string ErrorMethod, Exception Exception)
        {
            try
            {
                await _mainChannel.SendMessageAsync($"Error in {ErrorClass} - {ErrorMethod}: {GetInnermostException(Exception)}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Exception Handler: {ex.Message}");
                throw ex;
            }
        }

        public static string GetInnermostException(Exception ex)
        {
         
            while (ex.InnerException != null)
            {            
                    ex = ex.InnerException;             
            }

            return ex.Message;
        }
        public static string GetAsyncMethodName([CallerMemberName]string name = "unknown") => name;


    }
}





