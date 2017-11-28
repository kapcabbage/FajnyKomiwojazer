using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class MultipleLocalSearch
    {
        private Graf Graf;

        public MultipleLocalSearch(Graf graf)
        {
            Graf = graf;
        }

        public Graf Compute()
        {
            Graf computed = new Graf();

            RandomCycle baseAlg = new RandomCycle(Graf);
            var results = new double[Graf.Wierzcholki.Count];
            for (int i = 0; i < Graf.Wierzcholki.Count; i++)
            {
                Graf gc = baseAlg.Compute();
                LocalSearch localSearch = new LocalSearch(Graf, gc);
                Graf ls = localSearch.Solve();
                results[i] = gc.GetValueSoFarByEdge() - gc.GetDistanceSoFarByEdge();
                if (results.Max() <= results[i])
                {
                    computed = ls;
                }
            }
            var maxIndex = results.ToList().IndexOf(results.Max());

            return computed;
        }
    }
}
