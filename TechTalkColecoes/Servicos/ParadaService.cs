using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalkColecoes.Dominio;

namespace TechTalkColecoes.Servicos
{
    public class ParadaService
    {
        private const int QuantidadePorPagina = 10;
        private readonly ICollection<Parada> Paradas;
        public ParadaService(ICollection<Parada> paradas)
        {
            Paradas = paradas;
        }

        public decimal CalcularMediaTempoTodasParada()
        {
            var quantidadeTempos = new List<decimal>();
            foreach (var parada in Paradas)
            {
                ///Thread.Sleep para simular trabalho de um código mais complexo;
                Thread.Sleep(100);
                quantidadeTempos.Add(parada.QuantidadeTempo);
            }

            return quantidadeTempos.Sum() / quantidadeTempos.Count;
        }

        public IEnumerable<Parada> ObterTodasParadasOrdenadasPorCodigo()
        {
            return Paradas.OrderBy(t => t.Codigo).ToList();
        }

        /// <summary>
        /// Exemplo a não seguir. A variável consulta já criou a lista.
        /// </summary>
        public IEnumerable<Parada> OberParadasPaginadas(int paginaAtual)
        {
            var jump = Math.Max(paginaAtual - 1, 0) * QuantidadePorPagina;

            var consulta = ObterTodasParadasOrdenadasPorCodigo();
            var resultado = consulta
                .Skip(jump)
                .Take(QuantidadePorPagina)
                .ToList();
            return resultado;
        }

        public IEnumerable<DateTime> ObterDataInicioTodasParadasMacroPaginada(int paginaAtual)
        {
            var jump = Math.Max(paginaAtual - 1, 0) * QuantidadePorPagina;
            return Paradas
                .Where(t => t.QuantidadeTempo > Parada.QuantidadeTempoParadasMacro)
                .Skip(jump)
                .Take(QuantidadePorPagina)
                .Select(t => t.DataInicio)
                .ToList();
        }

        public IEnumerable<DateTime> ObterDataInicioTodasParadasMacroPaginadaEmParalelo(int paginaAtual)
        {
            var jump = Math.Max(paginaAtual - 1, 0) * QuantidadePorPagina;
            return Paradas
                .AsParallel()
                //.WithCancellation(CancellationToken.None)
                //.WithDegreeOfParallelism(2)
                //.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                //.WithMergeOptions(ParallelMergeOptions.AutoBuffered).ForAll(parada => { })
                .Where(t => t.QuantidadeTempo > Parada.QuantidadeTempoParadasMacro)
                .Skip(jump)
                .Take(QuantidadePorPagina)
                .Select(t => t.DataInicio)
                .ToList();
        }
    }
}
