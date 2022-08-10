namespace AccessBrasil.Logistica.TriagemOS.Integrador.Api.EventBus.Events
{
    public record OrdemServicoAtendimentoIntegrationEvent : IntegrationEvent
    {
        public int OrdemServicoId { get; set; }
        public long? OrdemServicoItemId { get; set; }
        public int UserId { get; set; }
        public int StatusOrdemServicoId { get; set; }
        public DateTime OrdemServicoDataCriacao { get; set; }       
        public DateTime? DataAtendimento { get; set; }
        public StatusAtendimento StatusAtendimento { get; set; }
        public OrigemOrdemServico Origem { get; set; }
    }

    public enum StatusAtendimento
    {
        Disponivel,
        EmProcessamento,
        Finalizada,
        IntegradaAllStore,
        Erro
    }

    public enum OrigemOrdemServico
    {
        Legado,
        Novo
        
    }
}
