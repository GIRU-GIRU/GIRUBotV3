using Discord.Commands;
using Discord.WebSocket;
using GIRUBotV3.Modules;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace GIRUBotV3.AdministrativeAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class IsModerator : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {

            var user = context.Message.Author as SocketGuildUser;

            bool isModerator = Helpers.IsRole("Moderator", user);

            if (isModerator || user.Id == Global.Config.OwnerID)
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError("Unauthorized");
        }   
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class IsModeratorOrVK : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {

            var user = context.Message.Author as SocketGuildUser;

            bool isModerator = Helpers.IsRole("Moderator", user);
            bool isVK = Helpers.IsRole("VK", user);

            if (isModerator || isVK || user.Id == Global.Config.OwnerID)
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError("Unauthorized");
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class IsModeratorOrVKB : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {

            var user = context.Message.Author as SocketGuildUser;

            bool isModerator = Helpers.IsRole("Moderator", user);
            bool isVK = Helpers.IsRole("VK", user);
            bool isVKB = Helpers.IsRole("VK-B", user);

            if (isModerator || isVK || isVKB || user.Id == Global.Config.OwnerID)
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError("Unauthorized");
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class IsLastOasis : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {

            var user = context.Message.Author as SocketGuildUser;

            bool IsLastOasis = Helpers.IsRole("🌴 Last Oasis", user);
           

            if (IsLastOasis || user.Id == Global.Config.OwnerID)
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError("Unauthorized");
        }
    }
}
