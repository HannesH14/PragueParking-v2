
namespace PragueParking.App;

public class ParkeringsPlats
{
    public int PlatsNr { get; set; }
    public List<Fordon> Fordon { get; set; } = new List<Fordon>();

    public ParkeringsPlats(int platsnr)
    {
        PlatsNr = platsnr;
    }
}