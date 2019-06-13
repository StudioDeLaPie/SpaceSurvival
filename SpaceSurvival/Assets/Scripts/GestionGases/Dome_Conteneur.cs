using Pression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome_Conteneur : Conteneur
{

    private Atmosphere atmosphere;

    /// <summary>
    /// Initialise les particules du dome quand le joueur place le dome
    /// </summary>
    public void placed()
    {
        CalculationVolume();
        atmosphere = GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Atmosphere>();
        atmosphere.GiveParticulesToDome(this); //Initialisation des particules
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
}
