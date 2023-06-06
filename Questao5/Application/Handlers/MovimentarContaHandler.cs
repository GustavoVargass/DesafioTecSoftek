using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, ResponseMovimentacao>
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentarContaHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ResponseMovimentacao> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var contas = connection.Query("SELECT * FROM contacorrente WHERE idcontacorrente='{0}'", request.IdContaCorrente);
            ContaCorrente contaCorrente = contas.FirstOrDefault();

            if (contaCorrente == null)
                throw new BadHttpRequestException("INVALID_ACCOUNT");

            if (contaCorrente.ativo == 0)
                throw new BadHttpRequestException("INACTIVE_ACCOUNT");

            if (request.Valor <= 0)
                throw new BadHttpRequestException("INVALID_VALUE");

            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                throw new BadHttpRequestException("INVALID_TYPE");

            var movimentacao = new Movimento
            {
                idmovimento = Guid.NewGuid().ToString(),
                idcontacorrente = request.IdContaCorrente,
                datamovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                tipomovimento = request.TipoMovimento,
                valor = request.Valor
            };

            string insert = "INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)" +
                " VALUES(@idmovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor)";
            using (SqliteCommand insertCommand = new SqliteCommand(insert, connection))
            {
                insertCommand.Parameters.AddWithValue("@idmovimento", movimentacao.idmovimento);
                insertCommand.Parameters.AddWithValue("@idcontacorrente", movimentacao.idcontacorrente);
                insertCommand.Parameters.AddWithValue("@datamovimento", movimentacao.datamovimento);
                insertCommand.Parameters.AddWithValue("@tipomovimento", movimentacao.tipomovimento);
                insertCommand.Parameters.AddWithValue("@valor", movimentacao.valor);

            }

            return new ResponseMovimentacao
            {
                IdMovimento = movimentacao.idmovimento
            };
        }
    }
}
