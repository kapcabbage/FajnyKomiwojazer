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
            GreedyCycleWithRegrets alg = new GreedyCycleWithRegrets(graf);
            NearestNeighbour algNN = new NearestNeighbour(graf);
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                Graf nn = algNN.Compute(i);
                Console.WriteLine(i);
                Console.WriteLine(nn.GetValueSoFar()-nn.GetDistanceSoFar());
            }
            Graf na = algNN.Compute(73);
            Graf solution = alg.Solve(1);
            Console.WriteLine(solution.GetValueSoFarByEdge()- solution.GetDistanceSoFarByEdge());
            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
