using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class FinAccountQueryHandler : IFinAccountQueryHandler
    {
        private readonly IFinAccountRepository _finAccountRepository;

        public FinAccountQueryHandler(IFinAccountRepository finAccountRepository)
        {
            _finAccountRepository = finAccountRepository;
        }
     
        public async Task<List<FinAccountEntity>> HandleAsync(FindFinAccountWithBalancesByIdQuery query)
        {
            return new List<FinAccountEntity> { await _finAccountRepository.GetByIdWithBalanceAsync(query.Id) };
        }
    }
}