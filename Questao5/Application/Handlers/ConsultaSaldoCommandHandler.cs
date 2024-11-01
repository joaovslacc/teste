using MediatR;
using Questao5.Application.Models;
using Questao5.Application.Queries;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Resources;

namespace Questao5.Application.Handlers;

public class ConsultaSaldoCommandHandler : IRequestHandler<ConsultaSaldoQuery, Result<decimal>>
{
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IContaCorrenteRepository _contaCorrenteRepository;

    public ConsultaSaldoCommandHandler(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository)
    {
        _movimentoRepository = movimentoRepository;
        _contaCorrenteRepository = contaCorrenteRepository;
    }

    public async Task<Result<decimal>> Handle(ConsultaSaldoQuery request, CancellationToken cancellationToken)
    {        
        var contaCorrente = await _contaCorrenteRepository.GetContaCorrenteAsync(request.IdContaCorrente);
        if (contaCorrente == null)
        {
            return Result<decimal>.Failure(Messages.InvalidAccount);
        }

        if (!contaCorrente.Ativo)
        {
            return Result<decimal>.Failure(Messages.InactiveAccount);
        }
                
        var saldo = await _movimentoRepository.GetSaldoAsync(request.IdContaCorrente);

        return Result<decimal>.Success(saldo);
    }
}
