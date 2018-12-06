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
			var textChannel = channel as ITextChannel;
            var logChannel = await textChannel.Guild.GetChannelAsync(Config.DeletedMessageLog) as ITextChannel;
            var message = await msg.GetOrDownloadAsync();
            var dateTimeStamp = message.Timestamp.ToString("yyyy/MM/dd hh:mm");

            var embed = new EmbedBuilder();
            embed.WithTitle($"🗑 {message.Author.Username} deleted message at {dateTimeStamp}");
            embed.WithDescription(message.Content);
            if (message.Author.AvatarId != null)
            {
                embed.WithThumbnailUrl(message.Author.AvatarId);
            }
            embed.WithColor(new Color(255, 102, 0));
            await logChannel.SendMessageAsync("", false, embed.Build());
        }

    }
}


