using Pression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conteneur : MonoBehaviour
{
    public float nbParticulesOxygene = 0;
    public float nbParticulesGaz = 0;
    public float nbParticulesVapeur = 0;

    [SerializeField]
    protected float nbTotalParticules;

    [Space]
    [SerializeField]
    protected float pressure;
    [SerializeField]
    private float volume;

    [Space]
    [SerializeField]
    protected float ratioOxygene;
    [SerializeField]
    protected float ratioGaz;
    [SerializeField]
    protected float ratioVapeur;


    protected void CalculationPressure()
    {
        pressure = ((nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur) * PressionGestion.constanteGazParfait * PressionGestion.temperatureEnKelvin) / volume;
    }

    protected virtual void CalculationVolume()
    {
        float rayon = transform.localScale.x / 2;
        volume = ((4 * Mathf.PI * Mathf.Pow(rayon, 3)) / 3) / 2; //mètre cube //Calcul pour demisphère
        CalculationPressure();
    }

    protected void CalculationRatio()
    {

        nbTotalParticules = nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur;
        if(nbTotalParticules != 0)
        {
        ratioOxygene = nbParticulesOxygene / nbTotalParticules;
        ratioGaz = nbParticulesGaz / nbTotalParticules;
        ratioVapeur = nbParticulesVapeur / nbTotalParticules;
        }
        else
        {
            ratioOxygene = 0;
            ratioGaz = 0;
            ratioVapeur = 0;
        }
    }

    public Gases AspireGases(float nbParticules)
    {
        CalculationRatio();//On calcul le Ratio de gasses qu'on a
        //Puis on calcul la quantité de chaque gaz qu'on doit transvaser
        float oxygenToGive = (nbParticules * ratioOxygene);
        float gazToGive = (nbParticules * ratioGaz);
        float vapeurToGive = (nbParticules * ratioVapeur);

        Gases result = new Gases(Mathf.Abs(AddGas(EGases.Oxygene, -oxygenToGive)), Mathf.Abs(AddGas(EGases.Gaz,-gazToGive)), Mathf.Abs(AddGas(EGases.Vapeur,-vapeurToGive))); // A retoucher
        CheckParticulesAt0(); //Vérification que des particules sont pas en dessous de 1
        return result;
    }

    public void AddGases(Gases gases)
    {
        AddGas(EGases.Oxygene ,gases.nbParticulesOxygene);
        AddGas(EGases.Gaz, gases.nbParticulesGaz);
        AddGas(EGases.Vapeur, gases.nbParticulesVapeur);
    }


    public float AddGas(EGases gas, float quantity)
    {
        float nbParticulesDispo;
        switch (gas) //On Selectionne le gas à traiter
        {
            case EGases.Oxygene:
                nbParticulesDispo = nbParticulesOxygene;
                break;
            case EGases.Gaz:
                nbParticulesDispo = nbParticulesGaz;
                break;
            case EGases.Vapeur:
                nbParticulesDispo = nbParticulesVapeur;
                break;
            default:
                nbParticulesDispo = 0;
                break;
        }

        if (nbParticulesDispo + quantity >= 0) //Si le resultat de l'opération ne fait pas tomber le nombre de particules en négatif
        {
            switch (gas)
            {
                case EGases.Oxygene:
                    nbParticulesOxygene += quantity;
                    break;
                case EGases.Gaz:
                    nbParticulesGaz += quantity;
                    break;
                case EGases.Vapeur:
                    nbParticulesVapeur += quantity;
                    break;
                default:
                    break;
            }
            CalculationPressure();
            return quantity;
        }
        else //Si on demande plus de particules à retirer qu'il y en a de dispo
        {
            float dif = Mathf.Abs(quantity) - nbParticulesDispo;
            float particulesMaxARetirer = (Mathf.Abs(quantity) - dif);
            switch (gas)
            {
                case EGases.Oxygene:
                    nbParticulesOxygene = nbParticulesDispo - particulesMaxARetirer;
                    break;
                case EGases.Gaz:
                    nbParticulesGaz = nbParticulesDispo - particulesMaxARetirer;
                    break;
                case EGases.Vapeur:
                    nbParticulesVapeur = nbParticulesDispo - particulesMaxARetirer;
                    break;
                default:
                    break;
            }
            CalculationPressure();
            return particulesMaxARetirer;
        }
    }

    private void CheckParticulesAt0()
    {
        if (nbParticulesOxygene < 1) nbParticulesOxygene = 0;
        if (nbParticulesGaz < 1) nbParticulesGaz = 0;
        if (nbParticulesVapeur < 1) nbParticulesVapeur = 0;
    }

    public float Volume { get => volume; set => volume = value; }

    [ContextMenu("CalculAllValues")]
    private void CalculAll()
    {
        CalculationVolume();
        CalculationPressure();
        CalculationRatio();
    }
}
