using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerateurElec : EnginElec
{
    //Génération par ticks
    public float puissance;

    [ShowOnly] public float puissanceParSec;

    [ContextMenu("■ Refresh PuissanceParSec")]
    protected void RefreshPuissanceParSec()
    {
        puissanceParSec = puissance / Time.fixedDeltaTime;
    }

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
