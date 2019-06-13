using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connexion : MonoBehaviour
{
    public EConnexionType type;
    public List<Connexion> connexions;

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
}
