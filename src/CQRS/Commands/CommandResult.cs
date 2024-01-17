namespace CQRS.Commands
{
    public class CommandResult
    {
        public bool IsSucces { get; protected set; }
        public string Message { get; protected set; }

        protected CommandResult() { }

        public static CommandResult Success()
        {
            return new CommandResult { IsSucces = true };
        }

        public static CommandResult Error(string message = null)
        {
            return new CommandResult { Message = message };
        }
    }

    public class CommandResult<TResponse> : CommandResult
    {
        public TResponse Response { get; init; }

        public static CommandResult<TResponse> Success(TResponse response)
        {
            return new CommandResult<TResponse>
            {
                IsSucces = true,
                Response = response
            };
        }

        public static CommandResult<TResponse> Error(string message = null)
        {
            return new CommandResult<TResponse> { Message = message };
        }
    }
}
