using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBancoOracleSQL.Model.ModelOracle
{
    public class PedidoParcelasOracle
    {
        public long Filial { get; set; }
        public long NumNota { get; set; }
        public string? SerieNota { get; set; }
        public string? NumDocumento { get; set; }
        public int Parcela { get; set; }
        public DateTime Vencimento { get; set; }
        public string? Percentual { get; set; }
        public decimal ValorParcela { get; set; }
    }
}
