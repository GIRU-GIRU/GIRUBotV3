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
using System.Text.RegularExpressions;

namespace GIRUBotV3.Modules
{
    public class Memestore : ModuleBase<SocketCommandContext>
    {
        [Command("storememe")]
        [Alias("smeme", "addmeme")]
        private async Task StoreMeme([Remainder]string input)
        {
            var inputAsArray = input.Split(" ");
            var title = inputAsArray[0].ToString(); title = Regex.Replace(title, @"\t|\n|\r", "");
            var contentOfMessage = String.Join(" ", inputAsArray.Skip(1));
            var insult = await Insults.GetInsult();
            if (contentOfMessage.Length < 2)
            {
                await Context.Channel.SendMessageAsync($"what the fuck are you actually doing you fucking {insult}, why are you trying to make an empty meme ? r u legit fucking autist or what, fuckign dumb {insult} cunt");
                return;
            }
            if (title.Where(x => Char.IsDigit(x)).Any())
            {
                await Context.Channel.SendMessageAsync($"the meme title can't contain numbers or weird shit, sry bitch");
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
        [Alias("meme")]
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
        [Alias("meme")]
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
        [Command("rm")]
        [Alias("randommeme")]
        private async Task RandomCallMeme()
        {
            bool success = false;
            var insult = await Insults.GetInsult();
            var rnd = new Random();
            using (var db = new Memestorage())
            {
                while (success == false)
                {
                    try
                    {
                        var maxID = db.Memestore.Max(x => x.MemeId);
                        var meme = db.Memestore.Where(x => x.MemeId == rnd.Next(0, maxID)).FirstOrDefault();

                        if (!string.IsNullOrEmpty(meme.Content))
                        {
                            await Context.Channel.SendMessageAsync($"Meme#{meme.MemeId}: {meme.Content}");
                            success = true;
                        }
                    }
                    catch (Exception)
                    {
                    
                    }
                }
            }
        }


        [Command("memecreated")]
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

        [Command("memecreated")]
        [Alias("mc", "mcreated")]
        private async Task MemeCreatedDetails(int id)
        {
            using (var db = new Memestorage())
            {
                try
                {
                    var meme = db.Memestore.Where(x => x.MemeId == id).FirstOrDefault();

                    await Context.Channel.SendMessageAsync($"{meme.Title} was created by {meme.Author} on {meme.Date} at {meme.Time}. MemeID = {meme.MemeId}");
                    return;
                }
                catch (Exception)
                {
                    await Context.Channel.SendMessageAsync($"dosen't exist");
                    return;
                }
            }
        }

        [Command("editmeme")]
        [Alias("changememe")]
        private async Task EditMeme([Remainder]string input)
        {
            var inputAsArray = input.Split(" ");
            var title = inputAsArray[0].ToString();
            var contentOfMessage = String.Join(" ", inputAsArray.Skip(1));
            var insult = await Insults.GetInsult();
            if (contentOfMessage.Length < 2)
            {
                await Context.Channel.SendMessageAsync($"wat r u tryin to do");
                return;
            }
            if (title.Where(x => Char.IsDigit(x)).Any())
            {
                await Context.Channel.SendMessageAsync($"the meme title can't contain numbers or weird shit, sry bitch");
                return;
            }

            using (var db = new Memestorage())
            {
                if (db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).Any())
                {
                    var meme = db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).FirstOrDefault();
                    //is it a valid user ? (mod/original author)
                    if (Helpers.IsModeratorOrOwner(Context.Message.Author as SocketGuildUser) || meme.AuthorID == Context.Message.Author.Id)
                    {
                        meme.Content = contentOfMessage;
                        db.SaveChanges();

                        await Context.Channel.SendMessageAsync($"{title} was successfully updated");
                        return;
                    }
                }
                await Context.Channel.SendMessageAsync($"nah ur not allowed nice try tho lmfao");
            }
        }

        [Command("mymemestore")]
        private async Task MyMemeStore()
        {
            using (var db = new Memestorage())
            {
                try
                {
                    var memestoreArray = db.Memestore.Where(x => x.AuthorID == Context.Message.Author.Id).ToArray();
                    var listOfMemes = new List<string>();
                    foreach (var item in memestoreArray)
                    {
                        listOfMemes.Add(item.Title);
                    }
                    var outputList = String.Join(", ", listOfMemes.ToArray());
                    var outputMessage = $"{Context.Message.Author.Username} is the owner of the memes: {outputList}";
                    if (outputMessage.Length >= 1999)
                    {
                        await Context.Channel.SendMessageAsync("well u made too many memes so im not gonna tell u, sry ");
                        return;
                    }
                    await Context.Channel.SendMessageAsync(outputMessage);
                    return;
                }
                catch (Exception)
                {
                    await Context.Channel.SendMessageAsync($"u not got any memes that u own lmfao");
                    return;
                }
            }
        }

    }

}