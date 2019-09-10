using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connexion : MonoBehaviour
{
    public Transform anchorLink = null;

    [Space]
    public ConnexionType_SO typeConnexion;

    [Space]
    public List<Connexion> connexions;
    public List<Link> links;

    private void Start()
    {
        connexions = new List<Connexion>();
    }

    public bool AddConnexion(Connexion connexion)
    {
        if (!connexions.Contains(connexion))
        {
            connexions.Add(connexion);
            return true;
        }
        else
            return false;
    }

    public bool RemoveConnexion(Connexion connexion)
    {
        return connexions.Remove(connexion);
    }

    public bool AddLink(Link link)
    {
        if (!links.Contains(link))
        {
            links.Add(link);
            return true;
        }
        else
            return false;
    }

    public bool RemoveLink(Link link)
    {
        return links.Remove(link);
    }

    public void AllLinksDoUpdate(bool doUpdate)
    {
        foreach (Link l in links)
        {
            l.enabled = doUpdate;
        }
    }

    public void AllLinksInstantRefresh()
    {
        foreach (Link l in links)
        {
            l.RefreshLine();
        }
    }

    public List<Connexion> GetConnexions()
    {
        return connexions;
    }

    /// <summary>
    /// remplie la list d'engins avec tout les engins qui sont connecté entre eux
    /// Check chaque reseau de l'engin. Si il possède le reseau donné en paramètre celui ci est ajouté au premier de la list
    /// </summary>
    /// <param name="enginsVus">List d'engins que se ferra remplir</param>
    /// <param name="checkReseau">Reseau a checker sur chaque engins (Facultatif) </param>
    public void GetAllEnginsConnected(List<EnginElec> enginsVus, ReseauElec checkReseau = null)
    {
        if (checkReseau != null && GetComponent<ReseauElec>() == checkReseau)                                //Si on est le réseau maitre recherché 
            enginsVus.Insert(0, (GetComponent<EnginElec>()));                                                //On s'ajoute en premier de la list
        else                                                                                                 //Sinon
            enginsVus.Add(GetComponent<EnginElec>());                                                        //On s'ajoute à la fin

        foreach (Connexion co in connexions)
        {
            if (co.GetComponent<EnginElec>() && !enginsVus.Contains(co.GetComponent<EnginElec>()))
            {
                co.GetAllEnginsConnected(enginsVus, checkReseau);
            }
        }
    }
}
