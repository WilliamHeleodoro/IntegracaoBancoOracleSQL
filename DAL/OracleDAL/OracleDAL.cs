using Dapper;
using Dapper.Contrib.Extensions;
using IntegracaoBancoOracleSQL.Conexoes;
using IntegracaoBancoOracleSQL.Model.ModelOracle;

namespace IntegracaoBancoOracleSQL.DAL.Oracle
{
    public class OracleDAL
    {
        public string ClienteJaIntegrado(string cpfcnpj)
        {
            try
            {
                using (var conexao = new ConexaoOracle())
                {
                    conexao.Conectar();

                    var sql = @"SELECT CGC_CPF 
                    FROM VIS_CLIENTE_NOTA_FISCAL 
                    WHERE ROWNUM = 1 AND
                        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CGC_CPF, '.', ''), '-', ''), '/', ''), ' ', ''), ',', '') = 
                        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(:CGC_CPF, '.', ''), '-', ''), '/', ''), ' ', ''), ',', '')";

                    var parametros = new { CGC_CPF = cpfcnpj };


                    return conexao.ObjetoConexao.QueryFirstOrDefault<string>(sql, parametros);
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se o cliente com cpf/cnpj: {cpfcnpj} existe na Oracle" + ex.Message);
            }
        }

        public void InserirClienteOracle(ClienteOracle clienteOracle)
        {
            try
            {
                using (var conexao = new ConexaoOracle())
                {
                    conexao.Conectar();

                    var sql = @"INSERT INTO VIS_CLIENTE_NOTA_FISCAL 
                    ( 
                      tipo_cli
                    , cgc_cpf
                    , ie_rg
                    , razaosocial
                    , logradouro
                    , bairro
                    , cidade
                    , uf
                    , cep)
                VALUES 
                    (
                      :tipo_cli
                    , :cgc_cpf
                    , :ie_rg
                    , :razaosocial
                    , :logradouro
                    , :bairro
                    , :cidade
                    , :uf
                    , :cep)";

                    var parametros = new
                    {
                        tipo_cli = clienteOracle.tipo_cli,
                        cgc_cpf = clienteOracle.cgc_cpf,
                        ie_rg = clienteOracle.ie_rg,
                        razaosocial = clienteOracle.razaosocial,
                        logradouro = clienteOracle.logradouro,
                        bairro = clienteOracle.bairro,
                        cidade = clienteOracle.cidade,
                        uf = clienteOracle.uf,
                        cep = clienteOracle.cep
                    };


                    conexao.ObjetoConexao.Execute(sql, parametros);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível inserir o cliente: {clienteOracle.cgc_cpf}. " + ex.Message);
            }
        }

        public void InserirPedidoOracle(PedidoOracle pedidoOracle)
        {
            using (var conexao = new ConexaoOracle())
            {
                conexao.Conectar();

                using (var transacao = conexao.ObjetoConexao.BeginTransaction())
                {
                    try
                    {
                        string pedido = @"INSERT INTO VIS_NOTA_FISCAL
                                                (
                                                    FILIAL,
                                                    NUM_NOTA,
                                                    SERIE_NOTA,
                                                    CHAVE_ACESSO,
                                                    DATA_EMISSAO,
                                                    TIPO_CLIENTE,
                                                    CNPJ_CPF,
                                                    CCUSTO,
                                                    PROJETO,
                                                    COND_PGTO,
                                                    VALOR_NF,
                                                    STATUS_IMP,
                                                    XML_NFS
                                                )
                                                VALUES
                                                (
                                                    :FILIAL,
                                                    :NUM_NOTA,
                                                    :SERIE_NOTA,
                                                    :CHAVE_ACESSO,
                                                    :DATA_EMISSAO,
                                                    :TIPO_CLIENTE,
                                                    :CNPJ_CPF,
                                                    :CCUSTO,
                                                    :PROJETO,
                                                    :COND_PGTO,
                                                    :VALOR_NF,
                                                    :STATUS_IMP,
                                                    :XML_NFS
                                                )";
                        var parametrosPedido = new
                        {
                            FILIAL = pedidoOracle.Filial,
                            NUM_NOTA = pedidoOracle.NumNota,
                            SERIE_NOTA = pedidoOracle.SerieNota,
                            CHAVE_ACESSO = pedidoOracle.ChaveAcesso,
                            DATA_EMISSAO = pedidoOracle.DataEmissao,
                            TIPO_CLIENTE = pedidoOracle.TipoCliente,
                            CNPJ_CPF = pedidoOracle.CnpjCpf,
                            CCUSTO = pedidoOracle.CCusto,
                            PROJETO = pedidoOracle.Projeto,
                            COND_PGTO = pedidoOracle.CondPgto,
                            VALOR_NF = pedidoOracle.ValorNF,
                            STATUS_IMP = pedidoOracle.StatusImp,
                            XML_NFS = pedidoOracle.XmlNfs
                        };


                        conexao.ObjetoConexao.Execute(pedido, parametrosPedido);

                        foreach (var itemPedido in pedidoOracle.pedidoItensServicoOracle)
                        {
                            string inserirItemPedido = @"INSERT INTO VIS_ITEM_NOTA_FISCAL
                                                        (
                                                            FILIAL,
                                                            NUM_NOTA,
                                                            SERIE_NOTA,
                                                            COD_ITEM,
                                                            SEQ_ITEM,
                                                            UNIDADE,
                                                            QUANTIDADE,
                                                            VALOR_UN,
                                                            APLICACAO,
                                                            CFOP,
                                                            BASE_ISS,
                                                            ALIQ_ISS,
                                                            VALOR_ISS,
                                                            RETEM_ISS,
                                                            MUNICIPIO,
                                                            UF,
                                                            NCM
                                                        )
                                                        VALUES
                                                        (
                                                            :FILIAL,
                                                            :NUM_NOTA,
                                                            :SERIE_NOTA,
                                                            :COD_ITEM,
                                                            :SEQ_ITEM,
                                                            :UNIDADE,
                                                            :QUANTIDADE,
                                                            :VALOR_UN,
                                                            :APLICACAO,
                                                            :CFOP,
                                                            :BASE_ISS,
                                                            :ALIQ_ISS,
                                                            :VALOR_ISS,
                                                            :RETEM_ISS,
                                                            :MUNICIPIO,
                                                            :UF,
                                                            :NCM
                                                        )";

                            var parametrosItemPedido = new
                            {
                                FILIAL = itemPedido.Filial,
                                NUM_NOTA = itemPedido.NumNota,
                                SERIE_NOTA = itemPedido.SerieNota,
                                COD_ITEM = itemPedido.CodItem,
                                SEQ_ITEM = itemPedido.SeqItem,
                                UNIDADE = itemPedido.Unidade,
                                QUANTIDADE = itemPedido.Quantidade,
                                VALOR_UN = itemPedido.ValorUn,
                                APLICACAO = itemPedido.Aplicacao,
                                CFOP = itemPedido.Cfop,
                                BASE_ISS = itemPedido.BaseIss,
                                ALIQ_ISS = itemPedido.AliqIss,
                                VALOR_ISS = itemPedido.ValorIss,
                                RETEM_ISS = itemPedido.RetemIss,
                                MUNICIPIO = itemPedido.Municipio,
                                UF = itemPedido.Uf,
                                NCM = itemPedido.Ncm
                            };

                            conexao.ObjetoConexao.Execute(inserirItemPedido, parametrosItemPedido);
                        }

                        foreach (var parcela in pedidoOracle.pedidoParcelasOracle)
                        {
                            string insertParcelaPedido = @"INSERT INTO VIS_PARCELA_NOTA_FISCAL
                                                               (
                                                                   FILIAL,
                                                                   NUM_NOTA,
                                                                   SERIE_NOTA,
                                                                   NUM_DOCUMENTO,
                                                                   PARCELA,
                                                                   VENCIMENTO,
                                                                   PERCENTUAL,
                                                                   VALOR_PARCELA
                                                               )
                                                               VALUES
                                                               (
                                                                   :FILIAL,
                                                                   :NUM_NOTA,
                                                                   :SERIE_NOTA,
                                                                   :NUM_DOCUMENTO,
                                                                   :PARCELA,
                                                                   :VENCIMENTO,
                                                                   :PERCENTUAL,
                                                                   :VALOR_PARCELA
                                                               )";

                            var parametrosParcelaPedido = new
                            {
                                FILIAL = parcela.Filial,
                                NUM_NOTA = parcela.NumNota,
                                SERIE_NOTA = parcela.SerieNota,
                                NUM_DOCUMENTO = parcela.NumDocumento,
                                PARCELA = parcela.Parcela,
                                VENCIMENTO = parcela.Vencimento,
                                PERCENTUAL = parcela.Percentual,
                                VALOR_PARCELA = parcela.ValorParcela
                            };

                            conexao.ObjetoConexao.Execute(insertParcelaPedido, parametrosParcelaPedido);
                        }

                        transacao.Commit();

                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        throw new Exception($"Não foi inserido o pedido referente a nota: {pedidoOracle.NumNota}, mensagem: {ex.Message}");
                    }

                }
            }
        }

        public void InserirBaixaPedidoOracle(PedidoBaixasOracle baixaOracle)
        {
            try
            {
                using (var conexao = new ConexaoOracle())
                {
                    conexao.Conectar();

                    var sql = @"INSERT INTO VIS_BXPARCELA_NOTA_FISCAL 
                                ( 
                                  COD_ACAO
                                , CONTA_FINAC
                                , TIPO_DOC
                                , NUM_DOCUMENTO
                                , CGC_CPF
                                , PARCELA
                                , VENCIMENTO
                                , DATA_BAIXA
                                , VALOR_BAIXA
                                , COMP_HIST)
                            VALUES 
                                (
                                  :COD_ACAO
                                , :CONTA_FINAC
                                , :TIPO_DOC
                                , :NUM_DOCUMENTO
                                , :CGC_CPF
                                , :PARCELA
                                , :VENCIMENTO
                                , :DATA_BAIXA
                                , :VALOR_BAIXA
                                , :COMP_HIST)";

                    var parametros = new
                    {
                        COD_ACAO = baixaOracle.COD_ACAO,
                        CONTA_FINAC = baixaOracle.CONTA_FINAC,
                        TIPO_DOC = baixaOracle.TIPO_DOC,
                        NUM_DOCUMENTO = baixaOracle.NUM_DOCUMENTO,
                        CGC_CPF = baixaOracle.CGC_CPF,
                        PARCELA = baixaOracle.PARCELA,
                        VENCIMENTO = baixaOracle.VENCIMENTO,
                        DATA_BAIXA = baixaOracle.DATA_BAIXA,
                        VALOR_BAIXA = baixaOracle.VALOR_BAIXA,
                        COMP_HIST = baixaOracle.COMP_HIST
                    };

                    conexao.ObjetoConexao.Execute(sql, parametros);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível inserir o baixa item: {baixaOracle.COD_ACAO}. Referente ao documento: {baixaOracle.NUM_DOCUMENTO} " + ex.Message);
            }
        }
    }
}
