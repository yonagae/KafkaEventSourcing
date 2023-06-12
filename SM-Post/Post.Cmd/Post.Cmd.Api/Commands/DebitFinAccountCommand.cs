using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class DebitFinAccountCommand : BaseCommand
{
    public Decimal Amount { get; set; }
    public string TransactionType { get; set; }
}
