using System.Data.SqlClient;

namespace IntegracaoBancoOracleSQL.Conexoes
{
    public class ConexaoSQL
    {
        private string _stringConexao;
        public SqlConnection _conexao;

        public ConexaoSQL()
        {
            _conexao = new SqlConnection();
            _stringConexao = GetStringConexaoBanco();
            _conexao.ConnectionString = GetStringConexaoBanco();
            //ObjetoConexao = _conexao;
        }

        public string StringConexao
        {
            get { return _stringConexao; }
            set { _stringConexao = value; }
        }

        public SqlConnection ObjetoConexao
        {
            get { return _conexao; }
            set { _conexao = value; }
        }

        public void Conectar()
        {
            _conexao.Open();
        }

        public void Desconectar()
        {
            _conexao.Close();
        }

        private string GetStringConexaoBanco()
        {
            var ip = "";
            var database = "";
            var senha = "";
            return $"Data Source={ip};Initial Catalog={database};User ID=sa;Password={senha}";
        }
    }
}
