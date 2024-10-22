using IntegracaoBancoOracleSQL.Conexoes;
using IntegracaoBancoOracleSQL.Servicos;
using static IntegracaoBancoOracleSQL.Logs;

namespace IntegracaoBancoOracleSQL
{
    public static class Globais
    {
        public static Logs log { get; set; } = new Logs(AppDomain.CurrentDomain.BaseDirectory, "IntegracaoBancoOracleSQL");
        public static TipoOperacaoLog operacaoLOG { get; set; } = TipoOperacaoLog.Iniciar;
    }

    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Globais.log.Info(TipoOperacaoLog.Iniciar, $"Iniciando Integração ");

                var conexao = new ConexaoSQL();

                if (string.IsNullOrEmpty(conexao.ObjetoConexao.Database))
                {
                    Globais.log.Info(TipoOperacaoLog.Iniciar, $"Não achou nenhuma conexão. Verifique se possui o VSCONF");
                    return;
                }

                new EnviarPedidos().EnviarPedido();

                new EnviarBaixasPedidos().EnviarBaixaPedido();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Integração: {ex.Message}");
                Globais.log.Info(TipoOperacaoLog.Pedido, $"Integração: {ex.Message}");
            }

        }
    }
}
