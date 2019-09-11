using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnginElec : MonoBehaviour
{
    public ReseauElec reseauMaitre;
    public bool ON_OffElec = true;
    protected I_Elec engin;

    protected void Start()
    {
        reseauMaitre = GetComponent<ReseauElec>();
        engin = GetComponent<I_Elec>();
    }

    /// <summary>
    /// Permet d'allumer ou éteindre l'appareil
    /// </summary>
    /// <param name="state"> true = active | false = desactive</param>
    public virtual void ActiveEngin(bool state)
    {
        ON_OffElec = state;
        if (ON_OffElec)
        {
            if (GetAlimentationSuffisante())
            {
                engin.TurnOn();
            }
            else
                engin.TurnOff();
        }
        else
            engin.TurnOff();
    }

    public virtual void SwitchON_OFF()
    {
        ActiveEngin(!ON_OffElec);
    }

    public virtual void AlimentationSuffisante(bool suffisant)
    {
        //Si on rentre ici c'est que le reseau vient de changer d'état
        if (ON_OffElec && suffisant)
        {
            engin.TurnOn();
        }
        else
        {
            engin.TurnOff();
        }
    }

    public bool GetAlimentationSuffisante()
    {
        return reseauMaitre.EtatFonctionnementReseau;
    }

    /// <summary>
    /// Demande à l'engin de re-checker s'il devrait être allumé
    /// </summary>
    public virtual void CheckReseauToTurnOn()
    {
        if (ON_OffElec && GetAlimentationSuffisante())
        {
            engin.TurnOn();
        }
        else
        {
            engin.TurnOff();
        }
    }
}
