using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    class Program
    {
        static void Main(string[] args)
        {
            DAO dao = new DAO();
            Graf graf = dao.GetGraf("kroA100.tsp", "kroB100.tsp");
            GreedyCycleWithRegrets algGCR = new GreedyCycleWithRegrets(graf);
            NearestNeighbour algNN = new NearestNeighbour(graf);
            GreedyCycle algGC = new GreedyCycle(graf);
            //for (int i = 0; i < graf.Wierzcholki.Count; i++)
            //{
            //    Graf nn = algNN.Compute(i);
            //    Console.WriteLine(i);
            //    Console.WriteLine(nn.GetValueSoFar()-nn.GetDistanceSoFar());
            //}
            //for (int i = 0; i < graf.Wierzcholki.Count; i++)
            //{
            //    Graf nn = algGC.Solve(i);
            //    Console.WriteLine(i);
            //    Console.WriteLine(nn.GetValueSoFarByEdge() - nn.GetDistanceSoFarByEdge());
            //}


            Graf gcr = algGCR.Solve(1);

            gcr.SaveToFile("1.txt");

            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
