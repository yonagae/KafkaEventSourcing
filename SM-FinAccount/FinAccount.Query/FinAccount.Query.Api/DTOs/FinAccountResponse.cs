using Post.Common.DTOs;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.DTOs
{
    public class FinAccountResponse : BaseResponse
    {
        public List<FinAccountEntity> FinAccounts { get; set; }
    }
}