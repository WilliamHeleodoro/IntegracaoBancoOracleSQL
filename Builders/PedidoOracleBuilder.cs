using IntegracaoBancoOracleSQL.Model.ModelOracle;
using IntegracaoBancoOracleSQL.Model.ModelSQL;
using System.IO.Compression;
using System.Text;

namespace IntegracaoBancoOracleSQL.Builders
{
    public class PedidoOracleBuilder
    {
        public PedidoOracle MontarPedidoOracleBuilder(PedidoSQL pedidoSQL, ClienteSQL clienteSQL)
        {
            string xml = "";
            if (pedidoSQL.DS_ARQUIVO != null)
            {
                using (MemoryStream zipStream = new MemoryStream(pedidoSQL.DS_ARQUIVO))
                {
                    // Abre o arquivo ZIP a partir da stream
                    using (ZipArchive archive = new ZipArchive(zipStream))
                    {
                        // Procura o arquivo XML dentro do ZIP
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                            {
                                // Extrai e lê o conteúdo do arquivo XML
                                using (Stream xmlStream = entry.Open())
                                {
                                    using (StreamReader reader = new StreamReader(xmlStream, Encoding.UTF8))
                                    {
                                        xml  = reader.ReadToEnd();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            List<PedidoItensServicoOracle> listaServicosOracle = new List<PedidoItensServicoOracle>();
            foreach(var servico in pedidoSQL.pedidoServicoSQL)
            {
                PedidoItensServicoOracle servicoOracle = new PedidoItensServicoOracle
                {
                    Filial = pedidoSQL.CD_EMPRESA,
                    NumNota = pedidoSQL.NR_NOTAFISCAL,
                    SerieNota = pedidoSQL.DS_DF_SERIE,
                    CodItem = servico.CD_SERVICO,
                    SeqItem = servico.CD_ITEM,
                    Unidade = servico.DS_UNIDADE,
                    Quantidade = servico.NR_QUANTIDADE,
                    ValorUn = servico.VL_UNITARIO,
                    Aplicacao = null,
                    Cfop = servico.DS_CFOP,
                    BaseIss = servico.VL_BASE_ISS,
                    AliqIss = servico.AL_ISS,
                    ValorIss = servico.VL_ISS,
                    RetemIss = servico.X_ISS_RETIDO == true ? '1' : '0',
                    Municipio = servico.DS_CIDADE_RET,
                    Uf = servico.DS_UF,
                    Ncm = "39269022"
                };

                listaServicosOracle.Add(servicoOracle);
            }

            List<PedidoParcelasOracle> listaParcelasOracle = new List<PedidoParcelasOracle> ();
            foreach(var parcela in pedidoSQL.pedidoParcelasSQL)
            {
                PedidoParcelasOracle parcelaOracle = new PedidoParcelasOracle
                {
                    Filial = pedidoSQL.CD_EMPRESA,
                    NumNota = pedidoSQL.NR_NOTAFISCAL,
                    SerieNota = pedidoSQL.DS_DF_SERIE,
                    NumDocumento = pedidoSQL.NR_NOTAFISCAL.ToString(),
                    Parcela = parcela.NR_PARCELA,
                    Percentual = null,
                    Vencimento = parcela.DT_VENCIMENTO,
                    ValorParcela = parcela.VL_PARCELA,
                };

                listaParcelasOracle.Add (parcelaOracle);
            }

            PedidoOracle Oracle = new PedidoOracle
            {
                Filial = pedidoSQL.CD_EMPRESA,
                NumNota = pedidoSQL.NR_NOTAFISCAL,
                SerieNota = pedidoSQL.DS_DF_SERIE,
                ChaveAcesso = pedidoSQL.CV_ACESSO,
                DataEmissao = pedidoSQL.DT_EMISSAO,
                TipoCliente = clienteSQL.NR_CPFCNPJ.Where(char.IsDigit).ToArray().Length == 14 ? "J" : "F",
                CnpjCpf = clienteSQL.NR_CPFCNPJ,
                CCusto = null,
                Projeto = null,
                CondPgto = pedidoSQL.DS_FRPAGTO,
                StatusImp = "PEN",
                ValorNF = pedidoSQL.VL_PEDIDO,
                XmlNfs = xml,
                pedidoItensServicoOracle = listaServicosOracle,
                pedidoParcelasOracle = listaParcelasOracle,
            };

            return Oracle;   
        }
    }
}
