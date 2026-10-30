namespace PragueParking.App;

public class Bil : Fordon
{
    public override string Typ => "Bil";
    public Bil(string regnummer, decimal timPris, int gratisMinuter) : base(regnummer, timPris, gratisMinuter)
    {
      
    }
}
