using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBancoOracleSQL.Model.ModelOracle
{
    public class PedidoItensServicoOracle
    {
        public long Filial { get; set; }
        public long NumNota { get; set; }
        public string? SerieNota { get; set; }
        public long CodItem { get; set; }
        public long SeqItem { get; set; }
        public string? Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUn { get; set; }
        public string? Aplicacao { get; set; }
        public string? Cfop { get; set; }
        public decimal BaseIss { get; set; }
        public decimal AliqIss { get; set; }
        public decimal ValorIss { get; set; }
        public char RetemIss { get; set; }
        public string? Municipio { get; set; }
        public string? Uf { get; set; }
        public string? Ncm { get; set; }
    }
}
