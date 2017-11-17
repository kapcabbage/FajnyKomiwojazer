using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FajnyKomiwojazer
{
    public class DAO
    {
        public Graf GetGraf(string filename1, string filename2)
        {
            Graf graf = new Graf();

            string[] linie1 = File.ReadAllLines(filename1);
            string[] linie2 = File.ReadAllLines(filename2);

            List<string> listaLinii1 = new List<string>(linie1.Where(l =>
            {
                string[] slowa = l.Split(' ');
                if (slowa.Count() == 0)
                {
                    return false;
                }
                try
                {
                    Double cos = Convert.ToDouble(slowa[0]);
                    return true;
                }
                catch { }
                return false;
            }));

            List<string> listaLinii2 = new List<string>(linie2.Where(l =>
            {
                string[] slowa = l.Split(' ');
                if (slowa.Count() == 0)
                {
                    return false;
                }
                try
                {
                    Double cos = Convert.ToDouble(slowa[0]);
                    return true;
                }
                catch { }
                return false;
            }));

            int wielkosc = Math.Min(listaLinii1.Count, listaLinii2.Count);

            for(int i = 0; i < wielkosc; i++)
            {
                string[] wartosci1 = listaLinii1[i].Split(' ');
                string[] wartosci2 = listaLinii2[i].Split(' ');
                try
                {
                    graf.AddWiercholek(new Wierzcholek()
                    {
                        Index = i,
                        X = Convert.ToDouble(wartosci1[1]),
                        Y = Convert.ToDouble(wartosci1[2]),
                        Wartosc = Convert.ToDouble(wartosci2[1]),
                        Indeks = i
                    });
                }
                catch
                {
                    throw new IOException("Niewłaściwy format pliku wejściowego");
                }
            }

            return graf;
        }
    }
}
