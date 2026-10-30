namespace PragueParking.App;
public class MC : Fordon
{
    public override string Typ => "MC";
    public MC(string regnummer, decimal timPris, int gratisMinuter) : base (regnummer, timPris, gratisMinuter)
    {      
    }
  
}
