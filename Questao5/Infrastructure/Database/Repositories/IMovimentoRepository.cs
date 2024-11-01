using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IMovimentoRepository
    {
        Task<Movimento> GetMovimentoByIdAsync(string id);
        Task InsertMovimentoAsync(Movimento movimento);
        Task<decimal> GetSaldoAsync(string idContaCorrente);
    }
}
