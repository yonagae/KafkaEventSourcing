using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using Post.Common.Events;

namespace Post.Cmd.Api.Commands
{
    public class FinAccountCommandHandler : IFinAccountCommandHandler
    {
        private readonly IEventSourcingHandler<FinAccountAggregate> _eventSourcingHandler;

        public FinAccountCommandHandler(IEventSourcingHandler<FinAccountAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewFinAccountCommand command)
        {
            var aggregate = new FinAccountAggregate(command.Owner);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DebitFinAccountCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.Debit(command.Amount, command.TransactionType);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(CreditFinAccountCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.Credit(command.Amount);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddTransactionTypeCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.CreateTransactionType(command.Name);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}