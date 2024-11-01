using Dapper;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IDbConnection _connection;

        public MovimentoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task InsertMovimentoAsync(Movimento movimento)
        {
            var sql = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            await _connection.ExecuteAsync(sql, movimento);
        }

        public async Task<decimal> GetSaldoAsync(string idContaCorrente)
        {
            var sqlCredito = "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'C'";
            var sqlDebito = "SELECT SUM(valor) FROM movimento WHERE idcontacorrente = @Id AND tipomovimento = 'D'";

            var totalCredito = await _connection.ExecuteScalarAsync<decimal?>(sqlCredito, new { Id = idContaCorrente }) ?? 0;
            var totalDebito = await _connection.ExecuteScalarAsync<decimal?>(sqlDebito, new { Id = idContaCorrente }) ?? 0;

            return totalCredito - totalDebito;
        }

        public Task<Movimento> GetMovimentoByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }

}
