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

    protected override void CalculationVolume()
    {
        float rayon = transform.localScale.x / 2;
        volume = ((4 * Mathf.PI * Mathf.Pow(rayon, 3)) / 3) / 2; //mètre cube //Calcul pour demisphère
        CalculationPressure();
    }
}
