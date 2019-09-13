using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreuse : MonoBehaviour, I_Elec
{
    public float delayMinage = 15f;
    public DetecteurForeuse detecteur;
    public GameObject prefabMinerais;
    public Connexion connexion;

    public ForeuseElec foreuseElec;

    new public Animation animation;

    private Coffre coffre; //Un des coffres connecté à la foreuse

    public void TurnOff()
    {
        animation.Stop();
        CancelInvoke("Minage");
    }

    public void TurnOn()
    {
        animation.Play();
        InvokeRepeating("Minage", delayMinage, delayMinage);
    }

    /// <summary>
    /// Appellé en boucle pour "MINER" un minerais et le placer dans les coffres
    /// La Foreuse s'arrête si il n'y a plus de place dans les coffres
    /// </summary>
    private void Minage()
    {
        if (CoffreConnected())
        {
            prefabMinerais = detecteur.prefabMinerais;

            GameObject temp = Instantiate(prefabMinerais);

            if (!coffre.AjouterItem(temp.GetComponent<Recoltable>().Recolte())) //Si le minage est impossible
            {
                foreuseElec.SwitchON_OFF(); //On s'éteint
                Destroy(temp);              //On détruit le minerai qui devait être récolté
            }
        }
        else
        {
            //Si aucun coffre branché
            foreuseElec.SwitchON_OFF();
        }
    }

    /// <summary>
    /// Renvoie true si au moins un coffre est connecté
    /// </summary>
    /// <returns></returns>
    private bool CoffreConnected()
    {
        foreach (Connexion co in connexion.GetConnexions(TypeLink.Recoltables))
        {
            if (co.GetComponent<Coffre>())
            {
                coffre = co.GetComponent<Coffre>();
                return true;
            }
        }
        return false;
    }
}
