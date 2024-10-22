using Dapper.Contrib.Extensions;

namespace IntegracaoBancoOracleSQL.Model.ModelOracle
{
    [Table("VIS_CLIENTE_NOTA_FISCAL")]
    public class ClienteOracle
    {
        [Key]
        public string? cgc_cpf { get; set; }
        public string? tipo_cli {  get; set; }

        public string? ie_rg { get; set; }

        public string? razaosocial { get; set; }

        public string? logradouro { get; set; }

        public string? bairro { get; set; }

        public string? cidade { get; set; }
        public string? uf { get; set; }
        public string? cep { get; set; }

    }
}
