using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IFinAccountRepository _finAccountRepository;
        private readonly IBalanceByTransactionTypeRepository _balanceByTransactionTypeRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public EventHandler(
            IFinAccountRepository finAccountRepository, 
            IBalanceByTransactionTypeRepository balanceByTransactionTypeRepository, 
            ITransactionTypeRepository transactionTypeRepository)
        {
                _finAccountRepository = finAccountRepository;
            _balanceByTransactionTypeRepository = balanceByTransactionTypeRepository;
            _transactionTypeRepository = transactionTypeRepository;
        }

           public async Task On(NewFinAccountEvent @event)
        {
            var post = new FinAccountEntity
            {
                Id = @event.Id,
                Owner = @event.Owner,
                TotalBalance = 0,
                Balances = null
            };

            await _finAccountRepository.CreateAsync(post);
        }

        public async Task On(DebitFinAccountEvent @event)
        {
            var finAccount = await _finAccountRepository.GetByIdWithBalanceAsync(@event.Id);
            if (finAccount == null) return;
            finAccount.TotalBalance -= @event.DebitAmount;

            var transactionTypeBalance = finAccount.Balances.FirstOrDefault(x => x.TransactionType.Name == @event.TransactionType);

            if(transactionTypeBalance == null)
            {
                var transactionType = await _transactionTypeRepository.GetByNameAsync(finAccount.Id, @event.TransactionType);

                await _balanceByTransactionTypeRepository.CreateAsync(new BalanceByTransactionTypeEntity
                {
                    Balance = -@event.DebitAmount,
                    FinAccountId = finAccount.Id,
                    TransactionTypeId = transactionType.Id,
                    //FinAccount = finAccount,
                    //TransactionType = transactionType
                });
            }
            else
            {
                transactionTypeBalance.Balance -= @event.DebitAmount;
                await _balanceByTransactionTypeRepository.UpdateAsync(transactionTypeBalance);
            }

            await _finAccountRepository.UpdateAsync(finAccount);
        }

        public async Task On(CreditFinAccountEvent @event)
        {
            var finAccount = await _finAccountRepository.GetByIdWithBalanceAsync(@event.Id);
            if (finAccount == null) return;
            finAccount.TotalBalance += @event.CreditAmount;

            await _finAccountRepository.UpdateAsync(finAccount);
        }

        public async Task On(AddTransactionTypeEvent @event)
        {
            var transactionType = new TransactionTypeEntity
            {
                Id = Guid.NewGuid(),
                FinAccountId = @event.Id,
                Name = @event.TransactioTypeName
            };

            await _transactionTypeRepository.CreateAsync(transactionType);
        }
    }
}