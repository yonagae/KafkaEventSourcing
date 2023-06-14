using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class FinAccountAggregate : AggregateRoot
    {
        private string _owner;        
        private Decimal _balance;
        private Decimal _totalbalance;
        private List<string> _transferTypes = new();

        public FinAccountAggregate()
        {
        }

        public FinAccountAggregate(string owner)
        {
            RaiseEvent(new NewFinAccountEvent
            {
                Id = Guid.NewGuid(),
                Owner = owner,
                CreationDate = DateTime.Now,
            });
        }

        public void Apply(NewFinAccountEvent @event)
        {
            _id = @event.Id;
            _balance = 0;
            _totalbalance = 0;
        }

        public void Debit(Decimal amount, string transactionType)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException($"The value of {nameof(amount)} cannot be less than zero");
            }

            if(!_transferTypes.Contains(transactionType))
            {
                throw new InvalidOperationException($"The {nameof(transactionType)} doesn't exist, please provid a valid {nameof(transactionType)}");
            }

            RaiseEvent(new DebitFinAccountEvent
            {
                Id = _id,
                DebitAmount = amount,
                TimeStamp = DateTime.Now,
                TransactionType = transactionType,
            });
        }

        public void Apply(DebitFinAccountEvent @event)
        {
            _id = @event.Id;
            _balance -= @event.DebitAmount;
            _totalbalance -= @event.DebitAmount;
        }

        public void Credit(Decimal amount)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException($"The value of {nameof(amount)} cannot be less than zero");
            }
            RaiseEvent(new CreditFinAccountEvent
            {
                Id = _id,
                CreditAmount = amount,
                TimeStamp = DateTime.Now,
            });
        }

        public void Apply(CreditFinAccountEvent @event)
        {
            _id = @event.Id;
            _balance += @event.CreditAmount;
            _totalbalance += @event.CreditAmount;
        }

        public void CreateTransactionType(string transactionTypeName)
        {

            if (String.IsNullOrEmpty(transactionTypeName))
            {
                throw new InvalidOperationException($"The value of {nameof(transactionTypeName)} cannot be null or empty");
            }

            if (_transferTypes.Contains(transactionTypeName))
            {
                throw new InvalidOperationException($"The {nameof(transactionTypeName)} already exist, please provid a valid new {nameof(transactionTypeName)}");
            }

            RaiseEvent(new AddTransactionTypeEvent
            {
                Id = _id,
                TransactioTypeName = transactionTypeName
            });
        }

        public void Apply(AddTransactionTypeEvent @event)
        {
            _id = @event.Id;
            _transferTypes.Add(@event.TransactioTypeName);
        }
    }
}
