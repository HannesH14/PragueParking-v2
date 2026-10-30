using PragueParking.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.App;

public class Konfig
{
    public int AntalPlatser { get; set; }
    public List<FordonKonfig> Fordonstyper { get; set; } = new List<FordonKonfig>();
    public static Konfig? LäsKonfig(string filnamn)
    {
        MinaFiler filer = new MinaFiler();
        return filer.LäsJson<Konfig>(filnamn);

    }
}
public class FordonKonfig
{
    public string Namn { get; set; } = "";
    public int AntalPerPlats { get; set; }
}
