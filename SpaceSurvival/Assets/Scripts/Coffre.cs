using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffre : MonoBehaviour
{
    private List<Recoltable> _recoltables = new List<Recoltable>();

    public List<Recoltable> GetRecoltables()
    {
        return _recoltables;
    }

    public void AjouterItem(Recoltable item)
    {
        _recoltables.Add(item);
    }

    public bool RetirerItem(Recoltable item)
    {
        return _recoltables.Remove(item);
    }
}
