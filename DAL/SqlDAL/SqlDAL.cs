using Dapper;
using IntegracaoBancoOracleSQL.Conexoes;
using IntegracaoBancoOracleSQL.Model.ModelSQL;

namespace IntegracaoBancoOracleSQL.DAL.SQLDAL
{
    public class SqlDAL
    {
        private ConexaoSQL _conexaoSQL = new ConexaoSQL();

        public List<PedidoSQL> BuscarPedidos()
        {
            try
            {
                var sql = @"SELECT 
                        PEDIDO.*, 
                        XML.DS_ARQUIVO, 
                        PAGTO.DS_FRPAGTO, 
                        SERVICOS.*, 
                        ESTADO.DS_UF,
                        PARCELAS.*  
                    FROM TBL_VENDAS_PEDIDO PEDIDO
                    INNER JOIN TBL_NFSE2_XML XML                   ON XML.CD_PEDIDO      = PEDIDO.CD_PEDIDO
                    INNER JOIN TBL_FINANCEIRO_FORMA_PAGTO PAGTO    ON PAGTO.CD_FRPAGTO   = PEDIDO.CD_FRPAGTO
                    LEFT JOIN TBL_VENDAS_PEDIDO_SERVICOS SERVICOS  ON SERVICOS.CD_PEDIDO = PEDIDO.CD_PEDIDO
                    LEFT JOIN TBL_VENDAS_PEDIDO_PARCELAS PARCELAS  ON PARCELAS.CD_PEDIDO = PEDIDO.CD_PEDIDO
                    LEFT JOIN TBL_CEP_CIDADE CIDADE                ON CIDADE.CD_CIDADE   = SERVICOS.CD_CIDADE_RET
					LEFT JOIN TBL_CEP_ESTADO ESTADO                ON ESTADO.CD_ESTADO   = CIDADE.CD_ESTADO
                    WHERE PEDIDO.CD_STATUS = 2 
                    AND CAST(PEDIDO.DT_EMISSAO AS DATE) >= CAST(GETDATE() -1 AS DATE) 
                    AND PEDIDO.CD_PEDIDO NOT IN (
                        SELECT INTE.CD_PEDIDO 
                        FROM TBL_PEDIDOS_INTEGRACAO INTE
                        WHERE INTE.CD_PEDIDO = PEDIDO.CD_PEDIDO
                    );";

                var pedidoDict = new Dictionary<long, PedidoSQL>();

                var result = _conexaoSQL.ObjetoConexao.Query<PedidoSQL, PedidoItensServicoSQL, PedidoParcelasSQL, PedidoSQL>(
                    sql,
                    (pedido, servico, parcela) =>
                    {
                        // Verifica se o pedido já está no dicionário
                        if (!pedidoDict.TryGetValue(pedido.CD_PEDIDO, out var pedidoEntry))
                        {
                            pedidoEntry = pedido;
                            pedidoEntry.pedidoServicoSQL = new List<PedidoItensServicoSQL>();
                            pedidoEntry.pedidoParcelasSQL = new List<PedidoParcelasSQL>();
                            pedidoDict.Add(pedido.CD_PEDIDO, pedidoEntry);
                        }

                        // Adiciona o item de serviço sem duplicar o pedido
                        if (servico != null && pedidoEntry.pedidoServicoSQL != null && !pedidoEntry.pedidoServicoSQL.Any(s => s.CD_ITEM == servico.CD_ITEM))
                        {
                            pedidoEntry.pedidoServicoSQL.Add(servico);
                        }

                        // Adiciona a parcela sem duplicar o pedido
                        if (parcela != null && pedidoEntry.pedidoParcelasSQL != null && !pedidoEntry.pedidoParcelasSQL.Any(p => p.NR_PARCELA == parcela.NR_PARCELA))
                        {
                            pedidoEntry.pedidoParcelasSQL.Add(parcela);
                        }

                        return pedidoEntry;
                    },
                    splitOn: "CD_PEDIDO"
                ).Distinct().ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível buscar os pedidos para integração " + ex.Message);
            }
        }


        public ClienteSQL BuscarCliente(long codigoCliente)
        {
            try
            {
                var sql = @"SELECT CD_ENTIDADE, NR_CPFCNPJ, DS_ENTIDADE, NR_IE, DS_ENDERECO, DS_BAIRRO, NR_CEP , DS_CIDADE , DS_UF 
                                    FROM SEL_ENTIDADES WHERE CD_ENTIDADE = @CD_ENTIDADE";

                var parametros = new { CD_ENTIDADE = codigoCliente };


                return _conexaoSQL.ObjetoConexao.QueryFirst<ClienteSQL>(sql, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível buscar o cliente: {codigoCliente} para integração " + ex.Message);
            }
        }

        public void InserirPedidoIntegradoSQL(PedidoSQL pedido)
        {
            try
            {
                var sql = @"INSERT INTO TBL_PEDIDOS_INTEGRACAO_ENVIO
                      (
                        CD_PEDIDO
                       ,DS_INTEGRACAO 
                       ,NR_NOTAFISCAL
                       ,DT_EMISSAO
                       ,DT_ENVIO
                       ,DS_TIPO
                       ,X_ENVIADO
                      )
                      VALUES
                      (
                        @CD_PEDIDO
                       ,'SQL_ORACLE'
                       ,@NR_NOTAFISCAL
                       ,@DT_EMISSAO
                       ,GETDATE()
                       ,'ERP'
                       ,1
                      )";

                var parametros = new
                {
                    CD_PEDIDO = pedido.CD_PEDIDO,
                    NR_NOTAFISCAL = pedido.NR_NOTAFISCAL,
                    DT_EMISSAO = pedido.DT_EMISSAO
                };

                _conexaoSQL.ObjetoConexao.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi gravar o retorno do pedido: {pedido.CD_PEDIDO} integrado " + ex.Message);
            }
        }

        public List<PedidoBaixasSQL> BuscarPedidosBaixas()
        {
            try
            {
                var sql = @"SELECT 
                         BAIXAS.*
                        ,CLIENTE.NR_CPFCNPJ
						,PARCELAS.DT_VENCIMENTO
                    FROM TBL_VENDAS_PEDIDO_BAIXAS		  BAIXAS
                    INNER JOIN TBL_VENDAS_PEDIDO		  PEDIDO   ON PEDIDO.CD_PEDIDO    = BAIXAS.CD_PEDIDO
					INNER JOIN TBL_VENDAS_PEDIDO_PARCELAS PARCELAS ON PARCELAS.CD_PEDIDO  = BAIXAS.CD_PEDIDO AND PARCELAS.NR_PARCELA = BAIXAS.NR_PARCELA
                    INNER JOIN TBL_ENTIDADES       CLIENTE ON CLIENTE.CD_ENTIDADE = PEDIDO.CD_CLIENTE 
                    WHERE BAIXAS.CD_PEDIDO IN (
                        SELECT INTE.CD_PEDIDO 
                        FROM TBL_PEDIDOS_INTEGRACAO INTE
                        WHERE INTE.CD_PEDIDO = BAIXAS.CD_PEDIDO
					)
					AND BAIXAS.CD_PEDIDO NOT IN (
						SELECT 	BAIXAS_INTE.CD_PEDIDO 
						FROM TBL_PEDIDOS_BAIXAS_INTEGRACAO BAIXAS_INTE
						WHERE BAIXAS_INTE.CD_PEDIDO = BAIXAS.CD_PEDIDO AND BAIXAS_INTE.CD_ITEM = BAIXAS.CD_ITEM
					)";


                return _conexaoSQL.ObjetoConexao.Query<PedidoBaixasSQL>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível buscar as baixas referente aos pedidos para integração " + ex.Message);
            }  
        }

        public void InserirBaixasPedidoIntegradoSQL(PedidoBaixasSQL baixasSQL)
        {
            try
            {
                var sql = @"INSERT INTO TBL_PEDIDOS_BAIXAS_INTEGRACAO_ENVIO
                      (
                        CD_PEDIDO
                       ,CD_ITEM
                       ,DS_ORIGEM_INTEGRACAO
                       ,NR_NOTAFISCAL
                       ,DT_PAGAMENTO
                       ,DT_ENVIO
                      )
                      VALUES
                      (
                        @CD_PEDIDO
                       ,@CD_ITEM
                       ,'SQL_ORACLE'
                       ,@NR_NOTAFISCAL
                       ,@DT_PAGAMENTO
                       ,GETDATE()
                      )";

                var parametros = new
                {
                    CD_PEDIDO = baixasSQL.CD_PEDIDO,
                    CD_ITEM = baixasSQL.CD_ITEM,
                    NR_NOTAFISCAL = baixasSQL.NR_DOCUMENTO,
                    DT_PAGAMENTO = baixasSQL.DT_PAGAMENTO
                };

                _conexaoSQL.ObjetoConexao.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi gravar o retorno da baixa do pedido: {baixasSQL.CD_PEDIDO}, item: {baixasSQL.CD_ITEM} integrado " + ex.Message);
            }
        }
    }
}
