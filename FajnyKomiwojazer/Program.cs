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
            
            Graf solution = alg.Solve(1);
            Console.WriteLine("Done, press any key");
            Console.ReadKey();
        }
    }
}
