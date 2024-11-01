using MediatR;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoRequest : IRequest
    {
        public string IdRequisicao { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; }
    }

}
