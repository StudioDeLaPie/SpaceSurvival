using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Craft_SO;

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

    public void SuppressionRessources(ref ComposantRecette composantASupprimer, List<Coffre> coffresVus)
    {
        coffresVus.Add(this);

        for (int i = _recoltables.Count - 1; i >= 0; i--)
        {
            if (composantASupprimer.quantity > 0) //Si on ne les a pas encore tous trouvé, on continue à chercher
            { 
                if (_recoltables[i].data == composantASupprimer.recoltable) //S'il s'agit du même SO
                {
                    Recoltable r = _recoltables[i];
                    _recoltables.RemoveAt(i);               //Suppression de la liste
                    Destroy(r.gameObject);    //Suppression de l'objet
                    composantASupprimer.quantity--;         //On en a trouvé un, on réduit donc le nombre recherché
                }
            }
        }

        if (composantASupprimer.quantity > 0) //S'il n'y en avait pas assez dans ce coffre
        {
            foreach (Coffre c in ConnectedCoffres()) //On va chercher dans les coffres connectés
            {
                if (!coffresVus.Contains(c))
                {
                    c.SuppressionRessources(ref composantASupprimer, coffresVus);
                }
            }
        }
    }

    public void GetComposantsDisponibles(Dictionary<Recoltable_SO, int> composantsDispos, List<Coffre> coffresVus)
    {
        coffresVus.Add(this);
        foreach(Recoltable recoltable in _recoltables)
        {
            if (composantsDispos.ContainsKey(recoltable.data))
            {
                composantsDispos[recoltable.data]++;
            }
            else
            {
                composantsDispos.Add(recoltable.data, 1);
            }
        }

        foreach (Coffre c in ConnectedCoffres()) //On va chercher dans les coffres connectés
        {
            if (!coffresVus.Contains(c))
            {
                c.GetComposantsDisponibles(composantsDispos, coffresVus);
            }
        }
    }

    //Modifie le composant passé en paramètre pour réduire sa valeur "quantité" à chaque fois qu'on le trouve dans un coffre
    public void TestQuantiteRecursively(ref ComposantRecette composantToTest, List<Coffre> coffresVus)
    {
        coffresVus.Add(this);
        foreach (Recoltable recoltable in _recoltables)
        {
            if (recoltable.data == composantToTest.recoltable) //S'il s'agit du même SO
            {
                composantToTest.quantity--; //On en a trouvé un, on réduit donc le nombre recherché
                if (composantToTest.quantity <= 0)
                    break;
            }
        }

        if (composantToTest.quantity > 0) //S'il n'y en avait pas assez dans ce coffre
        {
            foreach (Coffre c in ConnectedCoffres()) //On va chercher dans les coffres connectés
            {
                if (!coffresVus.Contains(c))
                {
                    c.TestQuantiteRecursively(ref composantToTest, coffresVus);
                }
            }
        }
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
