using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FajnyKomiwojazer
{
    public class Plotmaker
    {
        private Random _rand = new Random();
        private List<Graf> _results = new List<Graf>();
        private Graf _best;
        private System.IO.StreamWriter _file;

        public String Path
        {
            get;
            set;
        }
        public int SampleSize
        {
            get;
            set;
        }

        public Graf Instance
        {
            get;
            set;
        }
        

        public void MakeDemPlots()
        {
            using (_file = new System.IO.StreamWriter(Path))
            {
                GenerateSolution();
                VerticleSimilarityToAll();
                VerticleSimilarityToBest();
                EdgeSimilarityToAll();
                EdgeSimilarityToBest();
                Console.WriteLine("dun");
            }
        }


        private double VerticleSimilarity(Graf g1, Graf g2)
        {
            var similar = g1.Krawedzie.Select(k => k.Wierzcholek1.Indeks).Intersect(g2.Krawedzie.Select(k => k.Wierzcholek1.Indeks));
            return 2.0d * similar.Count() / (double)(g1.Krawedzie.Count() + g2.Krawedzie.Count());
        }

        private double EdgeSimilarity(Graf g1, Graf g2)
        {
            var similar = g1.Krawedzie.Where(k => g2.Krawedzie.Any(k2 => k2.Equals(k) || 
                    (k2.Wierzcholek2.Indeks == k.Wierzcholek1.Indeks && k2.Wierzcholek1.Indeks == k.Wierzcholek2.Indeks)));
            return 2.0d * similar.Count() / (double)(g1.Krawedzie.Count() + g2.Krawedzie.Count());
        }


        private void VerticleSimilarityToAll()
        {
            List<double> xSeries = new List<double>();
            List<double> ySeries = new List<double>();

            foreach (Graf graf in _results)
            {
                double wynik = _results.Where(g => g != graf).Select(g => VerticleSimilarity(g, graf)).Average();
                xSeries.Add(graf.Wynik);
                ySeries.Add(wynik);
            }


            for (int i = 0; i < xSeries.Count; i++)
            {
                _file.WriteLine($"{xSeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)} {ySeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }
            _file.WriteLine();
        }

        private void VerticleSimilarityToBest()
        {
            List<double> xSeries = new List<double>();
            List<double> ySeries = new List<double>();

            foreach (Graf graf in _results)
            {
                double wynik = VerticleSimilarity(_best, graf);
                xSeries.Add(graf.Wynik);
                ySeries.Add(wynik);
            }


            for (int i = 0; i < xSeries.Count; i++)
            {
                _file.WriteLine($"{xSeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)} {ySeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }
            _file.WriteLine();
        }

        private void EdgeSimilarityToAll()
        {
            List<double> xSeries = new List<double>();
            List<double> ySeries = new List<double>();

            int j = 0;
            Console.WriteLine(j);
            foreach (Graf graf in _results)
            {
                Console.WriteLine(++j);
                double wynik = _results.Where(g => g != graf).Select(g => EdgeSimilarity(g, graf)).Average();
                xSeries.Add(graf.Wynik);
                ySeries.Add(wynik);
            }


            for (int i = 0; i < xSeries.Count; i++)
            {
                _file.WriteLine($"{xSeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)} {ySeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }
            _file.WriteLine();
        }

        private void EdgeSimilarityToBest()
        {
            List<double> xSeries = new List<double>();
            List<double> ySeries = new List<double>();

            foreach (Graf graf in _results)
            {
                double wynik = EdgeSimilarity(_best, graf);
                xSeries.Add(graf.Wynik);
                ySeries.Add(wynik);
            }


            for (int i = 0; i < xSeries.Count; i++)
            {
                _file.WriteLine($"{xSeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)} {ySeries[i].ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }
            _file.WriteLine();
        }



        private void GenerateSolution()
        {
            for (int i = 0; i < SampleSize; i++)
            {
                Console.WriteLine(i);
                Graf clone = Instance.Clone();
                RandomCycle baseAlg = new RandomCycle(clone, _rand);
                Graf random = baseAlg.Compute();
                LocalSearch localSearch = new LocalSearch(clone, random, false);
                Graf ls = localSearch.Solve();
                ls.Wynik = ls.GetScore();
                _results.Add(ls);

                if(_best == null || _best.Wynik < ls.Wynik)
                {
                    _best = ls;
                }
            }
        }
    }
}
