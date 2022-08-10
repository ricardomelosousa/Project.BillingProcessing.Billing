using System.Text.Json.Serialization;

namespace Project.BillingProcessing.Api.EventBus.Events
{
    public record IntegrationEvent
    {
        public IntegrationEvent()
        {
            IntegrationId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            IntegrationId = id;
            CreationDate = createDate;
        }

        [JsonInclude]
        public Guid IntegrationId { get; private init; }

        [JsonInclude]
        public DateTime CreationDate { get; private init; }
    }
}
