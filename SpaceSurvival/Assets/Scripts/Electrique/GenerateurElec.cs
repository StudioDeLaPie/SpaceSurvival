using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerateurElec : EnginElec
{
    protected float puissance;

    /// <summary>
    /// Combien produit le générateur maintenant
    /// </summary>
    public float GetProduction()
    {
        if (ON_OffElec)             //Si l'appareil est allumé
            return puissance;
        else
            return 0;
    }
}
