using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.App;

public class SparadParkering
{
    public int PlatsNr { get; set; }
    public string Fordonstyp { get; set; } = "";
    public string Regnummer { get; set; } = "";
    public DateTime ParkeringsTid { get; set; }
    public decimal TimPris { get; set; }
}