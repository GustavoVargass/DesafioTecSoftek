namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public string idmovimento { get; set; }
        public string idcontacorrente { get; set; }
        public string datamovimento { get; set; }
        public string tipomovimento { get; set; }
        public decimal valor { get; set; }
    }
}
