using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoElec : EnginElec
{
    //Conso par fixed update
    public float consommation;

    [ShowOnly] public float consoParSec;

    [ContextMenu("■ Refresh ConsoParSec")]
    protected void RefreshConsoParSec()
    {
        consoParSec = consommation / Time.fixedDeltaTime;
    }

    public float GetConsommation()
    {
        if (ON_OffElec)             //Si l'appareil est allumé par le joueur
            return consommation;
        else
            return 0;
    }
}
