using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class FinAccountRepository : IFinAccountRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public FinAccountRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreateAsync(FinAccountEntity finAccount)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.FinAccounts.Add(finAccount);

            _ = await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinAccountEntity finAccount)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.FinAccounts.Update(finAccount);

            _ = await context.SaveChangesAsync();
        }

        public async Task<FinAccountEntity> GetByIdAsync(Guid finAccountId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.FinAccounts
                    .FirstOrDefaultAsync(x => x.Id == finAccountId);
        }

        public async Task<FinAccountEntity> GetByIdWithBalanceAsync(Guid finAccountId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.FinAccounts
                    .Include(i => i.Balances)
                    .ThenInclude(b => b.TransactionType)
                    .FirstOrDefaultAsync(x => x.Id == finAccountId);
        }
    }
}