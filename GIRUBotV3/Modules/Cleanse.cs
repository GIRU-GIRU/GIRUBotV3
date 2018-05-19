using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Linq;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{

    public class Cleanse : ModuleBase<SocketCommandContext>
    {
        [Command("cleanse")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task CleanChat(int amount)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            var chnl = Context.Channel as ITextChannel;
            await chnl.DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync("wat ? 😃");
        }

        [Command("cleanse")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.Administrator)]
        private async Task CleanChat()
        {
            int amount = 2;

            var messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var chnl = Context.Channel as ITextChannel;
            await chnl.DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync("wat ? 😃");
        }

        [Command("cleanse")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.Administrator)]
        private async Task CleanChat(IGuildUser User)
        {
            // var messages = await Context.Channel.User.GetMessagesAsync(amount).FlattenAsync();
            int amount = 99;
            // var user = await Context.Channel.GetUserAsync(User.Id);
            var msgsCollection = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var chnl = Context.Channel as ITextChannel;
            foreach (var item in msgsCollection)
            {
                var result = from u in msgsCollection
                             where u.Author == User
                             select u.Id;
                await chnl.DeleteMessagesAsync(result);
            }
            await Context.Channel.SendMessageAsync($"stfu {User.Mention}");
        }
        [Command("cleanse")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.Administrator)]
        private async Task CleanChat(IGuildUser User, int messagesToDelete)
        {
            int amount = 99;// discord api limit for downloads
            var msgsCollection = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var chnl = Context.Channel as ITextChannel;

            int loopCount = 0;
            int maxAPICalls = 99;

            while (loopCount <= messagesToDelete || loopCount <= maxAPICalls)
            {              
                    var result = from u in msgsCollection
                                 where u.Author == User
                                 select u.Id;

                    var resultSingle = result.FirstOrDefault();

                //// delete it
                    var msgToDelete = await chnl.GetMessageAsync(resultSingle) as IEnumerable<IMessage>;
                    await chnl.DeleteMessagesAsync(msgToDelete);
                ///
                    loopCount++;
            }
            await Context.Channel.SendMessageAsync($"stfu {User.Mention}");
        }

    }
}




