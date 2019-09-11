using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour, I_Elec
{
    private OutilsConnecteur connecteur;
    private Connexion connexion;

    private void Start()
    {
        connecteur = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStarter>().player.GetComponentInChildren<OutilsConnecteur>(true);
        connexion = GetComponent<Connexion>();
    }

    public void TurnOff()
    {
        Debug.Log("Off");
        foreach (Link link in connexion.GetLinks())
        {
            link.SetTypeOfLink(TypeLink.ElectricMort);
            connecteur.RegenererReseauElec(link.GetFirstConnexion(), link.GetSecondConnexion());
        }
    }

    public void TurnOn()
    {
        Debug.Log("On");
        foreach (Link link in connexion.GetLinks())
        {
            connecteur.ConnexionEnginsElec(link.GetFirstConnexion(), link.GetSecondConnexion(), link);
        }
        //RegenererReseauElec();
    }

    private void RegenererReseauElec()
    {
        foreach (Connexion connectedConnexion in connexion.GetConnexions())
        {
            connecteur.RegenererReseauElec(connexion, connectedConnexion);
        }
    }
}
