using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalkColecoes.Dominio;

namespace TechTalkColecoes.Servicos
{
    public class ParadaConcurrentService
    {
        private readonly IReadOnlyCollection<Parada> _paradas;
        private readonly ParallelOptions _options;

        public ParadaConcurrentService(IReadOnlyCollection<Parada> paradas)
        {
            _paradas = paradas;
            _options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 3
            };
        }

        public decimal CalcularMediaTempoTodasParada()
        {
            var quantidadeTempos = new ConcurrentBag<decimal>();
            Parallel.ForEach(_paradas, _options, parada =>
            {
                ///Thread.Sleep para simular trabalho de um código mais complexo;
                Thread.Sleep(100);
                quantidadeTempos.Add(parada.QuantidadeTempo);
            });

            return quantidadeTempos.Sum() / quantidadeTempos.Count;
        }

        public decimal CalcularMediaTempoTodasParadaComParticao()
        {
            var quantidadeTempos = new ConcurrentBag<decimal>();
            var particao = Partitioner.Create(_paradas);

            Parallel.ForEach(particao, _options, parada =>
            {
                ///Thread.Sleep para simular trabalho de um código mais complexo;
                Thread.Sleep(100);
                quantidadeTempos.Add(parada.QuantidadeTempo);
            });

            return quantidadeTempos.Sum() / quantidadeTempos.Count;
        }
    }
}
