using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    class Program
    {
        static void TestGCLS(Graf graf)
        {
            string name = "Local Search based on Greedy Cycle.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;

            GreedyCycle baseAlg = new GreedyCycle(graf);
            var results = new double[graf.Wierzcholki.Count];
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                Graf gc = baseAlg.Solve(i);
                LocalSearch localSearch = new LocalSearch(graf, gc);
                Graf ls = localSearch.Solve();
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if(results.Max() <= results[i])
                {
                    best = ls;
                }
                Console.WriteLine(i);
            }
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}, Best Result: {maxIndex}");
            Console.WriteLine();
            
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\GCLS{maxIndex}.txt"));
        }

        static void TestRNDLS(Graf graf)
        {
            string name = "Local Search based on Random Cycle.";
            Console.WriteLine($"Starting {name}");

            Graf best = null;


            RandomCycle baseAlg = new RandomCycle(graf);
            var results = new double[graf.Wierzcholki.Count];
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                Graf gc = baseAlg.Compute();
                LocalSearch localSearch = new LocalSearch(graf, gc);
                Graf ls = localSearch.Solve();
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if (results.Max() <= results[i])
                {
                    best = ls;
                }
                Console.WriteLine(i);
            }
            var maxIndex = results.ToList().IndexOf(results.Max());
            Console.WriteLine($"Results {name}");
            Console.WriteLine($"Min: {results.Min()}, Average: {results.Average()}, Max: {results.Max()}");
            Console.WriteLine();
            
            best.SaveToFile(String.Format($@"..\..\..\Visualisation\RNDLS.txt"));
        }


        [STAThread]
        static void Main(string[] args)
        {
            DAO dao = new DAO();
            Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");

            TestGCLS(graf);
            TestRNDLS(graf);


            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
