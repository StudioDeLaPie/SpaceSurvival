using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    public int nbParticulesOxygene = 0;
    public int nbParticulesGaz = 0;
    public int nbParticulesVapeur = 0;

    [Header("Affichage")]
    [SerializeField]
    private float volume; //DemiSphère
    [SerializeField]
    private float pressure; //DemiSphère

    private void Start()
    {
        CalculationVolume();
    }

    [ContextMenu("CalculationPressure")]
    private void CalculationPressure()
    {
        pressure = ((nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur) * Constantes.constanteGazParfait * Constantes.temperatureEnKelvin) / volume;
        Debug.Log("pression: " +pressure);
    }

    [ContextMenu("CalculationVolume")]
    private void CalculationVolume()
    {
        float rayon = transform.localScale.x / 2;
        volume = ((4 * Mathf.PI * Mathf.Pow(rayon, 3)) / 3 )/2; //mètre cube
        CalculationPressure();
        //Debug.Log("Volume: " + volume);
    }
   

    public bool AddOxygene(int quantity)
    {
        if (nbParticulesOxygene + quantity >= 0)
        {
            nbParticulesOxygene += quantity;
            CalculationPressure();
            return true;
        }
        else
            return false;
    }

    public bool AddGaz(int quantity)
    {
        if (nbParticulesGaz + quantity >= 0)
        {
            nbParticulesGaz += quantity;
            CalculationPressure();
            return true;
        }
        else
            return false;
    }

    public bool AddVapeur(int quantity)
    {
        if (nbParticulesVapeur + quantity >= 0)
        {
            nbParticulesVapeur += quantity;
            CalculationPressure();
            return true;
        }
        else
            return false;
    }
}
