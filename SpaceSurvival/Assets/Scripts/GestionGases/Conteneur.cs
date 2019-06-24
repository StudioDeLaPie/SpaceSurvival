using Pression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Conteneur : MonoBehaviour
{
    public bool ON_OFF = false;

    [Space]
    public float nbParticulesOxygene = 0;
    public float nbParticulesGaz = 0;
    public float nbParticulesVapeur = 0;

    [SerializeField] protected float nbTotalParticules;

    [Space]
    [SerializeField] protected float pressure;
    [SerializeField] protected float volume;

    [Space]
    [SerializeField] protected float ratioOxygene;
    [SerializeField] protected float ratioGaz;
    [SerializeField] protected float ratioVapeur;

    public virtual void Initialize()
    {
        CalculationVolume();
        CalculationPressure();
        CalculationRatio();
    }

    protected void CalculationPressure()
    {
        pressure = ((nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur) * PressionGestion.constanteGazParfait * PressionGestion.temperatureEnKelvin) / volume;
    }

    protected abstract void CalculationVolume();

    protected void CalculationRatio()
    {

        nbTotalParticules = nbParticulesOxygene + nbParticulesGaz + nbParticulesVapeur;
        if (nbTotalParticules != 0)
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

    public virtual Gases AspireToutesParticules(float nbParticules)
    {
        CalculationRatio();//On calcul le Ratio de gasses qu'on a
        //Puis on calcul la quantité de chaque gaz qu'on doit transvaser
        float oxygenToGive = (nbParticules * ratioOxygene);
        float gazToGive = (nbParticules * ratioGaz);
        float vapeurToGive = (nbParticules * ratioVapeur);

        Gases result = new Gases(Mathf.Abs(RemoveGas(EGases.Oxygene, oxygenToGive)), Mathf.Abs(RemoveGas(EGases.Gaz, gazToGive)), Mathf.Abs(RemoveGas(EGases.Vapeur, vapeurToGive))); // A retoucher
        CheckParticulesAt0(); //Vérification que des particules sont pas en dessous de 1
        return result;
    }

    public virtual void AddGases(Gases gases)
    {
        
        AddGas(EGases.Oxygene, gases.nbParticulesOxygene);
        AddGas(EGases.Gaz, gases.nbParticulesGaz);
        AddGas(EGases.Vapeur, gases.nbParticulesVapeur);
    }

    public virtual Gases RemoveGases(Gases gases)
    {
        Gases result = new Gases();
        result.nbParticulesOxygene = RemoveGas(EGases.Oxygene, gases.nbParticulesOxygene);
        result.nbParticulesGaz = RemoveGas(EGases.Gaz, gases.nbParticulesGaz);
        result.nbParticulesVapeur = RemoveGas(EGases.Vapeur, gases.nbParticulesVapeur);
        return null;
    }

    #region Fonctions Gas
    /// <summary>
    /// Ajoute un gaz donné au conteneur dans une certaine quantité
    /// </summary>
    /// <param name="gas">Type du gaz à ajouter</param>
    /// <param name="quantity">Nombre de particules à ajouter</param>
    public virtual void AddGas(EGases gas, float quantity)
    {
        switch (gas) //On Selectionne le gas à traiter
        {
            case EGases.Oxygene:
                calculation(ref nbParticulesOxygene, quantity);
                break;
            case EGases.Gaz:
                calculation(ref nbParticulesGaz, quantity);
                break;
            case EGases.Vapeur:
                calculation(ref nbParticulesVapeur, quantity);
                break;
        }
    }

    /// <summary>
    /// Enlève un gaz donné au conteneur dans une certaine quantité
    /// </summary>
    /// <param name="gas">Type du gaz à retirer</param>
    /// <param name="quantity">Nombre de particules à retirer</param>
    /// <returns>Return le nombre de particule qu'il à reussi à retirer</returns>
    public virtual float RemoveGas(EGases gas, float quantity)
    {
        switch (gas) //On Selectionne le gas à traiter
        {
            case EGases.Oxygene:
                return calculation(ref nbParticulesOxygene, -quantity);
            case EGases.Gaz:
                return calculation(ref nbParticulesGaz, -quantity);
            case EGases.Vapeur:
                return calculation(ref nbParticulesVapeur, -quantity);
            default:
                return 0;
        }
    }

    private float calculation(ref float floatPraticules, float quantity)
    {
        float nbParticulesDispo;
        float nbParticulesAdded = 0;

        nbParticulesDispo = floatPraticules;

        if (nbParticulesDispo + quantity >= 0) //Si le resultat de l'opération ne fait pas tomber le nombre de particules en négatif
        {
            floatPraticules += quantity;
            nbParticulesAdded = quantity;
        }
        else //Si on demande plus de particules à retirer qu'il y en a de dispo
        {
            float dif = Mathf.Abs(quantity) - nbParticulesDispo;
            float particulesMaxARetirer = (Mathf.Abs(quantity) - dif);

            floatPraticules = nbParticulesDispo - particulesMaxARetirer;

            nbParticulesAdded = particulesMaxARetirer;
        }

        CalculationPressure();
        CalculationRatio();
        return nbParticulesAdded;
    }
    #endregion

    private void CheckParticulesAt0()
    {
        if (nbParticulesOxygene < 1) nbParticulesOxygene = 0;
        if (nbParticulesGaz < 1) nbParticulesGaz = 0;
        if (nbParticulesVapeur < 1) nbParticulesVapeur = 0;
    }

    public float Ratio(EGases typeGase)
    {
        switch (typeGase)
        {
            case EGases.Gaz:
                return ratioGaz;
            case EGases.Oxygene:
                return ratioOxygene;
            case EGases.Vapeur:
                return ratioVapeur;
        }
        return 0;
    }

    [ContextMenu("CalculAllValues")]
    private void CalculAll()
    {
        Initialize();
    }

    [ContextMenu("FillContener")]
    private void FillContener()
    {
        CalculAll();
        float nbParticuleToFill = (PressionGestion.goalPression * volume) / (PressionGestion.constanteGazParfait * PressionGestion.temperatureEnKelvin);
        if ((ratioOxygene + ratioGaz + ratioVapeur) != 0)
        {
            nbParticulesOxygene = ratioOxygene * nbParticuleToFill;
            nbParticulesGaz = ratioGaz * nbParticuleToFill;
            nbParticulesVapeur = ratioVapeur * nbParticuleToFill;
        }
        else
        {

        }
        CalculAll();
    }

    public Gases GetGases()
    {
        return new Gases(nbParticulesOxygene, nbParticulesGaz, nbParticulesVapeur);
    }

    public float Volume { get => volume; set => volume = value; }
    public float Pressure { get => pressure; set => pressure = value; }
}
