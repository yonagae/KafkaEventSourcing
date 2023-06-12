using Post.Common.Events;

namespace Post.Query.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(NewFinAccountEvent @event);
        Task On(DebitFinAccountEvent @event);
        Task On(CreditFinAccountEvent @event);
        Task On(AddTransactionTypeEvent @event);


    }
}