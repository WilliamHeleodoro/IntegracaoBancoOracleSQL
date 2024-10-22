using Dapper.Contrib.Extensions;

namespace IntegracaoBancoOracleSQL.Model.ModelOracle
{
    [Dapper.Contrib.Extensions.Table("VIS_NOTA_FISCAL")]
    public class PedidoOracle
    {
        public string? CnpjCpf { get; set; }
        public long Filial { get; set; }
        public long NumNota { get; set; }
        public string? SerieNota { get; set; }
        public string? ChaveAcesso { get; set; }
        public DateTime DataEmissao { get; set; }
        public string? TipoCliente { get; set; }
        public string? CCusto { get; set; }
        public string? Projeto { get; set; }
        public string? CondPgto { get; set; }
        public decimal? ValorNF { get; set; }
        public string? StatusImp { get; set; }
        public string? XmlNfs { get; set; }

        [Write(false)]
        public required List<PedidoItensServicoOracle> pedidoItensServicoOracle { get; set; }

        [Write(false)]
        public required List<PedidoParcelasOracle> pedidoParcelasOracle { get; set; }
    }
}
