using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCommand : ICommand<ResponseMovimentacao>
    {
        public string IdContaCorrente { get; set; }

        public string TipoMovimento { get; set; }

        public decimal Valor { get; set; }
    }
}
