using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoElec : EnginElec
{
    public GameObject prefabparticules;
    private GameObject particules;
    
    public virtual new void Start()
    {
        particules = Instantiate(prefabparticules);
        particules.GetComponent<Transform>().position = transform.position;
        particules.transform.parent = transform;
        particules.GetComponent<ParticleSystem>().enableEmission = false;
    }
    public float consommation;
    [SerializeField]protected bool alimentationSuffisante = false;

    public float GetConsommation()
    {
        if (ON_OffElec)             //Si l'appareil est allumé
            return consommation;
        else
            return 0;
    }

    public void AlimentationSuffisante(bool suffisant)
    {
        //Si on rentre ici c'est que le reseau vient de changer d'état
        alimentationSuffisante = suffisant;

        particules.GetComponent<ParticleSystem>().enableEmission = suffisant;
    }
}
