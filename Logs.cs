using System.ComponentModel;

namespace IntegracaoBancoOracleSQL
{
    public class Logs
    {
        private string Caminho { get; set; } = "C:\\";
        private string NomeArquivo { get; set; } = "Log_info.txt";
        private string TipoIntegracao { get; set; }

        public Logs(string caminho, string tipo)
        {
            Caminho = caminho;
            TipoIntegracao = tipo;

            NomeArquivo = DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "");
        }

        public void Info(TipoOperacaoLog operacao, string log, bool parar = false)
        {
            try
            {

                var DetalheErro = DateTime.Now + " - (" + operacao + ") " + log +
                    Environment.NewLine +
                    "-----------------------------------------------------------------------------------------------------------" +
                    Environment.NewLine;
                Console.Write(DetalheErro);
                if (!Directory.Exists($@"{Caminho}\logs\"))
                    Directory.CreateDirectory($@"{Caminho}\logs\");
                File.AppendAllText($@"{Caminho}\logs\{NomeArquivo}", DetalheErro);
            }
            catch
            {
            }
        }

        public enum TipoOperacaoLog
        {
            [Description("INICIAR")]
            Iniciar,
            [Description("PEDIDO")]
            Pedido,
            [Description("CLIENTE")]
            Cliente,
            [Description("BAIXAS")]
            Baixas,
        }
    }
}
