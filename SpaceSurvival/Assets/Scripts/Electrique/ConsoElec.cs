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
    [SerializeField] protected bool alimentationSuffisante = false;


    public float GetConsommation()
    {
        if (ON_OffElec)             //Si l'appareil est allumé
            return consommation;
        else
            return 0;
    }

    public virtual void AlimentationSuffisante(bool suffisant)
    {
        //Si on rentre ici c'est que le reseau vient de changer d'état
        alimentationSuffisante = suffisant;
    }

    public bool GetAlimentationSuffisante()
    {
        return alimentationSuffisante;
    }


}
