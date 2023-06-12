using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries
{
    public interface IFinAccountQueryHandler
    {
        Task<List<FinAccountEntity>> HandleAsync(FindFinAccountWithBalancesByIdQuery query);
    }
}