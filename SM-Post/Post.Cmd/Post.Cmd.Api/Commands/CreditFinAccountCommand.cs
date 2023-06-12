using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class CreditFinAccountCommand : BaseCommand
{
    public Decimal Amount { get; set; }
}
