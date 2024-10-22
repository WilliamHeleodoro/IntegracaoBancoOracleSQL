using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBancoOracleSQL.Model.ModelSQL
{
    public class PedidoBaixasSQL
    {
        public int CD_PEDIDO { get; set; }
        public int NR_PARCELA { get; set; }
        public int CD_ITEM { get; set; }
        public DateTime DT_PAGAMENTO { get; set; }
        public DateTime DT_VENCIMENTO { get; set; }
        public decimal VL_PAGAMENTO { get; set; }
        public long NR_DOCUMENTO { get; set; }
        public int CD_CONTA { get; set; }
        public string? DS_CONTA { get; set; }
        public int CD_BANCO { get; set; }
        public string? DS_BANCO { get; set; }
        public int CD_CARTEIRA { get; set; }
        public string? DS_CARTEIRA { get; set; }
        public decimal VL_JURO { get; set; }
        public decimal VL_DESCONTO { get; set; }
        public string? NR_CPFCNPJ { get; set; }
    }
}
