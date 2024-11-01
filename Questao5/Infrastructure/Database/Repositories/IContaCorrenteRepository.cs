using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetContaCorrenteAsync(string id);
    }
}
