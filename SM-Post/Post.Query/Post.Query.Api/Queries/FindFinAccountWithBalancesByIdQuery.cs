using CQRS.Core.Queries;

namespace Post.Query.Api.Queries
{
    public class FindFinAccountWithBalancesByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}