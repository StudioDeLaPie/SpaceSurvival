using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnginElec : MonoBehaviour
{
    public ReseauElec reseauMaitre;
    public bool ON_OffElec = true;

    protected void Start()
    {
        reseauMaitre = GetComponent<ReseauElec>();
    }

    /// <summary>
    /// Permet d'allumer ou éteindre l'appareil
    /// </summary>
    /// <param name="state"> true = active | false = desactive</param>
    public virtual void ActiveEngin(bool state)
    {
        ON_OffElec = state;
    }

    public abstract void SwitchON_OFF();
    
}
