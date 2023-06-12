using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CreditFinAccountEvent : BaseEvent
    {
        public CreditFinAccountEvent() : base(nameof(CreditFinAccountEvent))
        {
        }

        public Decimal CreditAmount { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

