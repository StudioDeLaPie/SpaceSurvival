using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnginElec : MonoBehaviour
{
    public ReseauElec reseauMaitre;
    public bool ON_OffElec = true;
    [ShowOnly, SerializeField] private bool fonctionnel = false;
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
                fonctionnel = true;
                engin.TurnOn();
            }
            else
            {
                fonctionnel = false;
                engin.TurnOff();
            }
        }
        else
        {
            fonctionnel = false;
            engin.TurnOff();
        }
    }

    public virtual void SwitchON_OFF()
    {
        ActiveEngin(!ON_OffElec);
    }

    /// <summary>
    /// Permet au réseau de prévenir les egnins l'état actuel du réseau 
    /// </summary>
    /// <param name="suffisant"></param>
    public virtual void AlimentationSuffisante(bool suffisant)
    {
        //Si on rentre ici c'est que le reseau vient de changer d'état

        if (ON_OffElec && suffisant && !fonctionnel) //Si il était non fonctionnel mais qu'il est allumé et que le courant est maintenant suffisant
        {
            fonctionnel = true; //L'engin fonctionne (le compresseur compresse, ...)
            engin.TurnOn();
        }
        else if (!suffisant && fonctionnel) //Si était fonctionnel mais plus assez de courant
        {
            fonctionnel = false; 
            engin.TurnOff();
        }
    }

    public bool GetAlimentationSuffisante()
    {
        return reseauMaitre.EtatFonctionnementReseau;
    }

    public bool Fonctionnel { get => fonctionnel; set => fonctionnel = value; }
}
