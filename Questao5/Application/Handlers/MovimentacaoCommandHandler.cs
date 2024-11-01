using MediatR;
using Questao5.Application.Commands;
using Questao5.Application.Models;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Resources;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, Result<string>>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public MovimentacaoCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<Result<string>> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetContaCorrenteAsync(request.IdContaCorrente);
            if (contaCorrente == null)
            {
                return Result<string>.Failure(Messages.InvalidAccount);
            }

            if (!contaCorrente.Ativo)
            {
                return Result<string>.Failure(Messages.InactiveAccount);
            }

            if (request.Valor <= 0)
            {
                return Result<string>.Failure(Messages.InvalidValue);
            }

            if (request.TipoMovimento != TipoMovimento.Credito && request.TipoMovimento != TipoMovimento.Debito)
            {
                return Result<string>.Failure(Messages.InvalidType);
            }


            var movimento = new Movimento
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = DateTime.UtcNow,
                TipoMovimento = (char)request.TipoMovimento,
                Valor = request.Valor
            };
                        
            await _movimentoRepository.InsertMovimentoAsync(movimento);
         
            return Result<string>.Success(movimento.IdMovimento);
        }
    }
}
