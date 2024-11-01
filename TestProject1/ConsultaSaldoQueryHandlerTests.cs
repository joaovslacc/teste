using Moq;
using Xunit;
using Questao5.Application.Queries;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Application.Models;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Resources;

namespace TestProject1
{
    public class ConsultaSaldoQueryHandlerTests
    {
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepository;
        private readonly Mock<IMovimentoRepository> _movimentoRepository;
        private readonly ConsultaSaldoCommandHandler _handler;

        public ConsultaSaldoQueryHandlerTests()
        {
            _contaCorrenteRepository = new Mock<IContaCorrenteRepository>();
            _movimentoRepository = new Mock<IMovimentoRepository>();
            _handler = new ConsultaSaldoCommandHandler(_movimentoRepository.Object, _contaCorrenteRepository.Object);
        }

        [Fact]
        public async Task Handle_ContaInexistente_ReturnsInvalidAccount()
        {
            var query = new ConsultaSaldoQuery("invalid_id");
            _contaCorrenteRepository.Setup(repo => repo.GetContaCorrenteAsync("invalid_id"))
                .ReturnsAsync((ContaCorrente)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(Messages.InvalidAccount, result.Error);
        }

        [Fact]
        public async Task Handle_ContaInativa_ReturnsInactiveAccount()
        {
            var query = new ConsultaSaldoQuery("inactive_id");
            var contaCorrente = new ContaCorrente { IdContaCorrente = "inactive_id", Ativo = false };
            _contaCorrenteRepository.Setup(repo => repo.GetContaCorrenteAsync("inactive_id"))
                .ReturnsAsync(contaCorrente);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(Messages.InactiveAccount, result.Error);
        }

        [Fact]
        public async Task Handle_ContaValidaSemMovimentacao_ReturnsSaldoZero()
        {
            var query = new ConsultaSaldoQuery("valid_id");
            var contaCorrente = new ContaCorrente { IdContaCorrente = "valid_id", Ativo = true };
            _contaCorrenteRepository.Setup(repo => repo.GetContaCorrenteAsync("valid_id"))
                .ReturnsAsync(contaCorrente);
            _movimentoRepository.Setup(repo => repo.GetSaldoAsync("valid_id"))
                .ReturnsAsync(0m);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(0m, result.Value);
        }

        [Fact]
        public async Task Handle_ContaValidaComMovimentacao_ReturnsSaldo()
        {
            var query = new ConsultaSaldoQuery("valid_id");
            var contaCorrente = new ContaCorrente { IdContaCorrente = "valid_id", Ativo = true };
            _contaCorrenteRepository.Setup(repo => repo.GetContaCorrenteAsync("valid_id"))
                .ReturnsAsync(contaCorrente);
            _movimentoRepository.Setup(repo => repo.GetSaldoAsync("valid_id"))
                .ReturnsAsync(100m);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(100m, result.Value);
        }
    }
}
