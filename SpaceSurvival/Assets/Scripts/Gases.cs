using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gases 
{
    public float nbParticulesOxygene = 0;
    public float nbParticulesGaz = 0;
    public float nbParticulesVapeur = 0;

    public Gases(float nbParticulesOxygene, float nbParticulesGaz, float nbParticulesVapeur)
    {
        this.nbParticulesOxygene = nbParticulesOxygene;
        this.nbParticulesGaz = nbParticulesGaz;
        this.nbParticulesVapeur = nbParticulesVapeur;
    }

    /// <summary>
    /// Gases contenant 0 particules
    /// </summary>
    public Gases()
    {
        this.nbParticulesOxygene = 0;
        this.nbParticulesGaz = 0;
        this.nbParticulesVapeur = 0;
    }

    override
    public string ToString()
    {
        return "nbOxygène: " + nbParticulesOxygene + " | " + "nbGaz: " + nbParticulesVapeur + " | " + "nbVapeur: " + nbParticulesVapeur;
    }
}
