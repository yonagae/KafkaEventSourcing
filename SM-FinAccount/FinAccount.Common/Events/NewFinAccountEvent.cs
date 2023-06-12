using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class NewFinAccountEvent : BaseEvent
    {
        public NewFinAccountEvent() : base(nameof(NewFinAccountEvent))
        {
        }
        public string Owner { get; set; }
        public DateTime CreationDate { get; set; }
    }
}