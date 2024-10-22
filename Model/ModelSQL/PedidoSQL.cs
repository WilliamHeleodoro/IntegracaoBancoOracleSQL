using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracaoBancoOracleSQL.Model.ModelSQL
{

    [Table("TBL_VENDAS_PEDIDO")]
    public class PedidoSQL
    {
        [Required]
        public long CD_PEDIDO { get; set; }
        public long CD_EMPRESA { get; set; }
        public long CD_CLIENTE { get; set; }
        public long NR_NOTAFISCAL { get; set; }
        public string? DS_DF_SERIE { get; set; }
        public string? CV_ACESSO {  get; set; }
        public DateTime DT_EMISSAO { get; set; }
        public string? DS_FRPAGTO {  get; set; }
        public decimal? VL_PEDIDO { get; set; }
        public byte[]? DS_ARQUIVO { get; set; }
        public required List<PedidoItensServicoSQL> pedidoServicoSQL { get; set; }
        public required List<PedidoParcelasSQL> pedidoParcelasSQL { get; set; }
    }

}
