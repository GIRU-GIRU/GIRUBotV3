using Discord.Commands;
using Discord.WebSocket;
using Discord;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;

namespace GIRUBotV3.Modules
{
    public static class WordFilter
    {

        public static async Task<bool> CheckForNaughtyWords(string messageContent)
        {
            try
            {
                bool messageContainsBadWord = false;

                foreach (var badWord in naughtyWordsArray)
                {
                    if (messageContent.ToLower().Contains(badWord.ToLower()))
                    {
                        messageContainsBadWord = true;
                        break;
                    }
                }

                return messageContainsBadWord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static async Task PunishNaughtyWord(SocketCommandContext context)
        {
            try
            {
                await context.Message.DeleteAsync();
            }
            catch (Exception ex)
            {
                await context.Channel.SendMessageAsync($"unable to delete message.. {ex.Message}");
                throw ex;
            }
        }

        static string[] naughtyWordsArray = new string[]
            {
            "nigger",
            "nig",
            "fag",
            "faggot",
            "chink",
            };
    }
}