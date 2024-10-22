using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBancoOracleSQL.Model.ModelOracle
{
    public class PedidoBaixasOracle
    {
        public int COD_ACAO { get; set; }
        public string? CONTA_FINAC { get; set; }
        public string? TIPO_DOC { get; set; }
        public string? NUM_DOCUMENTO { get; set; }
        public string? CGC_CPF { get; set; }
        public string? PARCELA { get; set; }
        public DateTime VENCIMENTO { get; set; }
        public DateTime DATA_BAIXA { get; set; }
        public decimal VALOR_BAIXA { get; set; }
        public string? COMP_HIST { get; set; }
    }
}
