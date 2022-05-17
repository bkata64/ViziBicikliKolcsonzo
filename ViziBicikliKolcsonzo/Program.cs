using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ViziBicikliKolcsonzo
{
    class Kolcsonzes
    {
        public string Nev { get; set; }
        public string Azon { get; set; }
        public DateTime Ido1 { get; set; }
        public DateTime Ido2 { get; set; }

        public Kolcsonzes(string sor)
        {
            string[] adatok = sor.Split(';');
            Nev = adatok[0];
            Azon = adatok[1];
            Ido1 = Ido1.AddHours(int.Parse(adatok[2]));
            Ido1 = Ido1.AddMinutes(int.Parse(adatok[3]));
            Ido2 = Ido2.AddHours(int.Parse(adatok[4]));
            Ido2 = Ido2.AddMinutes(int.Parse(adatok[5]));            
        }

        public string fIdok()
        {
            return $"\t{Ido1.Hour:00}:{Ido1.Minute:00}-{Ido2.Hour:00}:{Ido2.Minute:00}";
        }

        public string fAdatok()
        {
            return $"{Ido1.Hour:00}:{Ido1.Minute:00}-{Ido2.Hour:00}:{Ido2.Minute:00} : {Nev}";
        }

        public static Kolcsonzes[] beolvas(string filename)
        {
            string[] beolvasott = File.ReadAllLines(filename);
            Kolcsonzes[] adattomb = new Kolcsonzes[beolvasott.Length - 1];
            for (int i = 0; i < adattomb.Length; i++)
            {
                adattomb[i] = new Kolcsonzes(beolvasott[i + 1]);
            }
            return adattomb;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Kolcsonzes[] kolcsonzesek = Kolcsonzes.beolvas("kolcsonzesek.txt");
            Console.WriteLine("5. feladat: Napi kölcsönzések száma: {0}", kolcsonzesek.Length);

            Console.Write("6. feladat: Kérek egy nevet: ");
            string nev = Console.ReadLine();
            Console.WriteLine($"\t{nev} kölcsönzései:");
            var szemelyes = kolcsonzesek.Where(n => n.Nev == nev);
            foreach (var item in szemelyes)
            {
                Console.WriteLine($"\t{item.fIdok()}");
            }

            Console.Write("7. feladat: Kérem adjon meg egy időpontot óra:perc alakban: ");
            string[] ip = Console.ReadLine().Split(':');
            DateTime idopont = new DateTime();
            idopont = idopont.AddHours(int.Parse(ip[0])).AddMinutes(int.Parse(ip[1]));
            Console.WriteLine("\tA vízen lévő járművek:");
            var vizen = kolcsonzesek.Where(n => (idopont >= n.Ido1 && idopont <= n.Ido2));            
            foreach (var item in vizen)
            {
                Console.WriteLine($"\t{item.fAdatok()}");
            }

            decimal bevetel = kolcsonzesek.Sum(n => Math.Ceiling((decimal)(n.Ido2.Subtract(n.Ido1).TotalMinutes / 30))) * 2400;
            Console.WriteLine($"8. feladat: A napi bevétel: {bevetel} Ft");

            StreamWriter sw = new StreamWriter("F.txt");
            kolcsonzesek.Where(n => n.Azon == "F").ToList().ForEach(x => sw.WriteLine(x.fAdatok()));
            sw.Close();

            SortedDictionary<string, int> stat = new SortedDictionary<string, int>();
            for (int i = 0; i < kolcsonzesek.Length; i++)
            {
                if (!stat.ContainsKey(kolcsonzesek[i].Azon))
                    stat.Add(kolcsonzesek[i].Azon, 1);
                else
                    stat[kolcsonzesek[i].Azon]++;
            }

            foreach (var item in stat)
            {
                Console.WriteLine($"\t{item.Key} - {item.Value}");
            }

            Console.ReadKey();
        }
    }
}
