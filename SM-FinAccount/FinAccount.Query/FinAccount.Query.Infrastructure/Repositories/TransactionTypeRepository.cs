using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public TransactionTypeRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(TransactionTypeEntity transactionType)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.TransactionTypes.Add(transactionType);

            _ = await context.SaveChangesAsync();
        }

        public async Task<TransactionTypeEntity> GetByNameAsync(Guid finAccountId, string name)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.TransactionTypes
                    .FirstOrDefaultAsync(x => x.FinAccountId == finAccountId && x.Name == name);
        }
    }
}