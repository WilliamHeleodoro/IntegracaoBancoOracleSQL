using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracaoBancoOracleSQL.Model.ModelSQL
{
    [Table("SEL_ENTIDADES")]
    public class ClienteSQL
    {
        public required string NR_CPFCNPJ { get; set; }
        public long CD_ENTIDADE { get; set; }
        public string? DS_ENTIDADE { get; set; }
        public string? NR_IE { get; set; }
        public string? DS_ENDERECO { get; set; }
        public string? DS_BAIRRO { get; set; }
        public string? NR_CEP { get; set; }
        public string? DS_CIDADE{ get; set; }
        public string? DS_UF { get; set; }

    }
}
