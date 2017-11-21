using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class RandomCycle
    {
        private Random Randmizer;
        private Graf ComputedGraf;
        private Graf Graf;

        public RandomCycle(Graf graf)
        {
            this.Graf = graf;
            this.Randmizer = new Random();
        }

        public Graf Compute()
        {
            ComputedGraf = new Graf();
            ComputedGraf.Wierzcholki = new List<Wierzcholek>();
            var initNumber = Randmizer.Next(2, Graf.Wierzcholki.Count);
            for (int i = 0; i<initNumber;i++)
            {
                int node = Randmizer.Next(100);
                while (ComputedGraf.Wierzcholki.Contains(Graf.Wierzcholki[node]))
                {
                    node = Randmizer.Next(100);
                }
                ComputedGraf.AddWiercholek(Graf.Wierzcholki[node]);
            }
            ComputedGraf.AddWiercholek(ComputedGraf.Wierzcholki.First());
            ConvertToEdges();
            ComputedGraf.Wierzcholki = Graf.Wierzcholki;
            return ComputedGraf;
        }

        public void ConvertToEdges()
        {
            for (int i = 0; i < ComputedGraf.Wierzcholki.Count; i++)
            {
                if (i + 1 != ComputedGraf.Wierzcholki.Count)
                {
                    ComputedGraf.AddKrawedz(new Krawedz(ComputedGraf.Wierzcholki[i], ComputedGraf.Wierzcholki[i + 1]));
                }
            }
        }
    }
}
