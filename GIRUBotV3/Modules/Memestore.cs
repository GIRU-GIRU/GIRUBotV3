using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using GIRUBotV3.Data;
using System.Threading.Tasks;
using System.Linq;

namespace GIRUBotV3.Modules
{
    public class Memestore : ModuleBase<SocketCommandContext>
    {
        [Command("storememe")]
        [Alias("smeme")]
        private async Task StoreMeme([Remainder]string input)
        {
            var inputAsArray = input.Split(" ");
            var title = inputAsArray[0].ToString();
            var contentOfMessage = String.Join(" ", inputAsArray.Skip(1));
            var insult = await Insults.GetInsult();
            if (contentOfMessage.Length < 2)
            {
                await Context.Channel.SendMessageAsync($"what the fuck are you actually doing you fucking {insult}, why are you trying to make an empty meme ? r u legit fucking autist or what, fuckign dumb {insult} cunt");
                return;
            }
            if (title.Where(x => Char.IsDigit(x)).Any())
            {
                await Context.Channel.SendMessageAsync($"the meme title can't contain numbers, sry bitch");
                return;
            }
            using (var db = new Memestorage())
            {
                if (db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).Any())
                {
                    await Context.Channel.SendMessageAsync($"that alrdy exists u {insult}");
                    return;
                }

                db.Memestore.Add(new MemeStoreModel
                {
                    Author = Context.Message.Author.Username,
                    AuthorID = Context.Message.Author.Id,
                    Content = contentOfMessage,
                    Title = title,
                    Date = DateTime.Now.ToShortDateString(),
                    Time = DateTime.Now.ToShortTimeString()
                });

                db.SaveChanges();
                await Context.Channel.SendMessageAsync($"{title} was successfully created!");
            }
        }

        [Command("delmeme")]
        [Alias("deletememe", "dmeme")]
        private async Task DeleteMeme([Remainder]string input)
        {
            var inputAsArray = input.Split(" ");
            var title = inputAsArray[0].ToString();
            var insult = await Insults.GetInsult();

            if (inputAsArray.Count() > 1)
            {
                await Context.Channel.SendMessageAsync("wtf is that supposed to be?");
                return;
            }

            using (var db = new Memestorage())
            {
                if (!db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).Any())
                {
                    await Context.Channel.SendMessageAsync($"there's no {title} {insult}");
                    return;
                }
                var rowToRemove = db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).SingleOrDefault();
                //needs to be original author or moderator
                if (Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser) || rowToRemove.AuthorID == Context.Message.Author.Id)

                {
                    db.Memestore.Remove(rowToRemove);
                    db.SaveChanges();
                    await Context.Channel.SendMessageAsync($"{title} was deleted successfully from the DB");
                    return;
                }
                await Context.Channel.SendMessageAsync($"only the original author or moderator can delete this");
                return;
            }
        }

        [Command("m")]
        private async Task CallMeme(string input)
        {
            var insult = await Insults.GetInsult();
            using (var db = new Memestorage())
            {
                try
                {
                    var meme = db.Memestore.Where(x => x.Title.ToLower() == input.ToLower()).FirstOrDefault();
                    await Context.Channel.SendMessageAsync($"{meme.Content}");
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        [Command("m")]
        private async Task CallMemeID(int id)
        {
            var insult = await Insults.GetInsult();
            using (var db = new Memestorage())
            {
                try
                {
                    var meme = db.Memestore.Where(x => x.MemeId == id).FirstOrDefault();
                    await Context.Channel.SendMessageAsync($"{meme.Content}");
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        [Command("m created")]
        [Alias("mc", "mcreated")]
        private async Task MemeCreatedDetails(string title)
        {
            using (var db = new Memestorage())
            {
                try
                {
                    var meme = db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).FirstOrDefault();

                    await Context.Channel.SendMessageAsync($"{title} was created by {meme.Author} on {meme.Date} at {meme.Time}. MemeID = {meme.MemeId}");
                    return;
                }
                catch (Exception)
                {
                    await Context.Channel.SendMessageAsync($"dosen't exist");
                    return;
                }

            }
        }
    }

}