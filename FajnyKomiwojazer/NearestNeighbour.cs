using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ComputedGraf.Wierzcholki = new List<Wierzcholek>();
            ComputedGraf.AddWiercholek(Graf.Wierzcholki[index]);
            //while((ComputedGraf.DistanceSoFar*Weight))

            return ComputedGraf;
           
        }

        public Wierzcholek FindNext()
        {
            var distance = 0.0;
            var index = 0;
            foreach (var node in Graf.Wierzcholki.Where(x => !ComputedGraf.Wierzcholki.Contains(x)))
            {
                var last = ComputedGraf.Wierzcholki.LastOrDefault();
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
