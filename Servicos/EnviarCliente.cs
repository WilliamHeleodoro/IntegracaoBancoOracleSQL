using IntegracaoBancoOracleSQL.Builders;
using IntegracaoBancoOracleSQL.DAL.Oracle;
using IntegracaoBancoOracleSQL.DAL.SQLDAL;
using IntegracaoBancoOracleSQL.Model.ModelOracle;
using IntegracaoBancoOracleSQL.Model.ModelSQL;

namespace IntegracaoBancoOracleSQL.Servicos
{
    public class EnviarCliente
    {
        OracleDAL oracleDAL = new OracleDAL();
        SqlDAL sqlDAL = new SqlDAL();

        public void EnviarClienteOracle(ClienteSQL cliente)
        {
            try
            {
                ClienteOracle clienteOracle = new ClienteOracleBuilder().MontarClienteOracleBuilder(cliente);

                oracleDAL.InserirClienteOracle(clienteOracle);

                Console.WriteLine($"Enviado o cliente Cód: {cliente.CD_ENTIDADE}. CNPJ: {cliente.NR_CPFCNPJ} com sucesso.");
                Globais.log.Info(Logs.TipoOperacaoLog.Cliente, $"Enviado o cliente Cód: {cliente.CD_ENTIDADE}. CNPJ: {clienteOracle.cgc_cpf} com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha ao enviar o cliente Cód: {cliente.CD_ENTIDADE}. CNPJ: {cliente.NR_CPFCNPJ}. {ex.Message}");
                Globais.log.Info(Logs.TipoOperacaoLog.Cliente, $"Falha ao enviar o cliente Cód: {cliente.CD_ENTIDADE}. CNPJ: {cliente.NR_CPFCNPJ}. {ex.Message}");
                throw new Exception($"Falha ao enviar o cliente Cód: {cliente.CD_ENTIDADE}. CNPJ: {cliente.NR_CPFCNPJ}. {ex.Message}");
            }
        }
    }
}
