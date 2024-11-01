using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Models;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands
{
    public class MovimentacaoCommand : IRequest<Result<string>>
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
        public string IdRequisicao { get; set; }

        public MovimentacaoCommand(string idRequisicao, string idContaCorrente, decimal valor, TipoMovimento tipoMovimento)
        {
            IdRequisicao = idRequisicao;
            IdContaCorrente = idContaCorrente;
            Valor = valor;
            TipoMovimento = tipoMovimento;
        }
    }
}
