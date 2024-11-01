using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Queries;
using Questao5.Infrastructure.Resources;


namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentacao")]
        public async Task<IActionResult> Movimentacao([FromBody] MovimentacaoCommand command)
        {
            var resultado = await _mediator.Send(command);

            if (!resultado.IsSuccess)
                return BadRequest(new { Mensagem = resultado.Error });

            return Ok(new { Id = resultado.Value });
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> Saldo(string idContaCorrente)
        {
            var resultado = await _mediator.Send(new ConsultaSaldoQuery(idContaCorrente));

            if (!resultado.IsSuccess)
                return BadRequest(new { Mensagem = resultado.Error });

            return Ok(new
            {
                Numero = idContaCorrente,
                Nome = "Nome do Titular",
                DataHora = DateTime.Now,
                Saldo = resultado.Value
            });
        }
    }
}