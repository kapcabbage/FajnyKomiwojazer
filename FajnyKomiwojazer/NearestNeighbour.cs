using System.Collections.Generic;
using System.Linq;

namespace FajnyKomiwojazer
{
    public class NearestNeighbour
    {
        private Graf ComputedGraf;
        private Graf Graf;
        private int Weight;

        public NearestNeighbour(Graf graf, int weight )
        {
            this.Graf = graf;
            this.Weight = weight;
        }

        public Graf Compute(int index)
        {
            ComputedGraf = new Graf();
            ComputedGraf.Wierzcholki = new List<Wierzcholek>();
            ComputedGraf.AddWiercholek(Graf.Wierzcholki[index]);
            var currentValue = 0.0;
            var currentDistance = 0.0;
            //while(currentValue < ComputedGraf.GetValueSoFar()-ComputedGraf.GetDistanceSoFar(Weight)&& ComputedGraf.Odleglosc(ComputedGraf.GetLast().Index,ComputedGraf.GetFirst().Index)*5 >)
            while(currentValue >= currentDistance * Weight)
            {
                var computedNode = FindNext();
                currentValue = computedNode.Value;
                currentDistance = Graf.Odleglosc(ComputedGraf.GetFirst().Index, computedNode.Index);
                ComputedGraf.AddWiercholek(computedNode);
            }

            return ComputedGraf;
           
        }

        public Wierzcholek FindNext()
        {
            var distance = 0.0;
            var index = 0;
            foreach (var node in Graf.Wierzcholki.Where(x => !ComputedGraf.Wierzcholki.Contains(x)))
            {
                var last = ComputedGraf.GetLast();
                var computedDistance = Graf.Odleglosc(last.Index, node.Index);
                if (distance < computedDistance)
                {
                    distance = computedDistance;
                    index = node.Index;
                }
            }

            return Graf.Wierzcholki[index];
        }
    }
}
