using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.App;

public class ParkeringsHus
{
    public List<ParkeringsPlats> Platser { get; } = new List<ParkeringsPlats>();
    public ParkeringsHus(int antalPlatser)
    {
        for (int i = 1; i <= antalPlatser; i++)
        {
           Platser.Add(new ParkeringsPlats(i));
           
        }
    }
    public ParkeringsPlats? HittaFordon(string regnr)
    {
        foreach (ParkeringsPlats plats in Platser)
        {
            foreach (Fordon fordon in plats.Fordon)
            {
                if (fordon.Regnummer == regnr)
                {
                    return plats;
                }
            }
        }
        return null;
    }
}
