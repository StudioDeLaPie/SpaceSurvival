using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoElec : EnginElec
{
    public virtual new void Start()
    {
        base.Start();
    }
    public float consommation;


    public float GetConsommation()
    {
        if (ON_OffElec)             //Si l'appareil est allumé
            return consommation;
        else
            return 0;
    }
}
