using IntegracaoBancoOracleSQL.Model.ModelOracle;
using IntegracaoBancoOracleSQL.Model.ModelSQL;

namespace IntegracaoBancoOracleSQL.Builders
{
    public class ClienteOracleBuilder
    {
        public ClienteOracle MontarClienteOracleBuilder(ClienteSQL cliente)
        {
            ClienteOracle clienteOracle = new ClienteOracle
            {
                tipo_cli = cliente.NR_CPFCNPJ.Where(char.IsDigit).ToArray().Length == 14 ? "J" : "F",
                cgc_cpf = cliente.NR_CPFCNPJ,
                ie_rg = string.IsNullOrEmpty(cliente.NR_IE) ? "ISENTO" : cliente.NR_IE,
                razaosocial = cliente.DS_ENTIDADE,
                logradouro = cliente.DS_ENDERECO,
                bairro = cliente.DS_BAIRRO,
                cep = cliente.NR_CEP,
                cidade = cliente.DS_CIDADE,
                uf = cliente.DS_UF,
            };

            return clienteOracle;
        }   
    }
}
