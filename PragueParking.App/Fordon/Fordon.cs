namespace PragueParking.App;

public class Fordon
{
    public string Regnummer { get; set; }
    public DateTime ParkeringsTid { get; set; }
    public TimeSpan BeräknaTid()
    {
        return DateTime.Now - ParkeringsTid;
    }
    public decimal TimPris { get; set; }
    public int GratisMinuter { get; set; }
    public virtual string Typ => "Fordon";
    public decimal BeräknaPris()
    {
        TimeSpan tid = BeräknaTid();

        if(tid.TotalMinutes < GratisMinuter)
        {
            return 0m;
        }

        double timmar = Math.Ceiling(tid.TotalHours);
        return (decimal)timmar * TimPris;
    }
    public Fordon(string regnummer, decimal timPris, int gratisMinuter)
    {
        Regnummer = regnummer;
        TimPris = timPris;
        ParkeringsTid = DateTime.Now;
        GratisMinuter = gratisMinuter;
    }
}
