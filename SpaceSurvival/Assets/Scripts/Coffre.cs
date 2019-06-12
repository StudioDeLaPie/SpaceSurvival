using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffre : MonoBehaviour
{
    public int capacity;

    private List<Recoltable> _recoltables = new List<Recoltable>();

    public List<Recoltable> GetRecoltables()
    {
        return _recoltables;
    }

    // Retourne vrai si l'item a pu être ajouté
    public bool AjouterItem(Recoltable item)
    {
        if (_recoltables.Count < capacity)
        {
            _recoltables.Add(item);
            return true;
        }
        else
            return false;
    }

    public bool RetirerItem(Recoltable item)
    {
        return _recoltables.Remove(item);
    }
}
