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
        GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Atmosphere>().GiveParticulesToDome(this); //Initialisation des particules
        Initialize();
    }
}
