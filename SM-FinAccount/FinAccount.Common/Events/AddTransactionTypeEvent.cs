using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class AddTransactionTypeEvent : BaseEvent
    {
        public AddTransactionTypeEvent() : base(nameof(AddTransactionTypeEvent))
        {
        }

        public string TransactioTypeName { get; set; }
    }
}
