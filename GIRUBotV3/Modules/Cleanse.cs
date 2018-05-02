using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{

    public class Cleanse : ModuleBase<SocketCommandContext>
    {
        [Command("cleanse")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Clean(int amount)
        {
            if (!Helpers.IsRole("test", (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(await Insults.GetNoPerm());
                return;
            } 
            var deleteMessages = new List<IMessage>(amount);
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync("wat ? 😃");
        }


        [Command("cleanse")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Clean()
        {
            if (!Helpers.IsRole("test", (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(await Insults.GetNoPerm());
                return;
            }
            int amount = 2;
            var deleteMessages = new List<IMessage>(amount);
            var messages = await Context.Channel.GetMessagesAsync(amount).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync("wat ? 😃");
        }
    }
        //    [Command("cleanse")]
        //    public async Task Cleanse(int amount)
        //    {
        //        var messages = new List<IMessage>();

        //        messages.Add(await Context.Channel.GetMessagesAsync(amount + 1).Flatten());
        //        Context.Channel.DeleteMessagesAsync(amount);
        //    }

        //    [Command("cleanse")]
        //    public async Task Cleanse()
        //    {

        //    }


    }
