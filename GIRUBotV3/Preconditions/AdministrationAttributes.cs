using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GIRUBotV3.Preconditions
{

    //public class RequireRoleAttribute : PreconditionAttribute
    //{
    //    private readonly ulong _requiredRoleId;
    //    public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    //    {

    //        var testVar = _requiredRoleId;

    //        await CheckPermissionsAsync(context, command, services, _requiredRoleID);


    //    }
    //}
    public class CommandFailure : RuntimeResult
    {
        public CommandFailure(CommandError? error, string reason) : base(error, reason)
        {
        }
        public static CommandFailure FromError(string reason) =>
            new CommandFailure(CommandError.Unsuccessful, reason);
        public static CommandFailure FromSuccess(string reason = null) =>
            new CommandFailure(null, reason);
    }
}













    

