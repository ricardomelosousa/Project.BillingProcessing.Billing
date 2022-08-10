namespace Project.BillingProcessing.Billing.Api.EventBus.Events
{
    public record ChargeIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; }
        public DateTime DueDate { get; set; }
        public string Month { get; set; }
        public decimal ChargeValue { get; set; }
        public long Identification { get; set; }
    }
}
