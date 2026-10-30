using Spectre.Console;

namespace PragueParking.App;

public class Meny
{
    private ParkeringsHus phus;
    private Prislista prislista;
    public Meny(ParkeringsHus phus, Prislista prislista)
    {
        this.phus = phus;
        this.prislista = prislista;
    }
    public void VisaMeny()
    {
        string val = "";

        while (val != "7")
        {
            val = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
                 .Title("[bold cyan]PRAGUE PARKING[/]")
                 .HighlightStyle(Color.Cyan1)
                 .AddChoices(
            "1.Parkera fordon",
            "2. Flytta fordon",
            "3. Sök efter fordon",
            "4. Karta över P-Hus",
            "5. Hämta ut fordon",
            "6. Prislista",
            "7. Avsluta"));

            val = val.Substring(0, 1);

            switch (val)
            {
                case "1":
                    Program.ParkeraFordon(phus, prislista);
                    break;

                case "2":
                    Program.FlyttaFordon(phus);
                    break;

                case "3":
                    Program.SökFordon(phus);
                    break;

                case "4":
                    Program.Karta(phus);
                    break;

                case "5":
                    Program.HämtaFordon(phus);
                    break;


                case "6":
                    prislista = Prislista.LäsPrislista();
                    Console.WriteLine("  ---PARKERINGSPRISER---\n");
                    Console.WriteLine($"Bil: {prislista.BilPris} CZK per påbörjad timme");
                    Console.WriteLine($"MC: {prislista.MCPris} CZK per påbörjad timme");
                    Console.WriteLine($"Gratis: {prislista.GratisMinuter} minuter");
                    break;

                case "7":
                    Console.WriteLine("Programmet avslutas");
                    break;
            }
        }
    }
}
