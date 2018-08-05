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

    public class OnExecutedCommand
    {
        private List<string> administrativeModuleNames = new List<string>();
        int willBeLogged = 0;
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
            info = _info;
            context = _context;
            result = _result;

            administrativeModuleNames.AddMany(
            "Administration",
            "Cleanse",
            "RaidProtect",
            "Pug"
            );

            for (int i = 0; i < administrativeModuleNames.Count; i++)
            {
                if (administrativeModuleNames[i] == info.Module.Name)
                {
                    willBeLogged = i;
                }
            }

            switch (willBeLogged)
            {
                case 1:
                    await AdministrationEmbed(_info, _context, _result);
                    break;
                case 2:
                    await CleanseEmbed(_info, _context, _result);
                    break;
                case 3:
                    await RaidProtectEmbed(_info, _context, _result);
                    break;
                case 4:
                    await PugEmbed(_info, _context, _result);
                    break;
                default:
                    return;

            }
            
             adminlogchannel = await context.Guild.GetChannelAsync(474729965359726593) as ITextChannel;

            async Task AdministrationEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle($"🗡 {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", context.Message.Timestamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(255, 102, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;


            }
            async Task CleanseEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle($"🗑 {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", context.Message.Timestamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(255, 204, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;


            }
            async Task RaidProtectEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle($"⭕️ {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", context.Message.Timestamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(255, 0, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;

            }
            async Task PugEmbed(CommandInfo _info, ICommandContext _context, IResult _result)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle($"🎮 {context.Message.Author.Username} used the \"{info.Module.Name}\" module");
                embed.AddField("command: ", context.Message.Content, false);
                embed.AddField("at: ", context.Message.Timestamp, false);
                embed.AddField("in: ", context.Message.Channel.Name, false);
                embed.WithColor(new Color(20, 255, 0));
                await adminlogchannel.SendMessageAsync("", false, embed.Build());
                return;
            }


        }
    }
}


