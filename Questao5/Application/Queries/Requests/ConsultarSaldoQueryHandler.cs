using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, ResponseConsultaSaldo>
    {
        private readonly DatabaseConfig databaseConfig;

        public ConsultarSaldoQueryHandler(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ResponseConsultaSaldo> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var contas = connection.Query("SELECT * FROM contacorrente WHERE idcontacorrente='{0}'", request.IdContaCorrente);
            ContaCorrente contaCorrente = contas.FirstOrDefault();


            if (contaCorrente == null)
            {
                throw new BadHttpRequestException("INVALID_ACCOUNT");
            }

            if (contaCorrente.ativo == 0)
            {
                throw new BadHttpRequestException("INACTIVE_ACCOUNT");
            }

            var saldo = connection.Query("SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END) as saldo FROM movimento where idcontacorrente={0}", request.IdContaCorrente);

            return new ResponseConsultaSaldo
            {
                NumeroConta = contaCorrente.numero,
                NomeTitular = contaCorrente.nome,
                DataHoraConsulta = DateTime.Now,
                Saldo = Convert.ToDecimal(saldo)
            };
        }
    }

}
