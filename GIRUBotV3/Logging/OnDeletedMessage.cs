using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{

    public class OnDeletedMessage
    {

        public async Task DeletedMessageStore(Cacheable<IMessage, ulong> msg, ISocketMessageChannel channel)
        {
            if (msg.Value.Content.Substring(0, 3) == "+s ") return;
           
			var textChannel = channel as ITextChannel;
            var logChannel = await textChannel.Guild.GetChannelAsync(Config.DeletedMessageLog) as ITextChannel;
            var message = await msg.GetOrDownloadAsync();
           

            var embed = new EmbedBuilder();
            embed.WithTitle($"🗑 {message.Author.Username}#{message.Author.Discriminator} deleted message at in {message.Channel.Name}. UserID = {message.Author.Id}");
            embed.WithDescription(message.Content);
            var test = message.Author.AvatarId;
            var test2 = message.Author.GetAvatarUrl();

            //if (!string.IsNullOrEmpty(message.Author.GetAvatarUrl()))
            //{
            //    embed.WithThumbnailUrl(message.Author.AvatarId);
            //}
            embed.WithColor(new Color(255, 102, 0));
            await logChannel.SendMessageAsync("", false, embed.Build());
        }

    }
}


