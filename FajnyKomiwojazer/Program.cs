using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DAO dao = new DAO();
            Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");
            GreedyCycleWithRegrets algGCR = new GreedyCycleWithRegrets(graf);
            NearestNeighbour algNN = new NearestNeighbour(graf);
            GreedyCycle algGC = new GreedyCycle(graf);
            RandomCycle rng = new RandomCycle(graf);
            var nnval = new double[graf.Wierzcholki.Count];
            var gcval = new double[graf.Wierzcholki.Count];
            //for (int i = 0; i < graf.Wierzcholki.Count; i++)
            //{
            //    Graf nn = algNN.Compute(i);
            //    nnval[i] = nn.GetValueSoFarByEdge() - nn.GetDistanceSoFarByEdge();
            //    //Console.WriteLine(nnval[i]);

            //}
            //Console.WriteLine(nnval.Max());
            //var maxIndex = nnval.ToList().IndexOf(nnval.Max());
            //var min = nnval.Min();
            //var mean = nnval.Average();
            //Console.WriteLine(min);
            //Console.WriteLine(mean);
            //Console.WriteLine(nnval.Max());

            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                Graf gc = algGC.Solve(i);
                LocalSearch localSearch = new LocalSearch(graf, gc);
                Graf ls = localSearch.Solve();
                gcval[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                Console.WriteLine(i);
            }
            var gcmaxIndex = gcval.ToList().IndexOf(gcval.Max());
            var gcmin = gcval.Min();
            var gcmean = gcval.Average();
            Console.WriteLine(gcmin);
            Console.WriteLine(gcmean);
            Console.WriteLine(gcval.Max());


            //for (int i = 0; i < graf.Wierzcholki.Count; i++)
            //{
            //    Graf gcr = algGCR.Solve(i);
            //    gcval[i] = gcr.GetValueSoFarByEdge() - gcr.GetDistanceSoFarByEdge();
            //}
            //var gcrmaxIndex = gcval.ToList().IndexOf(gcval.Max());
            //var gcrmin = gcval.Min();
            //var gcrmean = gcval.Average();
            //Console.WriteLine(gcrmin);
            //Console.WriteLine(gcrmean);
            //Console.WriteLine(gcval.Max());
            //Console.WriteLine(gcrmaxIndex);

            //for (int i = 0; i < graf.Wierzcholki.Count; i++)
            //{
            //    Graf gcr = rng.Compute();
            //    gcval[i] = gcr.GetValueSoFarByEdge() - gcr.GetDistanceSoFarByEdge();
            //}

            //Graf rnd = rng.Compute();
            //rnd.SaveToFile(@"..\..\..\Visualisation\rng.txt");
            //var rngmaxIndex = gcval.ToList().IndexOf(gcval.Max());
            //var rngmin = gcval.Min();
            //var rngmean = gcval.Average();
            //Console.WriteLine(rngmin);
            //Console.WriteLine(rngmean);
            //Console.WriteLine(gcval.Max());
            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
