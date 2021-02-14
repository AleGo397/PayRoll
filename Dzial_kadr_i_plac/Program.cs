using System;

namespace PayRoll
{
    

    abstract class Pracownik
    {
        public int nr;
        public string imie;
        public string nazwisko;
        public string stanowisko;
        public int stawka;
        public int dni_przepracowane;
        public int dni_zwolnienia;
        public int stawka_nagodzine;

        public Pracownik(int number,string name, string surname, int work_days, int free_days, int salary)
        {
            nr = number;
            imie = name;
            nazwisko = surname;
            dni_przepracowane = work_days;
            dni_zwolnienia = free_days;
            stawka = salary;
        }
        ~Pracownik(){}
        public void wypisz()
        {
            Console.WriteLine(nr + ". " + imie + " " + nazwisko);
        }

        public void wypiszImieNazwisko()
        {
            Console.Write(imie + " " + nazwisko);
        }
        public void rozliczenie(int premia_uznaniowa)
        {
            DateTime dt = DateTime.Now;             // aktualna data 
            string n = Environment.NewLine;
            string bufor = "--------------------------------------";
            string month = "Wynagrodzenie za " + dt.ToString("MMMM");      // aktualny miesiac
            string net = "Wynagrodzenie podstawowe netto: ";
            string premia = "Premia uznaniowa: ";
            string br = "Brutto: ";
            string podatek = "Podatek dochodowy: ";
            string zus = "Składki ZUS: ";
            string wyplata = "Łącznie do wypłaty na konto: ";
            string pln = " PLN";

            double netto = this.stawka;
            double brutto = ((double)this.stawka * 0.23 + (double)this.stawka);

            if (dni_zwolnienia > 0)
            {                                                                       // jezeli pracownik jest na zwolnieniu to dostaje połowe premi 
                premia_uznaniowa = premia_uznaniowa / 2;     

                netto = netto - ((netto / 20) * dni_zwolnienia * 0.2);          //- jeżeli pracownik przebywał na zwolnieniu lekarskim, wówczas dni zwolnienia są liczone razy 80% (0.8) - czyli wynagroznie jest pomniejszane.
            }                                                                   // pomniejszamy o odwrotność

            Console.WriteLine(bufor);
            this.wypiszImieNazwisko();
            Console.WriteLine(n + month + n + net + netto.ToString("F2") + pln + n + premia + premia_uznaniowa + pln + n + br + brutto.ToString("F2") + pln + n + "W tym:" + n + podatek + (brutto * 0.05).ToString("F2") + pln + n + zus + (brutto * 0.2).ToString("F2") + pln + n + wyplata + (netto + premia_uznaniowa).ToString("F2") + pln + n + bufor);
        }
        public abstract void wypisz_szegoly();
    }

     class PracownikFizyczny : Pracownik
    {
        public PracownikFizyczny(int number, string name, string surname, int work_days, int free_days, int salary)
            : base (number,name,surname,work_days,free_days,salary)
        {
            this.stawka_nagodzine = salary;
            this.stawka = this.stawka_nagodzine * (this.dni_przepracowane * 8);          // pracownicy fizyczni mają stawkę godzinową (wynagrodzenie jest iloczynem przepracowanych godzin x stawka)
            this.stanowisko = "Pracownik Fizyczny";
        }
        ~PracownikFizyczny() { }

        public override void wypisz_szegoly()
        {
            string n = Environment.NewLine;
            Console.WriteLine(n + imie + " " + nazwisko + n + "Stanowisko: " + stanowisko + n + "Stawka: " + stawka_nagodzine + " PLN/h" + n + "Ilość dni przepracowanych: " + dni_przepracowane + n + "Ilość dni na zwolnieniu: " + dni_zwolnienia);
        }

    }

    class PracownikUmysłowy : Pracownik
    {

        public PracownikUmysłowy(int number, string name, string surname, int work_days, int free_days, int salary)
                        : base(number, name, surname, work_days, free_days, salary)
        {
            this.stanowisko = "Pracownik Umysłowy";
        }
        ~PracownikUmysłowy() { }
        public override void wypisz_szegoly()
        {
            string n = Environment.NewLine;
            Console.WriteLine(n + imie + " " + nazwisko + n + "Stanowisko: " + stanowisko + n + "Stawka: " + stawka + " PLN" + n + "Ilość dni przepracowanych: " + dni_przepracowane + n + "Ilość dni na zwolnieniu: " + dni_zwolnienia);
        }


    }



    class Program
    {

        static void Main(string[] args)
        {
            string[] imiona = { "Ola", "Michał", "Wojtek", "Adam", "Teresa", "Marcin", "Anna", "Monika", "Damian", "Kamil" };
            string[] nazwiska = { "Nowak", "Kowal", "Kuc", "Antosiak", "Tkacz", "Leja", "Kula", "Majdan", "Polak", "Sasin" };
            Random rnd = new Random();
            Pracownik[] pracownicy = new Pracownik[10];
            string n = Environment.NewLine;

            for(int i = 0; i <10; i++)                  // dodajemy pracowników do tablicy , oraz ustawiamy im losowe parametry
            {
                int w_pracy = rnd.Next(20);
                int na_zwolnieniu = 20 - w_pracy;
                if (i < 5)
                    pracownicy[i] = new PracownikFizyczny(i + 1, imiona[i], nazwiska[i], w_pracy, na_zwolnieniu, rnd.Next(15, 28));
                else
                    pracownicy[i] = new PracownikUmysłowy(i + 1, imiona[i], nazwiska[i], w_pracy, na_zwolnieniu, rnd.Next(3000, 6000));     // - pozostali pracownicy mają stałą pensję miesięczną

            }

            Console.WriteLine("DZIAŁ KADR I PŁAC" + n + n + "Lista pracowników firmy:" + n);

            for (int i = 0; i < 10; i++)
            {
                pracownicy[i].wypisz();
            }

            Console.WriteLine(n + "Prosze podać numer pracownika: " + n);

            string indeks = Console.ReadLine();

            pracownicy[int.Parse(indeks)-1].wypisz_szegoly();


            Console.Write(n + "Prosze podać wysokość premii dla pracownika: ");
            pracownicy[int.Parse(indeks) - 1].wypiszImieNazwisko();
            Console.WriteLine();

            string premia = Console.ReadLine();

            pracownicy[int.Parse(indeks) - 1].rozliczenie(int.Parse(premia));

        }
    }
}
