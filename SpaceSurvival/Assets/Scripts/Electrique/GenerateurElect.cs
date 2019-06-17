using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerateurElect : MachineElec
{
    protected float puissance;
    public float puissanceDelivre;

    protected virtual void FixedUpdate()
    {
        if (ON_OffElec)//Si le générateur est allumé
        {
            //Consomation();
            puissanceDelivre = puissance;
        }
    }

    /// <summary>
    /// fonction lancer lorsque le générateur est en état de marche
    /// Permet de gérer si des resources sont a consommer
    /// </summary>
    //protected void Consomation()
    //{

    //}

    /// <summary>
    /// Permet d'allumer ou éteindre le générateur
    /// </summary>
    /// <param name="state"></param>
    public void ActiveGenerator(bool state)
    {
        ON_OffElec = state;
    }
}
