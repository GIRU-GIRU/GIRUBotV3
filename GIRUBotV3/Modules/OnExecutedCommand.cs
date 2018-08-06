﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Personality;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GIRUBotV3.Modules
{

    public class OnExecutedCommand
    {
       
        int? willBeLogged = null;
        CommandInfo _info;
        ICommandContext _context;
        IResult _result;
        ITextChannel adminlogchannel;

        private static DiscordSocketClient _client;
        public OnExecutedCommand(DiscordSocketClient client)
        {
            _client = client;
        }
        public async Task AdminLog(CommandInfo info, ICommandContext context, IResult result)
        {
             _info = info;
            _context = context;
            _result = result;

            switch (info.Module.Name)
            {
                case "Administration":
                    await AdministrationEmbed(_info, _context, _result);
                    break;
                case "Cleanse":
                    await CleanseEmbed(_info, _context, _result);
                    break;
                case "RaidProtect":
                    await RaidProtectEmbed(_info, _context, _result);
                    break;
                case "Pug":
                    await PugEmbed(_info, _context, _result);
                    break;
                default:
                    return;

            }

            async Task AdministrationEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                adminlogchannel = await context.Guild.GetChannelAsync(474729965359726593) as ITextChannel;
                var dateTimeStamp = context.Message.Timestamp.ToString("yyyy/MM/dd hh:mm");

                var embed = new EmbedBuilder();
                embed.WithTitle($"🗡 {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", dateTimeStamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(255, 102, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
            }
            async Task CleanseEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                adminlogchannel = await context.Guild.GetChannelAsync(474729965359726593) as ITextChannel;
                var dateTimeStamp = context.Message.Timestamp.ToString("yyyy/MM/dd hh:mm");

                var embed = new EmbedBuilder();
                embed.WithTitle($"🗑 {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", dateTimeStamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(255, 204, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;
            }
            async Task RaidProtectEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                adminlogchannel = await context.Guild.GetChannelAsync(474729965359726593) as ITextChannel;
                var dateTimeStamp = context.Message.Timestamp.ToString("yyyy/MM/dd hh:mm");

                var embed = new EmbedBuilder();
                embed.WithTitle($"⭕️ {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", dateTimeStamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(255, 0, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;
            }
            async Task PugEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                adminlogchannel = await context.Guild.GetChannelAsync(474729965359726593) as ITextChannel;
                var dateTimeStamp = context.Message.Timestamp.ToString("yyyy/MM/dd hh:mm");

                var embed = new EmbedBuilder();
                embed.WithTitle($"🎮 {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", dateTimeStamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(20, 255, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;
            }


        }
    }
}


