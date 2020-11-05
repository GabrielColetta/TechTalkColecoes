using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TechTalkColecoes.Dominio;
using TechTalkColecoes.Servicos;

namespace TechTalkColecoes
{
    static class Program
    {
        private static readonly IEnumerable<Parada> ParadasExemplo = new List<Parada>(20)
            {
                new Parada(1, DateTime.Now, 60),
                new Parada(2, DateTime.Now, 14),
                new Parada(3, DateTime.Now, 26),
                new Parada(4, DateTime.Now, 10),
                new Parada(5, DateTime.Now, 12),
                new Parada(6, DateTime.Now, 18),
                new Parada(7, DateTime.Now, 20),
                new Parada(8, DateTime.Now, 13),
                new Parada(9, DateTime.Now, 22),
                new Parada(10, DateTime.Now, 44),
                new Parada(11, DateTime.Now, 60),
                new Parada(12, DateTime.Now, 14),
                new Parada(13, DateTime.Now, 26),
                new Parada(14, DateTime.Now, 10),
                new Parada(15, DateTime.Now, 12),
                new Parada(16, DateTime.Now, 18),
                new Parada(17, DateTime.Now, 20),
                new Parada(18, DateTime.Now, 13),
                new Parada(19, DateTime.Now, 22),
                new Parada(20, DateTime.Now, 44)
            };

        public static void Main(string[] args)
        {
            Console.WriteLine("Escola o exemplo:");
            Console.WriteLine("[0] Exemplo de coleções thread-safe");
            Console.WriteLine("[1] Exemplo de concorrência com e sem partição");
            Console.WriteLine("[2] Exemplo Parallel LINQ que vai ser pior");
            Console.WriteLine("[3] Exemplo Parallel LINQ que vai ser melhor");

            var escolha = Console.ReadKey();
            Console.WriteLine(Environment.NewLine);
            switch (escolha.Key)
            {
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    ExemploConcorrencia();
                    break;
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    ExemploConcorrenciaComSemParticao();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    ExemploParalelismoPior();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    ExemploParalelismoMelhor();
                    break;
                default:
                    break;
            }
            Console.ReadKey();
        }

        private static void ExemploConcorrencia()
        {
            var paradaService = new ParadaService(ParadasExemplo as ICollection<Parada>);
            var watch = Stopwatch.StartNew();
            var resultado = paradaService.CalcularMediaTempoTodasParada();
            watch.Stop();
            Console.WriteLine($"Resultado: {resultado}, Tempo sincrono: {watch.ElapsedMilliseconds}ms");

            var paradaConcurrentService = new ParadaConcurrentService(ParadasExemplo as IReadOnlyCollection<Parada>);
            watch.Restart();
            resultado = paradaConcurrentService.CalcularMediaTempoTodasParada();
            watch.Stop();
            Console.WriteLine($"Resultado: {resultado}, Tempo em paralelo: {watch.ElapsedMilliseconds}ms");
        }

        private static void ExemploConcorrenciaComSemParticao()
        {
            var paradaConcurrentService = new ParadaConcurrentService(ParadasExemplo as IReadOnlyCollection<Parada>);
            var watch = Stopwatch.StartNew();
            var resultado = paradaConcurrentService.CalcularMediaTempoTodasParada();
            watch.Stop();
            Console.WriteLine($"Resultado: {resultado}, Tempo sem partição: {watch.ElapsedMilliseconds}ms");

            watch.Restart();
            resultado = paradaConcurrentService.CalcularMediaTempoTodasParadaComParticao();
            watch.Stop();
            Console.WriteLine($"Resultado: {resultado}, Tempo com partição: {watch.ElapsedMilliseconds}ms");
        }

        private static void ExemploParalelismoPior()
        {
            var paradaService = new ParadaService(ParadasExemplo as ICollection<Parada>);

            var watch = Stopwatch.StartNew();
            var resultado = paradaService.ObterDataInicioTodasParadasMacroPaginadaEmParalelo(1);
            watch.Stop();
            Console.WriteLine($"Resultado: {resultado.Count()}, Tempo em paralelo: {watch.ElapsedMilliseconds}ms");

            watch.Restart();
            resultado = paradaService.ObterDataInicioTodasParadasMacroPaginada(1);
            watch.Stop();
            Console.WriteLine($"Resultado: {resultado.Count()}, Tempo sincrono: {watch.ElapsedMilliseconds}ms");
        }

        private static void ExemploParalelismoMelhor()
        {
            var valores = Enumerable.Range(0, 10);
            static bool processoComplexo(int t)
            {
                ///Thread.Sleep para simular trabalho de um código mais complexo;
                Thread.Sleep(100);
                return t % 2 == 0;
            }

            var watch = Stopwatch.StartNew();
            var resultado = valores
                .Where(processoComplexo)
                .ToList();

            watch.Stop();
            Console.WriteLine($"Resultados: {string.Join(", ", resultado)}, Tempo sincrono: {watch.ElapsedMilliseconds}ms");

            watch.Restart();
            
            resultado = valores
                .AsParallel()
                .Where(processoComplexo)
                .ToList();
            watch.Stop();
            Console.WriteLine($"Resultados: {string.Join(", ", resultado)}, Tempo em paralelo: {watch.ElapsedMilliseconds}ms");
        }
    }
}
