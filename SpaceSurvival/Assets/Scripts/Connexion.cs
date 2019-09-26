using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connexion : MonoBehaviour
{
    public Transform anchorLink = null;

    [Space]
    public ConnexionType_SO typeConnexion;

    [Space]
    /*[SerializeField, HideInInspector]*/ public Dictionary<Connexion, Link> connexions_links = new Dictionary<Connexion, Link>();

    public bool AddConnexion(Connexion connexion, Link link)
    {
        if (!connexions_links.ContainsKey(connexion))
        {
            connexions_links.Add(connexion, link);
            return true;
        }
        else
            return false;
    }

    public bool RemoveConnexion(Connexion connexion)
    {
        //Pas besoin de déconnecter le Link, il sera détruit après le remove
        if (connexion != null)
            return connexions_links.Remove(connexion);
        return true; //Si connexion == null c'est que l'objectif est déjà atteint
    }

    public void AllLinksDoUpdate(bool doUpdate)
    {
        foreach (KeyValuePair<Connexion, Link> pair in connexions_links)
        {
            pair.Value.enabled = doUpdate;
        }
    }

    public void AllLinksInstantRefresh()
    {
        foreach (KeyValuePair<Connexion, Link> pair in connexions_links)
        {
            pair.Value.RefreshLine();
        }
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

        foreach (Connexion co in GetConnexions(TypeLink.Electric))
        {
            if (co.GetComponent<EnginElec>() && !enginsVus.Contains(co.GetComponent<EnginElec>()))
            {
                co.GetAllEnginsConnected(enginsVus, checkReseau);
            }
        }
    }

    /// <summary>
    /// Renvoie toutes les connexions.
    /// </summary>
    /// <returns></returns>
    public List<Connexion> GetConnexions()
    {
        return new List<Connexion>(connexions_links.Keys);
    }

    /// <summary>
    /// Renvoie toutes les connexions du type donné.
    /// </summary>
    /// <param name="typeLink"></param>
    /// <returns></returns>
    public List<Connexion> GetConnexions(TypeLink typeLink)
    {
        List<Connexion> result = new List<Connexion>();
        foreach (KeyValuePair<Connexion, Link> pair in connexions_links)
        {
            if (pair.Value.GetTypeLink() == typeLink)
                result.Add(pair.Key);
        }
        return result;
    }

    public List<Link> GetLinks()
    {
        return new List<Link>(connexions_links.Values);
    }

    [ContextMenu("■ Debug Connexions")]
    public void TaMer()
    {
        Debug.Log(" ■ Dictionnaire de : " + name);
        foreach (KeyValuePair<Connexion, Link> pair in connexions_links)
        {
            Debug.Log(pair.Key.name + " | " + pair.Value.name);
        }
    }
}
