using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FinAccountLookupController : ControllerBase
    {
        private readonly ILogger<FinAccountLookupController> _logger;
        private readonly IQueryDispatcher<FinAccountEntity> _queryDispatcher;

        public FinAccountLookupController(ILogger<FinAccountLookupController> logger, IQueryDispatcher<FinAccountEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("byId/{finAccountId}")]
        public async Task<ActionResult> FindFinAccountWithBalancesByIdQuery(Guid finAccountId)
        {
            try
            {
                var finAccounts = await _queryDispatcher.SendAsync(new FindFinAccountWithBalancesByIdQuery { Id = finAccountId });

                if (finAccounts == null || !finAccounts.Any())
                    return NoContent();

                return Ok(new FinAccountResponse
                {
                    FinAccounts = finAccounts,
                    Message = "Successfully returned post!"
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find post by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<FinAccountEntity> posts)
        {
            if (posts == null || !posts.Any())
                return NoContent();

            var count = posts.Count;
            return Ok(new FinAccountResponse
            {
                FinAccounts = posts,
                Message = $"Successfully returned {count} post{(count > 1 ? "s" : string.Empty)}!"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }
    }
}