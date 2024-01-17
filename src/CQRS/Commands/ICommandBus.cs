namespace CQRS.Commands
{
    public interface ICommandBus
    {
        Task<CommandResult<TResponse>> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    }
}
