using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffre : MonoBehaviour
{

    [SerializeField] private int capacity;
    private Connexion connexionComponent;
    private List<Recoltable> _recoltables = new List<Recoltable>();

    private void Start()
    {
        connexionComponent = GetComponent<Connexion>();
    }

    public int GetCapacity()
    {
        List<Coffre> coffresVus = new List<Coffre>();
        return GetCapacityRecursively(coffresVus);
    }


    public List<Recoltable> GetRecoltables()
    {
        List<Coffre> coffresVus = new List<Coffre>();
        return GetRecoltablesRecursively(coffresVus);
    }


    // Retourne vrai si l'item a pu être ajouté
    public bool AjouterItem(Recoltable item)
    {
        List<Coffre> coffresVus = new List<Coffre>();
        return AjouterItemRecursively(coffresVus, item);
    }


    public bool RetirerItem(Recoltable item)
    {
        List<Coffre> coffresVus = new List<Coffre>();
        return RetirerItemRecursively(coffresVus, item);
    }


    private List<Coffre> ConnectedCoffres()
    {
        List<Coffre> coffres = new List<Coffre>();
        foreach (Connexion conn in connexionComponent.connexions)
        {
            Coffre coffre = conn.GetComponentInChildren<Coffre>();
            if (coffre != null && !coffres.Contains(coffre))
                coffres.Add(coffre);
        }
        return coffres;
    }


    #region Fonctions recursives

    protected int GetCapacityRecursively(List<Coffre> coffresVus)
    {
        coffresVus.Add(this);
        int result = capacity;

        foreach (Coffre c in ConnectedCoffres())
        {
            if (!coffresVus.Contains(c))
                result += c.GetCapacityRecursively(coffresVus);
        }
        return result;
    }

    protected List<Recoltable> GetRecoltablesRecursively(List<Coffre> coffresVus)
    {
        coffresVus.Add(this); //S'ajoute lui-même à la liste des coffres vus
        List<Recoltable> result = new List<Recoltable>();
        result.AddRange(_recoltables); //Ajoute son contenu au résultat

        foreach (Coffre coffre in ConnectedCoffres()) //Dans la liste dans coffre connectés
        {
            if (!coffresVus.Contains(coffre)) //Si ce coffre n'a pas déjà été visité
            {
                result.AddRange(coffre.GetRecoltablesRecursively(coffresVus)); //On demande à ce coffre et tous ses enfants non vus de renvoyer leur contenu
            }
        }

        return result;
    }

    protected bool AjouterItemRecursively(List<Coffre> coffresVus, Recoltable item)
    {
        coffresVus.Add(this);
        if (_recoltables.Count < capacity)
        {
            _recoltables.Add(item);
            return true;
        }
        else
        {
            foreach (Coffre c in ConnectedCoffres())
            {
                if (!coffresVus.Contains(c))
                    if (c.AjouterItemRecursively(coffresVus, item))
                        return true;
            }
        }
        return false;
    }

    protected bool RetirerItemRecursively(List<Coffre> coffresVus, Recoltable item)
    {
        coffresVus.Add(this);

        if (_recoltables.Remove(item))
            return true;
        else
        {
            foreach (Coffre c in ConnectedCoffres())
            {
                if (!coffresVus.Contains(c))
                    if (c.RetirerItemRecursively(coffresVus, item))
                        return true;
            }
        }
        return false;
    }

    #endregion
}
