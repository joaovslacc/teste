using NSubstitute;
using Questao5.Application.Commands;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Resources;

namespace TestProject1;

public class MovimentacaoCommandHandlerTests
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly MovimentacaoCommandHandler _handler;

    public MovimentacaoCommandHandlerTests()
    {
        _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        _movimentoRepository = Substitute.For<IMovimentoRepository>();
        _handler = new MovimentacaoCommandHandler(_contaCorrenteRepository, _movimentoRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnInvalidAccount_When_ContaDoesNotExist()
    {
        var command = new MovimentacaoCommand("idRequisicao", "invalidId", 100, TipoMovimento.Credito);
        _contaCorrenteRepository.GetContaCorrenteAsync(command.IdContaCorrente).Returns((ContaCorrente)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.InvalidAccount, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnInactiveAccount_When_ContaIsInactive()
    {
        var command = new MovimentacaoCommand("idRequisicao", "validId", 100, TipoMovimento.Credito);
        _contaCorrenteRepository.GetContaCorrenteAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = false });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.InactiveAccount, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnInvalidValue_When_ValorIsNotPositive()
    {
        var command = new MovimentacaoCommand("idRequisicao", "validId", -50, TipoMovimento.Credito);
        _contaCorrenteRepository.GetContaCorrenteAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.InvalidValue, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnInvalidType_When_TipoMovimentoIsInvalid()
    {
        var command = new MovimentacaoCommand("idRequisicao", "validId", 100, (TipoMovimento)'X');
        _contaCorrenteRepository.GetContaCorrenteAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.InvalidType, result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_AllValidationsPass()
    {
        var command = new MovimentacaoCommand("idRequisicao", "validId", 100, TipoMovimento.Credito);
        _contaCorrenteRepository.GetContaCorrenteAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });
        _movimentoRepository.InsertMovimentoAsync(Arg.Any<Movimento>()).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
