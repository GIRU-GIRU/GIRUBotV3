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
using GIRUBotV3.Preconditions;

namespace GIRUBotV3.Modules
{

    public class Memestore : ModuleBase<SocketCommandContext>
    {
        [MemestoreToggle]
        [Command("storememe")]
        [Alias("sm", "smeme", "addmeme")]
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
                if (db.Memestore.Where(x => x.AuthorID == Context.Message.Author.Id).Count() >= 25)
                {
                    await Context.Channel.SendMessageAsync($"fucking greedy fuck {insult} bastard u cannot make over 25 memes");
                    return;
                }

                if (db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).Any())
                {
                    await Context.Channel.SendMessageAsync($"that alrdy exists u {insult}");
                    return;
                }

                await db.Memestore.AddAsync(new MemeStoreModel
                {
                    Author = Context.Message.Author.Username,
                    AuthorID = Context.Message.Author.Id,
                    Content = contentOfMessage,
                    Title = title,
                    Date = DateTime.Now.ToShortDateString(),
                    Time = DateTime.Now.ToShortTimeString()
                });

                await db.SaveChangesAsync();
                await Context.Channel.SendMessageAsync($"{title} was successfully created!");
            }
        }

        [MemestoreToggle]
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
                    await db.SaveChangesAsync();
                    await Context.Channel.SendMessageAsync($"{title} was deleted successfully from the DB");
                    return;
                }
                await Context.Channel.SendMessageAsync($"only the original author or moderator can delete this");
                return;
            }
        }

        [MemestoreToggle]
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
                    meme.MemeUses = meme.MemeUses + 1;
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        [MemestoreToggle]
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
                    meme.MemeUses += meme.MemeUses++;
                    await db.SaveChangesAsync();
                    await Context.Channel.SendMessageAsync($"{meme.Content}");
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        [MemestoreToggle]
        [Command("rm")]
        [Alias("randommeme")]
        private async Task RandomCallMeme()
        {
            bool success = false;
            var insult = await Insults.GetInsult();
            var rnd = new Random();
            using (var db = new Memestorage())
            {
                try
                {
                    while (success == false)
                    {
                        var maxID = db.Memestore.Max(x => x.MemeId);
                        var meme = db.Memestore.Where(x => x.MemeId == rnd.Next(0, maxID)).FirstOrDefault();
                        
                        if (meme != null && !string.IsNullOrEmpty(meme.Content))
                        {
                            await Context.Channel.SendMessageAsync($"Meme#{meme.MemeId}: {meme.Content}");
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync("smth went wrong ... " + ex.Message);
                }
            }
        }

        [MemestoreToggle]
        [Command("memecreated")]
        [Alias("mc", "mcreated")]
        private async Task MemeCreatedDetails(string title)
        {
            using (var db = new Memestorage())
            {
                try
                {
                    var meme = db.Memestore.Where(x => x.Title.ToLower() == title.ToLower()).FirstOrDefault();

                    var embed = new EmbedBuilder();
                    embed.WithTitle($"{title}#{meme.MemeId}");
                    embed.AddField("Creator: ", meme.Author, true);
                    embed.AddField("On: ", meme.Date + " " + meme.Time, true);
                    embed.AddField("Uses: ", meme.MemeUses.ToString(), true);
                    embed.WithFooter($"{meme.Content}");
                    embed.WithColor(new Color(104, 66, 244));
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    return;
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync($"that doesn't exist");
                    return;
                }
            }
        }

        [MemestoreToggle]
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
                    await Context.Channel.SendMessageAsync($"doesn't exist");
                    return;
                }
            }
        }

        [MemestoreToggle]
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
                        await db.SaveChangesAsync();

                        await Context.Channel.SendMessageAsync($"{title} was successfully updated");
                        return;
                    }
                }
                await Context.Channel.SendMessageAsync($"nah ur not allowed nice try tho lmfao {insult}");
            }
        }

        [MemestoreToggle]
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

        [MemestoreToggle]
        [Command("topmemes")]
        private async Task MemeStoreMostPopular()
        {
            using (var db = new Memestorage())
            {
                try
                {
                    var collectionTopMemes = db.Memestore.OrderByDescending(x => x.MemeUses).Take(10);
                    List<string> topMemeAuthors = new List<string>();
                    topMemeAuthors.AddRange(collectionTopMemes.Select(x => x.Author));

                    List<string> topMemetitles = new List<string>();
                    topMemetitles.AddRange(collectionTopMemes.Select(x => x.MemeId + "#" + x.Title.Substring(0, 15)));

                    List<int> topMemeUses = new List<int>();
                    topMemeUses.AddRange(collectionTopMemes.Select(x => x.MemeUses));

                    var embed = new EmbedBuilder();
                    embed.WithTitle($"Top memes in Melee Slasher");
                    embed.AddField("Number and Name: ", string.Join("\n", topMemetitles), true);
                    embed.AddField("Author: ", string.Join("\n", topMemeAuthors), true);
                    embed.AddField("Uses: ", string.Join("\n", topMemeUses), true);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync("problem... " + ex.Message);
                }
            }
        }

        [MemestoreToggle]
        [Command("memecount")]
        private async Task MemeStoreCount()
        {
            using (var db = new Memestorage())
            {
                try
                {
                    var memestoreCount = db.Memestore.Where(x => x.Content != null).Count();
                    await Context.Channel.SendMessageAsync($"there are currently {memestoreCount} active memes in the database.");
                    return;
                }
                catch (Exception)
                {
                    await Context.Channel.SendMessageAsync($"waet, somethin broken");
                    return;
                }
            }
        }

    }


}