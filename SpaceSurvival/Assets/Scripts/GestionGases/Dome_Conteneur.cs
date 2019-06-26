using Pression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome_Conteneur : Conteneur
{
    private Atmosphere atmosphere;

    public void TurnOnDomeConteneur()
    {
        if (atmosphere == null)
            Init();
        atmosphere.GiveParticulesToDome(this); //Initialisation des particules
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        atmosphere = GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Atmosphere>();
        Initialize();
    }


    private void OnDestroy()
    {
        atmosphere.AddGases(GetGases());
    }

    protected override void CalculationVolume()
    {
        float rayon = transform.localScale.x / 2;
        volume = ((4 * Mathf.PI * Mathf.Pow(rayon, 3)) / 3) / 2; //mètre cube //Calcul pour demisphère
        CalculationPressure();
    }

    /// <summary>
    /// Supprime les gazs du dome pour les ajouter à l'atmosphère
    /// </summary>
    public void RelacheInAtmosphere()
    {
        if (atmosphere == null)
            Init();
        atmosphere.AddGases(GetGases());
        RemoveGases(GetGases());
    }
}
