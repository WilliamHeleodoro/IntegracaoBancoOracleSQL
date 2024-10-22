

using IntegracaoBancoOracleSQL.Builders;
using IntegracaoBancoOracleSQL.DAL.Oracle;
using IntegracaoBancoOracleSQL.DAL.SQLDAL;
using IntegracaoBancoOracleSQL.Model.ModelOracle;
using IntegracaoBancoOracleSQL.Model.ModelSQL;

namespace IntegracaoBancoOracleSQL.Servicos
{
    public class EnviarPedidos
    {
        OracleDAL oracleDAL = new OracleDAL();
        SqlDAL sqlDAL = new SqlDAL();

        public void EnviarPedido()
        {

            List<PedidoSQL> pedidos = sqlDAL.BuscarPedidos();

            foreach (var pedido in pedidos)
            {
                try
                {
                    ClienteSQL cliente = sqlDAL.BuscarCliente(pedido.CD_CLIENTE);

                    string cpfcnpj = oracleDAL.ClienteJaIntegrado(cliente.NR_CPFCNPJ);

                    if (cpfcnpj == null)
                    {
                        new EnviarCliente().EnviarClienteOracle(cliente);
                    }

                    PedidoOracle pedidoOracle = new PedidoOracleBuilder().MontarPedidoOracleBuilder(pedido, cliente);

                    oracleDAL.InserirPedidoOracle(pedidoOracle);
                    sqlDAL.InserirPedidoIntegradoSQL(pedido);

                    Console.WriteLine($"Enviado o pedido: {pedido.CD_PEDIDO} com sucesso.");
                    Globais.log.Info(Logs.TipoOperacaoLog.Pedido, $"Enviado o pedido: {pedido.CD_PEDIDO} com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Falha ao integrar o pedido: {pedido.CD_PEDIDO}. {ex.Message}");
                    Globais.log.Info(Logs.TipoOperacaoLog.Pedido, $"Falha ao integrar o pedido: {pedido.CD_PEDIDO}. {ex.Message}");
                }

            }
        }
    }
}
