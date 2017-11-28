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

        static void TestMLS(Graf graf)
        {
            string name = "Multiple Local Search.";
            Console.WriteLine($"Starting {name}");
            MultipleLocalSearch mls = new MultipleLocalSearch(graf);
            Graf best = null;
            Stopwatch stopwatch = new Stopwatch();
            int iter = 20;
            var times = new int[iter];
            for (int i = 0; i < iter; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                best = mls.Compute();
                stopwatch.Stop();
                times[i] = stopwatch.Elapsed.Milliseconds;
                Console.WriteLine($"Current Elapsed: {times[i]}");
            }

            Console.WriteLine($"Average: {times.Average()}");
        }


        [STAThread]
        static void Main(string[] args)
        {
            DAO dao = new DAO();
            Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");

            TestGCLS(graf);
            TestRNDLS(graf);
            TestNNLS(graf);
            TestGCRLS(graf);

            TestMLS(graf);
            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
