using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnginElec : MonoBehaviour
{
    public ReseauElec reseauElec;
    public bool ON_OffElec = true;

    protected void Start()
    {
        reseauElec = GetComponent<ReseauElec>();
    }

    /// <summary>
    /// Permet d'allumer ou éteindre l'appareil
    /// </summary>
    /// <param name="state"> true = active | false = desactive</param>
    public void ActiveEngin(bool state)
    {
        ON_OffElec = state;
    }
}
