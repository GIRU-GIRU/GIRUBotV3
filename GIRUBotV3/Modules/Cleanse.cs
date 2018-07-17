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
        [Alias("purge", "prune")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        private async Task CleanChatAmount(int amount)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            var chnl = Context.Channel as ITextChannel;
            await chnl.DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync("wat ? 😃");
        }

        [Command("cleanse")]
        [Alias("purge", "prune")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        private async Task CleanChat()
        {
            int amount = 2;

            var messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var chnl = Context.Channel as ITextChannel;
            await chnl.DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync("wat ? 😃");
        }

        [Command("cleanse")]
        [Alias("purge", "prune")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        private async Task CleanChatUser(SocketGuildUser user)
        {
            var usersocket = user as SocketGuildUser;
            int amount = 300;
            var msgsCollection = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
                var result = from m in msgsCollection
                           where m.Author == user
                           select m.Id;

            var  chnl = Context.Channel as ITextChannel;
            int amountOfMessages = result.Count();
            await chnl.DeleteMessagesAsync(result);
            var cleanseUserEmbed = new EmbedBuilder();
            cleanseUserEmbed.WithTitle($"✅   cleansed {amountOfMessages} messages from {user.Username}");
            cleanseUserEmbed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, cleanseUserEmbed.Build());
        }
        [Command("cleanse")]
        [Alias("purge", "prune")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        private async Task CleanChatUserAmount(SocketGuildUser user, int amountToDelete)
        {
            var usersocket = user as SocketGuildUser;
            int amount = 900;
            var msgsCollection = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var result = from m in msgsCollection
                         where m.Author == user
                         select m.Id;

            var chnl = Context.Channel as ITextChannel;

            var totalToDelete = result.Take(amountToDelete);
            await chnl.DeleteMessagesAsync(totalToDelete);
            var cleanseUserEmbed = new EmbedBuilder();
            cleanseUserEmbed.WithTitle($"✅   cleansed {totalToDelete.Count()} messages from {user.Username}");
            cleanseUserEmbed.WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync("", false, cleanseUserEmbed.Build());
        }
    }
}




