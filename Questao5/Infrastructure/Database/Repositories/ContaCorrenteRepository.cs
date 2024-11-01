using Dapper;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDbConnection _connection;

        public ContaCorrenteRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ContaCorrente> GetContaCorrenteAsync(string id)
        {
            var sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @Id";
            return await _connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Id = id });
        }

        public async Task<bool> IsContaAtivaAsync(string id)
        {
            var sql = "SELECT ativo FROM contacorrente WHERE idcontacorrente = @Id";
            return await _connection.ExecuteScalarAsync<int>(sql, new { Id = id }) == 1;
        }
    }

}
