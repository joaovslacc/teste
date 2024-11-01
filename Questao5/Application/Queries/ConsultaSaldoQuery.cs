using MediatR;
using Questao5.Application.Models;

namespace Questao5.Application.Queries
{
    public class ConsultaSaldoQuery : IRequest<Result<decimal>>
    {
        public string IdContaCorrente { get; set; }

        public ConsultaSaldoQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}
