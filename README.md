# IntegracaoBancoOracleSQL

## Descrição
O projeto **IntegracaoBancoOracleSQL** tem como objetivo integrar dados de clientes e pedidos de venda entre dois bancos de dados distintos: um SQL Server e um Oracle. Ele realiza a sincronização de dados entre as duas plataformas, garantindo que as informações estejam sempre atualizadas em ambos os sistemas.

## Funcionalidades
- **Sincronização de Clientes**: Realiza a transferência dos dados de clientes entre o banco SQL Server e o banco Oracle, garantindo consistência entre as bases.
- **Sincronização de Pedidos de Venda**: Integra pedidos de venda do banco SQL Server com o Oracle, mantendo ambos os bancos atualizados.
- **Log de Operações**: Gera logs detalhados de cada operação de integração, facilitando a auditoria e a detecção de erros.
- **Tratamento de Erros**: Captura e trata possíveis exceções durante o processo de integração, garantindo a estabilidade do sistema.

## Requisitos
- **.NET 6.0 ou superior** (C#)
- **SQL Server** versão 2016 ou superior
- **Oracle Database** versão 19c ou superior

### Pacotes NuGet:
- `Dapper`
- `Oracle.ManagedDataAccess`
- `System.Data.SqlClient`
- `Newtonsoft.Json` (opcional, caso haja uso de serialização JSON)
