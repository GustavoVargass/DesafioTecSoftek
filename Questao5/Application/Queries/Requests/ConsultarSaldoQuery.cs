namespace Questao5.Application.Queries.Requests
{
    public class ConsultarSaldoQuery : IQuery<ResultadoConsultaSaldo>
    {
        public string IdContaCorrente { get; set; }
    }
}
