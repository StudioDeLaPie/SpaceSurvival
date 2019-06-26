using Pression;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Atmosphere : Conteneur
{
    public float rayonPlanete = 8000f;

    private void Start()
    {
        CalculationVolume();
        CalculationRatio();
    }

    private void Update()
    {
        CalculationPressure();
    }

    protected override void CalculationVolume()
    {
        float rayon = (rayonPlanete / 60f) + rayonPlanete;
        //Volume de la planète augmenté de 1 soixantième (1/60) - volume de la planète = Volume de l'atmosphère
        Volume = ((4 * Mathf.PI * Mathf.Pow(rayon + (rayon / 60f), 3)) / 3) - ((4 * Mathf.PI * Mathf.Pow(rayon, 3)) / 3); //mètre cube
        CalculationPressure();
    }

    public void GiveParticulesToDome(Dome_Conteneur dome)
    {
        if (Volume == -1) //Si volume n'est pas initialisé, on le fait
            CalculationVolume();

        float taux = dome.Volume / Volume;

        float oxygenToGive = nbParticulesOxygene * taux;
        float gazToGive = nbParticulesGaz * taux;
        float vapeurToGive = nbParticulesVapeur * taux;

        nbParticulesOxygene -= oxygenToGive;
        nbParticulesGaz -= gazToGive;
        nbParticulesVapeur -= vapeurToGive;

        dome.AddGas(EGases.Oxygene, oxygenToGive);
        dome.AddGas(EGases.Gaz, gazToGive);
        dome.AddGas(EGases.Vapeur, vapeurToGive);
    }

}
