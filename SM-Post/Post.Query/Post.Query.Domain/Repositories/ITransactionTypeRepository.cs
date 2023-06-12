using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ITransactionTypeRepository
{
    Task CreateAsync(TransactionTypeEntity transactionType);
    Task<TransactionTypeEntity> GetByNameAsync(Guid finAccountId, string name);
}
