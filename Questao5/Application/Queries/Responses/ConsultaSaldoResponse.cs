namespace Questao5.Application.Queries.Responses
{
    public class ConsultaSaldoResponse
    {
        public string NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal Saldo { get; set; }

        public ConsultaSaldoResponse(string numeroConta, string nomeTitular, DateTime dataConsulta, decimal saldo)
        {
            NumeroConta = numeroConta;
            NomeTitular = nomeTitular;
            DataConsulta = dataConsulta;
            Saldo = saldo;
        }
    }
}
