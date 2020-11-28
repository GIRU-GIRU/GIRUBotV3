using Discord.Commands;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{
    class InviteLinkPreventation
    {
        public async Task DeleteInviteLinkWarn(SocketCommandContext context)
        {
            try
            {
                var insult = await Insults.GetInsult();

                await context.Message.DeleteAsync();
                await context.Channel.SendMessageAsync($"{context.User.Mention}, don't post invite links {insult}");
            }
            catch (Exception ex)
            {
                await ExceptionHandler.HandleExceptionQuietly(GetType().FullName, ExceptionHandler.GetAsyncMethodName(), ex);
            }
        }
    }
}
