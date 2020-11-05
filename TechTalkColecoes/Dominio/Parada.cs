using System;

namespace TechTalkColecoes.Dominio
{
    public class Parada
    {
        public const int QuantidadeTempoParadasMacro = 10;
        public Parada(int codigo, DateTime dataInicio, decimal quantidadeTempo)
        {
            Codigo = codigo;
            DataInicio = dataInicio;
            QuantidadeTempo = quantidadeTempo;
            DataFim = DataInicio.AddMinutes((double)quantidadeTempo);
        }

        public int Codigo { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public decimal QuantidadeTempo { get; private set; }
    }
}
