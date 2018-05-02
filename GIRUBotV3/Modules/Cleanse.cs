//using Discord;
//using Discord.Commands;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Discord.WebSocket;
//using GIRUBotV3.Personality;
//using System.Threading.Tasks;

//namespace GIRUBotV3.Modules
//{
//    public class Cleanse : ModuleBase<SocketCommandContext>
//    {
//        [Command("cleanse")]
//        public async Task Cleanse(int amount)
//        {
//            var messages = new List<IMessage>();
                
//              messages.Add(await Context.Channel.GetMessagesAsync(amount + 1).Flatten());
//            Context.Channel.DeleteMessagesAsync(amount);
//        }

//        [Command("cleanse")]
//        public async Task Cleanse()
//        {
               
//        }

        
//    }
//}
