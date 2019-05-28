using Pression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : Conteneur
{
    public GameObject enfantBackFace; //enfant pour rendre la face arrière

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
        enfantBackFace.SetActive(true);
    }

    private void OnDestroy()
    {
        atmosphere.AddGases(GetGases());
    }
}
