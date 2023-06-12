using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface IFinAccountRepository
{
    Task CreateAsync(FinAccountEntity finAccount);
    Task UpdateAsync(FinAccountEntity finAccount);
    Task<FinAccountEntity> GetByIdAsync(Guid finAccountId);
    Task<FinAccountEntity> GetByIdWithBalanceAsync(Guid finAccountId);
}
