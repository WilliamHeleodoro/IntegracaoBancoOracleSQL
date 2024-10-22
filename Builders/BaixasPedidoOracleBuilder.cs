using IntegracaoBancoOracleSQL.Model.ModelOracle;
using IntegracaoBancoOracleSQL.Model.ModelSQL;

namespace IntegracaoBancoOracleSQL.Builders
{
    public class BaixasPedidoOracleBuilder
    {
        public PedidoBaixasOracle MontarBaixasPedidoOracleBuilder(PedidoBaixasSQL baixaSQL)
        {
            PedidoBaixasOracle pedidoBaixaOracle = new PedidoBaixasOracle
            {
                COD_ACAO = baixaSQL.CD_ITEM,
                CONTA_FINAC = baixaSQL.CD_BANCO.ToString(),
                TIPO_DOC = "NFSE",
                NUM_DOCUMENTO = baixaSQL.NR_DOCUMENTO.ToString(),
                CGC_CPF = baixaSQL.NR_CPFCNPJ,
                PARCELA = baixaSQL.NR_PARCELA.ToString(),
                VENCIMENTO = baixaSQL.DT_VENCIMENTO,
                DATA_BAIXA = baixaSQL.DT_PAGAMENTO,
                VALOR_BAIXA = baixaSQL.VL_PAGAMENTO,
                COMP_HIST = null,
            };

            return pedidoBaixaOracle;
        }
    }
}
