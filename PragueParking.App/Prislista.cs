using PragueParking.Data;

namespace PragueParking.App;

public class Prislista
{
    public decimal BilPris { get; set; }
    public decimal MCPris { get; set; }
    public int GratisMinuter { get; set; }
    public static Prislista LäsPrislista()
    {
        Prislista prislista = new Prislista();
        MinaFiler filer = new MinaFiler();
        string[] rader = filer.LäsRader("prislista.txt");


        foreach (string rad in rader)
        {
            string text = rad.Split('#')[0].Trim();
            string[] delar = text.Split('=');

            if (delar.Length != 2)
            {
                continue;
            }

            string namn = delar[0].Trim();
            string värde = delar[1].Trim();

            if (namn == "Bil")
            {
                prislista.BilPris = decimal.Parse(värde);
            }
            else if (namn == "MC")
            {
                prislista.MCPris = decimal.Parse(värde);
            }
            else if (namn == "GratisMinuter")
            {
                prislista.GratisMinuter = int.Parse(värde);
            }
        }
        return prislista;
    }
}