using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    public class ContaCorrenteController : Controller
    {
        private readonly IMediator mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCommand command)
        {
            try
            {
                var response = await mediator.Send(command);
                return Ok(new { idmovimento = response.IdMovimento });
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{idcontacorrente}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            var query = new ConsultarSaldoQuery
        }
    }
}