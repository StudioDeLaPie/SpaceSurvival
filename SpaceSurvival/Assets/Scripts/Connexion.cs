using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connexion : MonoBehaviour
{
    public EConnexionType type;
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
}
