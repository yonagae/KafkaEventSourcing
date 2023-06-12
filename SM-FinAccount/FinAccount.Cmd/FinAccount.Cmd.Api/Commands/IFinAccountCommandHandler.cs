using Post.Common.Events;

namespace Post.Cmd.Api.Commands
{
    public interface IFinAccountCommandHandler
    {
        Task HandleAsync(NewFinAccountCommand command);
        Task HandleAsync(DebitFinAccountCommand command);
        Task HandleAsync(CreditFinAccountCommand command);
        Task HandleAsync(AddTransactionTypeCommand command);
    }
}