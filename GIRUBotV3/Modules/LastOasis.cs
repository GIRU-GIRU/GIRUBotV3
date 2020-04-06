using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;
using GIRUBotV3.Models;

namespace GIRUBotV3.Modules
{
    public class LastOasis : ModuleBase<SocketCommandContext>
    {

        [Command("emergency")]
        private async Task LastOasisEmergency()
        {
            try
            {
    
                await Context.Channel.SendMessageAsync("Write a reason!");

            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }

        [Command("emergency")]
        private async Task LastOasisEmergency([Remainder]string inputMessage)
        {
            try
            {
                if (Context.Message.Channel.Id == 693097204675510333)
                {
                    IRole LastOasisRole = Helpers.ReturnRole(Context.Guild, "🌴 Last Oasis");
                    await LastOasisRole.ModifyAsync(x => x.Mentionable = true);

                    var embed = new EmbedBuilder();
                    embed.WithTitle("EMERGENCY IN LAST OASIS");
                    embed.WithDescription($"Called by {Context.Message.Author}\n{inputMessage}");
                    embed.ThumbnailUrl = "https://cdn.discordapp.com/attachments/693097204675510333/696842499729260575/zNID4uCK_400x400.png";
                    embed.WithColor(new Color(0, 204, 255));
                    await Context.Channel.SendMessageAsync(LastOasisRole.Mention, false, embed.Build());

                    await LastOasisRole.ModifyAsync(x => x.Mentionable = false);
                }                
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }
    }
}
