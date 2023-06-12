using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface IBalanceByTransactionTypeRepository
{
    Task CreateAsync(BalanceByTransactionTypeEntity balanceByTransactionType);
    Task UpdateAsync(BalanceByTransactionTypeEntity balanceByTransactionType);
}
