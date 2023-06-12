using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class AddTransactionTypeCommand : BaseCommand
{
    public string Name { get; set; }
}
