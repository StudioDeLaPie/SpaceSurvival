using Pression;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Atmosphere : MonoBehaviour
{
    public float rayonPlanete = 8000f;

    [Space]
    public int nbParticulesOxygene = 0;
    public int nbParticulesGaz = 0;
    public int nbParticulesVapeur = 0;
    private float nbTotalParticules;

    [Space]
    public float ratioOxygene;
    public float ratioGaz;
    public float ratioVapeur;

    [Space]
    [SerializeField]
    private float pressure;
    [SerializeField]
    private float volume;

    private void Start()
    {
        CalculationVolume();
        calculationRatio();
    }

    [ContextMenu("CalculationPressure")]
    private void CalculationPressure()
    {
        pressure = ((nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur) * PressionGestion.constanteGazParfait * PressionGestion.temperatureEnKelvin) / volume;
    }

    [ContextMenu("CalculationVolume")]
    private void CalculationVolume()
    {
        float rayon = (rayonPlanete / 60f) + rayonPlanete;
        volume = ((4 * Mathf.PI * Mathf.Pow(rayon, 3)) / 3); //mètre cube
        Debug.Log("Volume atmo: " + volume);
        CalculationPressure();
    }

    [ContextMenu("Radom Particules")]
    private void RandomParticules()
    {
        nbParticulesOxygene = Aleatoire.AleatoireBetween(0, 255) *100;
        nbParticulesGaz = Aleatoire.AleatoireBetween(0, 255) * 100;
        nbParticulesVapeur = Aleatoire.AleatoireBetween(0, 255) * 100;
    }

    [ContextMenu("Ratio")]
    private void calculationRatio()
    {

        nbTotalParticules = nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur;
        //Debug.Log(nbTotalParticules);

        ratioOxygene = nbParticulesOxygene / nbTotalParticules;
        ratioGaz = nbParticulesGaz / nbTotalParticules;
        ratioVapeur = nbParticulesVapeur / nbTotalParticules;
    }

    public void GiveParticulesToDome(Dome dome)
    {
        float taux = dome.Volume / volume;

        float oxygenToGive = nbParticulesOxygene * taux;
        float gazToGive = nbParticulesGaz * taux;
        float vapeurToGive = nbParticulesVapeur * taux;

        nbParticulesOxygene -= (int)oxygenToGive;
        nbParticulesGaz -= (int)gazToGive;
        nbParticulesVapeur -= (int)vapeurToGive;

        dome.AddOxygene((int)oxygenToGive);
        dome.AddGaz((int)gazToGive);
        dome.AddVapeur((int)vapeurToGive);
    }
}
