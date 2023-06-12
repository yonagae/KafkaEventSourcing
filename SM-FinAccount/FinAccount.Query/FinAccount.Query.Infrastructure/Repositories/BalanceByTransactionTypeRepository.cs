using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class BalanceByTransactionTypeRepository : IBalanceByTransactionTypeRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public BalanceByTransactionTypeRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(BalanceByTransactionTypeEntity balanceByTransactionType)
        {
            try
            {
                using DatabaseContext context = _contextFactory.CreateDbContext();
                context.Balances.Add(balanceByTransactionType);

                _ = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var pppp = ex.Message;
            }
        }

        public async Task UpdateAsync(BalanceByTransactionTypeEntity balanceByTransactionType)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Balances.Update(balanceByTransactionType);

            _ = await context.SaveChangesAsync();
        }
    }
}