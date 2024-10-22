using IntegracaoBancoOracleSQL.Builders;
using IntegracaoBancoOracleSQL.DAL.Oracle;
using IntegracaoBancoOracleSQL.DAL.SQLDAL;
using IntegracaoBancoOracleSQL.Model.ModelOracle;
using IntegracaoBancoOracleSQL.Model.ModelSQL;

namespace IntegracaoBancoOracleSQL.Servicos
{
    public class EnviarBaixasPedidos
    {
        SqlDAL sqlDAL = new SqlDAL();
        OracleDAL oracleDAL = new OracleDAL();

        public void EnviarBaixaPedido()
        {
            List<PedidoBaixasSQL> listaPedidoBaixas = sqlDAL.BuscarPedidosBaixas();

            foreach (var baixaSQL in listaPedidoBaixas)
            {
                try
                {
                    PedidoBaixasOracle baixaOracle = new BaixasPedidoOracleBuilder().MontarBaixasPedidoOracleBuilder(baixaSQL);

                    oracleDAL.InserirBaixaPedidoOracle(baixaOracle);

                    sqlDAL.InserirBaixasPedidoIntegradoSQL(baixaSQL);

                    Console.WriteLine($"Enviado a baixa item: {baixaSQL.CD_ITEM}, referente ao pedido: {baixaSQL.CD_PEDIDO} com sucesso.");
                    Globais.log.Info(Logs.TipoOperacaoLog.Baixas, $"Enviado a baixa item: {baixaSQL.CD_ITEM}, referente ao pedido: {baixaSQL.CD_PEDIDO} com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Falha ao integrar a baixa item: {baixaSQL.CD_ITEM} referente ao pedido: {baixaSQL.CD_PEDIDO}. {ex.Message}");
                    Globais.log.Info(Logs.TipoOperacaoLog.Baixas, $"Falha ao integrar a baixa item: {baixaSQL.CD_ITEM} referente ao pedido: {baixaSQL.CD_PEDIDO}. {ex.Message}");
                }
            }
        }
    }
}
