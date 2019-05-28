using Pression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : Conteneur
{
    private Atmosphere atmosphere;

    /// <summary>
    /// Méthode lancé par le constructeur lorsque l'objet est placé
    /// </summary>
    public void ObjectPlaced()
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
}
