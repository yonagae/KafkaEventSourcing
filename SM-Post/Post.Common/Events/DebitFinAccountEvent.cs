using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class DebitFinAccountEvent : BaseEvent
    {
        public DebitFinAccountEvent() : base(nameof(DebitFinAccountEvent))
        {
        }

        public Decimal DebitAmount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
