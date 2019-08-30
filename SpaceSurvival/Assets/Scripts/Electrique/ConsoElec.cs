using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoElec : EnginElec
{
    public float consommation;

    public float GetConsommation()
    {
        if (ON_OffElec)             //Si l'appareil est allumé par le joueur
            return consommation;
        else
            return 0;
    }
}
