using System.Collections.Generic;
using Spectre.Console;
using PragueParking.Data;

namespace PragueParking.App;

internal class Program
{
    static List<SparadParkering> SkapaSparadParkering(ParkeringsHus phus)
    {
        List<SparadParkering> sparadLista = new List<SparadParkering>();

        foreach (ParkeringsPlats plats in phus.Platser)
        {
            foreach (Fordon fordon in plats.Fordon)
            {
                SparadParkering sparad = new SparadParkering
                {
                    PlatsNr = plats.PlatsNr,
                    Fordonstyp = fordon.Typ,
                    Regnummer = fordon.Regnummer,
                    ParkeringsTid = fordon.ParkeringsTid,
                    TimPris = fordon.TimPris,
                };

                sparadLista.Add(sparad);
            }
        }

        return sparadLista;
    }
    static void SparaParkering(ParkeringsHus phus)
    {
        MinaFiler filer = new MinaFiler();

        List<SparadParkering> lista = SkapaSparadParkering(phus);

        filer.SparaJson("parkering.json", lista);
    }
    static void LäsParkering(ParkeringsHus phus, Prislista prislista)
    {
        MinaFiler filer = new MinaFiler();

        if (!filer.FinnsFil("parkering.json"))
        {
            return;
        }

        List<SparadParkering> lista =
            filer.LäsJson<List<SparadParkering>>("parkering.json")
            ?? new List<SparadParkering>();

        foreach (SparadParkering sparad in lista)
        {
            ParkeringsPlats? plats = null;

            foreach (ParkeringsPlats p in phus.Platser)
            {
                if (p.PlatsNr == sparad.PlatsNr)
                {
                    plats = p;
                    break;
                }
            }

            if (plats == null)
            {
                continue;
            }
            Fordon? fordon = null;

            if (sparad.Fordonstyp == "Bil")
            {
                fordon = new Bil(sparad.Regnummer, sparad.TimPris, prislista.GratisMinuter);
            }
            else if (sparad.Fordonstyp == "MC")
            {
                fordon = new MC(sparad.Regnummer, sparad.TimPris, prislista.GratisMinuter);
            }

            if (fordon != null)
            {
                fordon.ParkeringsTid = sparad.ParkeringsTid;
                plats.Fordon.Add(fordon);
            }
        }
    }



    static void Main(string[] args)
    {
        Konfig konfig = Konfig.LäsKonfig("konfig.json");
        Prislista prislista = Prislista.LäsPrislista();

        ParkeringsHus phus = new ParkeringsHus(konfig.AntalPlatser);
        LäsParkering(phus, prislista);

        Meny meny = new Meny(phus, prislista);
        meny.VisaMeny();
    }



    internal static void ParkeraFordon(ParkeringsHus phus, Prislista prislista)
    {
        string fordon = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
                     .Title("[bold cyan]Välj fordonstyp:[/]")
                     .HighlightStyle(Color.Cyan1)
                     .AddChoices(
                     "Bil",
                     "MC")
                     ).ToLower();

        Console.WriteLine("Ange registreringsnummer:");
        string regnr = (Console.ReadLine() ?? "").ToUpper();

        if (string.IsNullOrWhiteSpace(regnr) || regnr.Length > 10)
        {
            Console.WriteLine("Ogiltigt registreringsnummer!");
            return;
        }
        foreach (ParkeringsPlats plats in phus.Platser)
        {
            foreach (Fordon sammaRegnr in plats.Fordon)
            {
                if (sammaRegnr.Regnummer == regnr)
                {
                    Console.WriteLine($"Fordonet med registreringsnummer {regnr} är redan parkerat på plats: {plats.PlatsNr}");
                    return;
                }
            }
        }
        foreach (ParkeringsPlats plats in phus.Platser)
        {
            if (plats.Fordon.Count == 0)

            {
                if (fordon == "bil")
                {
                    plats.Fordon.Add(
                        new Bil(regnr, prislista.BilPris, prislista.GratisMinuter));
                }
                else
                {
                    plats.Fordon.Add(
                        new MC(regnr, prislista.MCPris, prislista.GratisMinuter));
                }

                Console.WriteLine($"{fordon}#{regnr} är parkerad på plats {plats.PlatsNr}");
                SparaParkering(phus);
                return;
            }
        }

        Console.WriteLine("Parkeringen är full.");
    }
    internal static void FlyttaFordon(ParkeringsHus phus)
    {
        Console.WriteLine("Ange registreringsnummer:");
        string regnr = (Console.ReadLine() ?? "").ToUpper();

        ParkeringsPlats? plats = phus.HittaFordon(regnr);

        if (plats == null)
        {
            Console.WriteLine("Fordonet hittades inte.");
            return;
        }

        Fordon? fordon = null;

        foreach (Fordon f in plats.Fordon)
        {
            if (f.Regnummer == regnr)
            {
                fordon = f;
                break;
            }
        }
        if (fordon == null)
        {
            Console.WriteLine("Fordonet hittades inte.");
            return;
        }

        Console.WriteLine($"Ange ny plats (1-{phus.Platser.Count}):");

        if (!int.TryParse(Console.ReadLine(), out int nyPlats) || nyPlats < 1 || nyPlats > phus.Platser.Count)
        {
            Console.WriteLine("Ogiltigt");
            return;
        }



        ParkeringsPlats nyParkering = phus.Platser[nyPlats - 1];

        if (nyParkering == plats)
        {
            Console.WriteLine("Fordonet står redan på den platsen.");
            return;
        }

        if (nyParkering.Fordon.Count > 0)
        {
            Console.WriteLine("Platsen är upptagen");
            return;
        }

        plats.Fordon.Remove(fordon);
        nyParkering.Fordon.Add(fordon);

        Console.WriteLine($"Fordonet har flyttats till platsnr: {nyPlats}.");
        SparaParkering(phus);
    }

    internal static void SökFordon(ParkeringsHus phus)
    {
        Console.WriteLine("Ange registreringsnummer:");
        string regnr = Console.ReadLine().ToUpper();

        ParkeringsPlats? plats = phus.HittaFordon(regnr);

        if (plats != null)
        {
            Console.WriteLine($"Fordonet med registreringsnummer {regnr} finns på plats: {plats.PlatsNr}");
        }
        else
        {
            Console.WriteLine($"Fordonet med registreringsnummer {regnr} hittades inte.");
        }
    }


    internal static void HämtaFordon(ParkeringsHus phus)
    {
        Console.WriteLine("Ange registreringsnummer:");
        string regnr = Console.ReadLine().ToUpper();

        ParkeringsPlats? plats = phus.HittaFordon(regnr);

        Fordon? fordon = null;

        if (plats != null)
        {
            foreach (Fordon f in plats.Fordon)
            {
                if (f.Regnummer == regnr)
                {
                    fordon = f;
                    break;
                }
            }
        }
        if (plats != null && fordon != null)
        {
            TimeSpan tid = fordon.BeräknaTid();
            Decimal pris = fordon.BeräknaPris();

            Console.WriteLine($"Fordonet med registreringsnummer {regnr} har hämtats ut från plats: {plats.PlatsNr}");
            Console.WriteLine($"Tid parkerad: {tid}\n Total: {pris} CZK");

            plats.Fordon.Remove(fordon);

            SparaParkering(phus);
            return;
        }
        Console.WriteLine($"Fordonet med registreringsnummer {regnr} hittades inte.");
    }
    internal static void Karta(ParkeringsHus phus)
    {
        var karta = new Table();

        karta.Title = new TableTitle("[bold yellow]PARKERINGSÖVERSIKT[/]");
        karta.AddColumn("Plats");
        karta.AddColumn("Status");
        karta.AddColumn("Registreringsnummer");

        foreach (ParkeringsPlats plats in phus.Platser)
        {
            if (plats.Fordon.Count == 0)
            {
                karta.AddRow(
                    plats.PlatsNr.ToString(),
                    "[green]Ledig[/]",
                    "-");
            }
            else
            {
                string fordonsTyp = plats.Fordon[0].Typ;
                string regnummer = plats.Fordon[0].Regnummer;

                karta.AddRow(
                    plats.PlatsNr.ToString(),
                    $"[red]Upptagen ({fordonsTyp})[/]",
                    regnummer);
            }
        }

        AnsiConsole.Write(karta);
    }
}






