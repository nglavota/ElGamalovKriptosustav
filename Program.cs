using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    // Funkcija koja provjerava je li broj prost
    static bool JeProst(int broj)
    {
        if (broj <= 1)
        {
            return false;
        }

        for (int i = 2; i <= Math.Sqrt(broj); i++)
        {
            if (broj % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    // Funkcija za izračunavanje najvećeg zajedničkog djelitelja (gcd)
    static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);

    //ModPow je metoda koja računa a^b mod c 
    static BigInteger Potencija(BigInteger a, BigInteger b, BigInteger c) => BigInteger.ModPow(a, b, c);

    // Funkcija koja provjerava je li broj primitivan korijen mod p
    static bool JePrimitivanKorijen(int num, int p)
    {
        HashSet<int> potencije = new HashSet<int>();
        for (int i = 1; i < p; i++)
        {
            potencije.Add((int)Potencija(num, i, p));
        }

        return potencije.Count == p - 1;
    }

    // Funkcija koja pronalazi sve primitivne korijene za zadani p
    static List<int> PronađiPrimitivneKorijene(int p)
    {
        List<int> primitivniKorijeni = new List<int>();
        for (int g = 2; g < p; g++)
        {
            if (JePrimitivanKorijen(g, p))
            {
                primitivniKorijeni.Add(g);
            }
        }
        return primitivniKorijeni;
    }


    // Funkcija za šifriranje poruke
    static void Šifriraj(string poruka, int p, BigInteger beta, BigInteger alpha, int k, bool brojčaniFormat, out string y1Text, out string y2Text)
    {
        // y1 = alpha^k mod p
        BigInteger y1 = Potencija(alpha, k, p);

        BigInteger x = BigInteger.Parse(poruka);

        // betaK = beta^k mod p
        BigInteger betaK = Potencija(beta, k, p);
        //  (x * beta^k) mod p
        BigInteger y2 = (x * betaK) % p;

        y1Text = y1.ToString();
        y2Text = y2.ToString();

        // Ispisivanje rezultata šifriranja
        Console.WriteLine($"y1 (alfa^k mod p): {y1Text}");
        Console.WriteLine($"y2 (x * beta^k mod p): {y2Text}");
    }

    // Funkcija za dešifriranje poruke
    static string Dešifriraj(string y2Text, int p, int a, string y1Text, bool brojčaniFormat)
    {
        BigInteger y1 = BigInteger.Parse(y1Text);
        BigInteger y2 = BigInteger.Parse(y2Text);

        // Računanje y1^(p-1-a) mod p (y1^(p-1-a) = (y1^a)^(-1))
        BigInteger y1A = Potencija(y1, p - 1 - a, p);

        // Dešifriranje poruke: x = (y2 * (y1^a)^(-1)) mod p
        BigInteger x = (y2 * y1A) % p;

        return x.ToString();
    }

    // Glavna funkcija koja omogućava šifriranje ili dešifriranje poruke
    static void Main()
    {
        string izborAkcije = "";
        // Unos želi li korisnik šifrirati ili dešifrirati poruku 
        while (true)
        {
            Console.Write("Želite li šifrirati ili dešifrirati poruku? ('s' za šifriranje ili 'd' za dešifriranje): ");
            izborAkcije = Console.ReadLine().ToLower();
            if (izborAkcije == "s" || izborAkcije == "d") break;
            else Console.WriteLine("Neispravan unos. Pokušajte ponovo.");
        }

        int p;
        // Unos prostog broja p
        while (true)
        {
            Console.Write("Unesite prost broj p: ");
            try
            {
                p = int.Parse(Console.ReadLine());
                if (p <= 1 || !JeProst(p))
                {
                    throw new FormatException();
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Neispravan unos. Molimo unesite valjani prost broj.");
            }
        }

        // Pronalazak primitivnih korijena za zadani p
        List<int> primitivniKorijeni = PronađiPrimitivneKorijene(p);
        Console.WriteLine("Primitivni korijeni modulo p:");
        for (int i = 0; i < primitivniKorijeni.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {primitivniKorijeni[i]}");
        }

        int izbor;
        // Odabir primitivnog korijena
        while (true)
        {
            Console.Write("Odaberite primitivan korijen (unesite broj koji odgovara korijenu): ");
            if (int.TryParse(Console.ReadLine(), out izbor) && izbor >= 1 && izbor <= primitivniKorijeni.Count)
            {
                break;
            }
            else
            {
                Console.WriteLine("Neispravan unos. Pokušajte ponovo.");
            }
        }

        BigInteger alpha = primitivniKorijeni[izbor - 1];

        if (izborAkcije == "s")
        {
            int a;
            // Unos tajnog ključa a
            while (true)
            {
                Console.Write("Unesite svoj tajni ključ a (1 < a < p-1): ");
                if (int.TryParse(Console.ReadLine(), out a) && a > 1 && a < p - 1)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Neispravan unos. Pokušajte ponovo.");
                }
            }

            BigInteger beta = Potencija(alpha, a, p);
            Console.WriteLine($"Primitivni korijen alfa: {alpha}");
            Console.WriteLine($"Javni ključ beta: {beta}");

            
            string poruka;
            // Unos poruke koju želimo šifrirati
            while (true)
            {
                Console.Write("Unesite poruku: ");
                
                try
                {
                    poruka = Console.ReadLine();

                    bool broj = BigInteger.TryParse(poruka, out BigInteger result); 
                    BigInteger por = BigInteger.Parse(poruka);

                    if (por >= p || broj != true)
                    {
                        throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Neispravan unos. Za jedinstveno šifriranje molim Vas da unesete brojčanu poruku manju od {p}.");
                }
            }

            bool brojčaniFormat = Regex.IsMatch(poruka, @"^\d+$");

            int k;
            // Unos jednokratnog ključa k
            while (true)
            {
                Console.Write("Unesite jednokratni ključ k: ");
                if (int.TryParse(Console.ReadLine(), out k) && k > 0 && k <= p - 1)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Neispravan unos. Pokušajte ponovo.");
                }
            }

            string y1Text, y2Text;
            Šifriraj(poruka, p, beta, alpha, k, brojčaniFormat, out y1Text, out y2Text);

            Console.WriteLine($"Šifrirana poruka: (y1: {y1Text}, y2: {y2Text})");

            // Provjera podudaraju li se šifrirana i dešifrirana poruka
            string dešifriranaPoruka = Dešifriraj(y2Text, p, a, y1Text, brojčaniFormat);
  
        }
        else if (izborAkcije == "d")
        {
            int a;
            // Unos tajnog ključa a za dešifriranje
            while (true)
            {
                Console.Write("Unesite svoj tajni ključ a (1 < a < p-1): ");
                if (int.TryParse(Console.ReadLine(), out a) && a > 1 && a < p - 1)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Neispravan unos. Pokušajte ponovo.");
                }
            }

            BigInteger beta = Potencija(alpha, a, p);
            Console.WriteLine($"Primitivni korijen alfa: {alpha}");
            Console.WriteLine($"Javni ključ beta: {beta}");

            // Unos šifrirane poruke (y1 i y2)
            Console.Write("Unesite šifriranu poruku (y1 y2) (y1 i y2 odvojeni razmakom): ");
            string[] yText = Console.ReadLine().Split();

            // Provjera ispravnosti unosa
            if (yText.Length != 2)
            {
                Console.WriteLine("Neispravan unos. Unesite oba broja (y1 i y2).");
                return;
            }

            string y1Text = yText[0];
            string y2Text = yText[1];

            // Provjera jesu li oba dijela šifrirane poruke brojčana
            bool brojčaniFormat = y1Text.All(char.IsDigit) && y2Text.All(char.IsDigit);

            // Dešifriranje poruke
            string dešifriranaPoruka = Dešifriraj(y2Text, p, a, y1Text, brojčaniFormat);
            Console.WriteLine($"Dešifrirana poruka: {dešifriranaPoruka}");
        }
        else
        {
            Console.WriteLine("Pogrešan unos!");
        }


        Console.ReadKey();
    }
}