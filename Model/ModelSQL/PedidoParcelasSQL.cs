
namespace IntegracaoBancoOracleSQL.Model.ModelSQL
{
    public class PedidoParcelasSQL
    {
        public long CD_PEDIDO { get; set; }
        public int NR_PARCELA {  get; set; }
        public DateTime DT_VENCIMENTO { get; set; }
        public decimal VL_PARCELA {  set; get; }
    }
}
