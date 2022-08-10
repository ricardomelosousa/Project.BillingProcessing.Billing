namespace AccessBrasil.Logistica.TriagemOS.Integrador.Api.EventBus.Events
{
    public record OrdemServicoDtoIntegrationEvent : IntegrationEvent
    {
        #region Propredades Genéricas
        public int OrdemServicoId { get; set; }
        public DateTime OrdemServicoDataCriacao { get; set; }
        public int? QtdeCaixa { get; set; }
        public int? QtdePagina { get; set; }
        public string? UsuarioSolicitanteNome { get; set; }
        public string? Observacoes { get; set; }
        public DateTime? DataLimiteAtendimento { get; set; }
        public int StatusId { get; set; }
        public string? ClienteNome { get; set; }
        public string? AplicacaoNome { get; set; }
        public int? PendenteTrocaCartonagem { get; set; }
        public DateTime? VolumeDataCriacao { get; set; }
        public string? CodigoBarrasProvisorio { get; set; }
        public string? CodigoBarrasDefinitivo { get; set; }
        public string? CodProvisoriaAntiga { get; set; }
        public string? CodDefinitivaAntiga { get; set; }
        public bool? Anexo { get; set; }
        public string? MeioArmazenamento { get; set; }
        public string? LocalGuarda { get; set; }
        public string? LocalAtual { get; set; }
        public bool? Estanteria { get; set; }
        public int? Estante { get; set; }
        public string? Lado { get; set; }
        public int? ColunaEstanteria { get; set; }
        public int? Andar { get; set; }
        public int? Vao { get; set; }
        public int? ColunaVao { get; set; }
        public int? Posicao { get; set; }
        public int? AndarVao { get; set; }
        public bool? MonitorarSla { get; set; }
        public string? Corredor { get; set; }
        public int? Piso { get; set; }
        public string? DescricaoGalpao { get; set; }
        public string? DescricaoZona { get; set; }
        public string? LocalObservacoes { get; set; }
        public string? Contato { get; set; }
        public DateTime? DataAtendimento { get; set; }
        public bool? EnviadoCliente { get; set; }
        public OrigemOrdemServico OrdemServicoOrigem { get; set; }
        public long? OrdemServicoItemId { get; set; }
        public bool? Movimentado { get; set; }
        public int? SistemaOrigem { get; set; }
        public int? TipoEntrega { get; set; }
        public bool? Justificada { get; set; }
        public bool? IniciadoAtendimento { get; set; }
        public DateTime? OrdemServicoItemDataCriacao { get; set; }
        public string? Barcode { get; set; }

        public int? SlaHoras { get; set; }
        public bool? SlaHoraUtil { get; set; }
        public int? TipoAtendimentoOS { get; set; }
        public bool? AlertaOs { get; set; }
        public int? TipoServico { get; set; }
        public bool? EmbalagemAssociada { get; set; }
        public bool? EntregaCliente { get; set; }
        public bool? RetiraCliente { get; set; }
        public bool? SalaConsulta { get; set; }
        public string? Logradouro { get; set; }
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public bool? ConferidoViagem { get; set; }
        public bool? EtiquetasImpressas { get; set; }
        public int? ViagemId { get; set; }
        public string? MotivoCancelamento { get; set; }
        public int? OrdemViagem { get; set; }
        public string? ObservacaoExpurgo { get; set; }
        public DateTime? DataDelegacao { get; set; }
        public bool? Acatado { get; set; }
        public int? CodTipoAntiga { get; set; }
        public int? SequenciaCaixa { get; set; }
        public string? Localizacao { get; set; }
        public float Contrato { get; set; }
        public string? Atendente { get; set; }
        public string? Localcoleta { get; set; }
        public string? CaixasSolicitadas { get; set; }
        public string? Cartonagem { get; set; }
        public char? FlagVisualiza { get; set; }
        public int? Filial { get; set; }
        public string? PaginasSolicitadas { get; set; }
        public string? AcatarMotivo { get; set; }
        public string? AcatarJustificativa { get; set; }
        public DateTime? DataAtendimentoReal { get; set; }
        public int? TipoCaixa { get; set; }
        public string? NomeUnidade { get; set; }
        public string? ServicoOperacionalNome { get; set; }
        public string? DescricaoStatus { get; set; }
        public Status StatusProcessamento { get; set; }

        #endregion





    }
}
