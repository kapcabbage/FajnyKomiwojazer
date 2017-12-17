using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    class Program
    {
        
        static void TestGCLS(Graf graf)
        {
            Stopwatch stopwatch = new Stopwatch();
            string name = "Local Search based on Greedy Cycle.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;

            GreedyCycle baseAlg = new GreedyCycle(graf);
            var results = new double[graf.Wierzcholki.Count];
            var stopwatchResult = new double[graf.Wierzcholki.Count];
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                stopwatch.Reset();
                Graf gc = baseAlg.Solve(i);
                LocalSearch localSearch = new LocalSearch(graf, gc);
                stopwatch.Start();
                Graf ls = localSearch.Solve();
                stopwatch.Stop();
                stopwatchResult[i] = stopwatch.Elapsed.Milliseconds;
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if(results.Max() <= results[i])
                {
                    best = ls;
                }
                //Console.WriteLine(i);
            }
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}, Best Result: {maxIndex}");
            Console.WriteLine($"MinTime: {stopwatchResult.Min()}, AverageTime: {stopwatchResult.Average()}, MaxTime: {stopwatchResult.Max()}");
            Console.WriteLine();
            
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\GCLS{maxIndex}.txt"));
        }

        static void TestRNDLS(Graf graf)
        {
            Stopwatch stopwatch = new Stopwatch();
            string name = "Local Search based on Random Cycle.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;
            RandomCycle baseAlg = new RandomCycle(graf);
            var results = new double[graf.Wierzcholki.Count];
            var stopwatchResult = new double[graf.Wierzcholki.Count];
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                stopwatch.Reset();
                Graf gc = baseAlg.Compute();
                LocalSearch localSearch = new LocalSearch(graf, gc);
                stopwatch.Start();
                Graf ls = localSearch.Solve();
                stopwatch.Stop();
                stopwatchResult[i] = stopwatch.Elapsed.Milliseconds;
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if (results.Max() <= results[i])
                {
                    best = ls;
                }
               // Console.WriteLine(i);
            }
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine($"MinTime: {stopwatchResult.Min()}, AverageTime: {stopwatchResult.Average()}, MaxTime: {stopwatchResult.Max()}");
            Console.WriteLine();
            
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\RNDLS{maxIndex}.txt"));
        }

        static void TestNNLS(Graf graf)
        {
            Stopwatch stopwatch = new Stopwatch();
            string name = "Local Search based on NN.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;


            NearestNeighbour baseAlg = new NearestNeighbour(graf);
            var results = new double[graf.Wierzcholki.Count];
            var stopwatchResult = new double[graf.Wierzcholki.Count];
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                stopwatch.Reset();
                Graf gc = baseAlg.Compute(i);
                LocalSearch localSearch = new LocalSearch(graf, gc);
                stopwatch.Start();
                Graf ls = localSearch.Solve();
                stopwatch.Stop();
                stopwatchResult[i] = stopwatch.Elapsed.Milliseconds;
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if (results.Max() <= results[i])
                {
                    best = ls;
                }
               // Console.WriteLine(i);
            }
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine($"MinTime: {stopwatchResult.Min()}, AverageTime: {stopwatchResult.Average()}, MaxTime: {stopwatchResult.Max()}");
            Console.WriteLine();

            best.SaveToFile(String.Format($@"..\..\..\Visualisation\NNLS{maxIndex}.txt"));
        }

        static void TestGCRLS(Graf graf)
        {
            Stopwatch stopwatch = new Stopwatch();
            string name = "Local Search based on Greedy Cycle with regret.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;


            GreedyCycleWithRegrets baseAlg = new GreedyCycleWithRegrets(graf);
            var results = new double[graf.Wierzcholki.Count];
            var stopwatchResult = new double[graf.Wierzcholki.Count];
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                stopwatch.Reset();
                Graf gc = baseAlg.Solve(i);
                LocalSearch localSearch = new LocalSearch(graf, gc);
                stopwatch.Start();
                Graf ls = localSearch.Solve();
                stopwatch.Stop();
                stopwatchResult[i] = stopwatch.Elapsed.Milliseconds;
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if (results.Max() <= results[i])
                {
                    best = ls;
                }
                //Console.WriteLine(i);
            }
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine($"MinTime: {stopwatchResult.Min()}, AverageTime: {stopwatchResult.Average()}, MaxTime: {stopwatchResult.Max()}");
            Console.WriteLine();

            best.SaveToFile(String.Format($@"..\..\..\Visualisation\GCRLS{maxIndex}.txt"));
        }

        static void TestMLS()
        {
            DAO dao = new DAO();

            string name = "Multiple Local Search.";
            Console.WriteLine($"Starting {name}");
            Graf best = null;
            int iter = 20;
            var results = new double[iter];
            var stopwatchResult = new double[iter];
            Parallel.For(0, iter, new ParallelOptions { MaxDegreeOfParallelism = 8 }, i =>
            {
                Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");
                MultipleLocalSearch mls = new MultipleLocalSearch(graf);
                Console.WriteLine(i);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var computed = mls.Compute();
                if (best == null || (best.GetValueSoFarByEdge() - best.GetDistanceSoFarByEdge()) < (computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge()))
                {
                    best = computed;
                }
                stopwatch.Stop();
                stopwatchResult[i] = stopwatch.Elapsed.TotalMilliseconds;
                results[i] = computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge();
            });
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\MLS.txt"));
            best.CopyCycleToClipboard();
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine($"MinTime: {stopwatchResult.Min()}, AverageTime: {stopwatchResult.Average()}, MaxTime: {stopwatchResult.Max()}");
        }
        static void TestITS()
        {
            DAO dao = new DAO();

            string name = "Iterated Local Search.";
            Console.WriteLine($"Starting {name}");
            Graf best = null;
            int iter = 20;
            var results = new double[iter];
            Parallel.For(0, iter, new ParallelOptions { MaxDegreeOfParallelism = 1 }, i =>
            {
                Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");
                IteratedLocalSearch mls = new IteratedLocalSearch(graf, 1125);
                Console.WriteLine(i);
                var computed = mls.Compute();
                if (best == null || (best.GetValueSoFarByEdge() - best.GetDistanceSoFarByEdge()) < (computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge()))
                {
                    best = computed;
                }
                results[i] = computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge();
            });
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\ITS.txt"));
            best.CopyCycleToClipboard();
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
        }

        static void TestILS(Graf graf)
        {
            string name = "Iterated Local Search.";
            Console.WriteLine($"Starting {name}");
            IteratedLocalSearch mls = new IteratedLocalSearch(graf, 5271);
            Graf best = null;
            best = mls.Compute();
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\ILS.txt"));
            Console.WriteLine($"Best: {best.GetValueSoFarByEdge() - best.GetDistanceSoFarByEdge()}");
        }

        static void TestATS()
        {
            int iterations = 24;
            DAO dao = new DAO();

            Stopwatch stopwatch = new Stopwatch();
            string name = "Adaptive Tabu Search.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;
            var results = new double[iterations];
            var stopwatchResult = new double[iterations];
            Parallel.For(0, iterations, new ParallelOptions { MaxDegreeOfParallelism = 8 }, i => {
                Console.WriteLine(i);
                Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");
                RandomCycle baseAlg = new RandomCycle(graf);
                Graf wstepny = baseAlg.Compute();
                AdaptiveTabuSearch tabuSearch = new AdaptiveTabuSearch(graf, wstepny);
                Graf ts = tabuSearch.Solve();
                results[i] = ts.GetValueSoFarByEdge() - ts.GetDistanceSoFarByEdge();
                if (results.Max() <= results[i])
                {
                    best = ts;
                }
            });
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine();

            best.SaveToFile(String.Format($@"..\..\..\Visualisation\ATS.txt"));
        }

        static void TestEH()
        {
            DAO dao = new DAO();

            string name = "Hybrid Evolutionary.";
            Console.WriteLine($"Starting {name}");
            Graf best = null;
            int iter = 20;
            var results = new double[iter];
            var stopwatchResult = new double[iter];
            var lsAmount = new double[iter];
            //Parallel.For(0, iter, new ParallelOptions { MaxDegreeOfParallelism = 1 }, i =>
            //{
            for (int i = 0; i < 20; i++)
            {
                Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");
                EvoHybrid evo = new EvoHybrid(graf);
                Console.WriteLine(i);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var computed = evo.Compute(1125);
                stopwatch.Stop();
                if (best == null || (best.GetValueSoFarByEdge() - best.GetDistanceSoFarByEdge()) < (computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge()))
                {
                    best = computed;
                }
                lsAmount[i] = evo.LSNumber;
                stopwatchResult[i] = stopwatch.Elapsed.TotalMilliseconds;
                results[i] = computed.GetValueSoFarByEdge() - computed.GetDistanceSoFarByEdge();
            }
            //});
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\EH.txt"));
            best.CopyCycleToClipboard();
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine($"MinTime: {stopwatchResult.Min()}, AverageTime: {stopwatchResult.Average()}, MaxTime: {stopwatchResult.Max()}");
            Console.WriteLine($"MinLS: {lsAmount.Min()}, AverageLS: {lsAmount.Average()}, MaxLS: {lsAmount.Max()}");
        }

        [STAThread]
        static void Main(string[] args)
        {
            DAO dao = new DAO();
            Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");

            //TestGCLS(graf);
            //TestRNDLS(graf);
            //TestNNLS(graf);
            // TestGCRLS(graf);

            //TestEH();
            //TestATS();

            Plotmaker maker = new Plotmaker();
            maker.Instance = graf;
            maker.Path = $@"..\..\..\Visualisation\Graphs.txt";
            maker.SampleSize = 300;
            maker.MakeDemPlots();

            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
