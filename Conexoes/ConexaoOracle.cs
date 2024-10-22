using Oracle.ManagedDataAccess.Client; // Certifique-se de usar a biblioteca correta
using System;
using System.Data;

namespace IntegracaoBancoOracleSQL.Conexoes
{
   
    public class ConexaoOracle : IDisposable
    {
        private string _stringConexao;
        public OracleConnection _conexao;

        public ConexaoOracle()
        {
            _stringConexao = GetStringConexaoBanco();
            _conexao = new OracleConnection(_stringConexao);
        }

        public string StringConexao
        {
            get { return _stringConexao; }
            set { _stringConexao = value; }
        }

        public OracleConnection ObjetoConexao
        {
            get { return _conexao; }
            set { _conexao = value; }
        }

        public void Conectar()
        {
            if (_conexao.State != ConnectionState.Open)
            {
                _conexao.Open();
            }
        }

        public void Desconectar()
        {
            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }
        }

        public void Dispose()
        {
            // Certifique-se de que a conexão esteja fechada antes de descartá-la
            if (_conexao != null)
            {
                if (_conexao.State == ConnectionState.Open)
                {
                    Desconectar();
                }
                _conexao.Dispose();
            }
        }

        private string GetStringConexaoBanco()
        {
            return "User Id=;Password=;Data Source=";
        }
    }

}
