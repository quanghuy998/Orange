using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace CQRS.Commands
{
    internal class CommandBus : ICommandBus
    {
        private readonly IMediator mediator;

        public CommandBus(IServiceProvider serviceProvider)
        {
            mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        public Task<CommandResult<TResponse>> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            return mediator.Send(command, cancellationToken);
        }
    }
}
