using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class NewFinAccountCommand : BaseCommand
{
    public string Owner { get; set; }
}
