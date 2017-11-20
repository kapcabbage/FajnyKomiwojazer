using System.Collections.Generic;
using System.Linq;

namespace FajnyKomiwojazer
{
    public class NearestNeighbour
    {
        private Graf ComputedGraf;
        private Graf Graf;

        public NearestNeighbour(Graf graf)
        {
            this.Graf = graf;
        }

        public Graf Compute(int index)
        {
            ComputedGraf = new Graf();
            ComputedGraf.Wierzcholki = new List<Wierzcholek>();
            ComputedGraf.AddWiercholek(Graf.Wierzcholki[index]);
            var currentValue = 0.0;
            var currentDistance = 0.0;
            var distanceToFirst = 0.0;

            while((currentValue >= currentDistance || currentValue >= distanceToFirst))
            {
                var computedNode = FindNext();
                if (computedNode == null) break;
                currentValue = computedNode.Wartosc;
                currentDistance = Graf.Odleglosc(ComputedGraf.GetLast().Indeks, computedNode.Indeks);
                distanceToFirst = Graf.Odleglosc(ComputedGraf.GetLast().Indeks, ComputedGraf.GetFirst().Indeks);
                ComputedGraf.AddWiercholek(computedNode);
            }
            ComputedGraf.AddWiercholek(ComputedGraf.GetFirst());
            ConvertToEdges();
            ComputedGraf.Wierzcholki = Graf.Wierzcholki;
            return ComputedGraf;
           
        }

        public Wierzcholek FindNext()
        {
            var value = 0.0;
            var index = -1;
            var last = ComputedGraf.GetLast();
            foreach (var node in Graf.Wierzcholki.Where(x => !ComputedGraf.Wierzcholki.Contains(x)))
            {
                var computedDistance = Graf.Odleglosc(last.Indeks, node.Indeks);
                var computedValue = node.Wartosc - computedDistance;
                if (computedValue > value )
                {
                    value = computedValue;
                    index = node.Indeks;
                }
            }
            if (index == -1) return null;

            return Graf.Wierzcholki[index];
        }

        public void ConvertToEdges()
        {
            for(int i =0; i < ComputedGraf.Wierzcholki.Count; i++)
            {
                if (i+1 !=  ComputedGraf.Wierzcholki.Count )
                {
                    ComputedGraf.AddKrawedz(new Krawedz(ComputedGraf.Wierzcholki[i], ComputedGraf.Wierzcholki[i + 1]));
                }
            }
        }
    }
}
