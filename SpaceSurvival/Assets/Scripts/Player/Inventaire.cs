using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public int capacity;
    public List<Recoltable> recoltables;
    public Aim aim;
    public UIManager uiManager;

    private bool _coffreOuvert = false;

    // Start is called before the first frame update
    void Start()
    {
        recoltables = new List<Recoltable>();
    }

    private void Update()
    {
        if (!_coffreOuvert) //L'inventaire ne rien si un coffre est ouvert
        {
            if (Input.GetButtonDown("LacherInventaire") && recoltables.Count > 0)
            {
                LacherItem(recoltables[0]);
            }
        }

    }

    // Retourne vrai si l'item a pu être ajouté
    public bool AjouterItem(Recoltable item)
    {
        if (recoltables.Count < capacity)
        {
            recoltables.Add(item);
            return true;
        }
        else
            return false;
    }

    public bool RetirerItem(Recoltable item)
    {
        return recoltables.Remove(item);
    }

    #region Recoltables
    /// <summary>
    /// Appelé par le script Aim pour prévenir qu'un récoltable est en mire
    /// </summary>
    /// <param name="recoltable"></param>
    public void AimingRecoltable(Recoltable recoltable)
    {
        if (Input.GetButtonDown("Recolte") && AjouterItem(recoltable))
            recoltable.Recolte();
    }

    public void LacherItem(Recoltable item)
    {
        if (RetirerItem(item))
            item.Lacher(aim.transform.position + aim.transform.forward * 2);
    }

    #endregion

    #region Coffre
    /// <summary>
    /// Appelé par Aiming pour avertir qu'on vise un coffre
    /// </summary>
    /// <param name="coffre"></param>
    public void AimingCoffre(Coffre coffre)
    {
        if (Input.GetButtonUp("Action") && !_coffreOuvert)
        {
            Debug.Log("Ouverture coffre");
            _coffreOuvert = true;
            uiManager.OuvertureCoffre(coffre);
        }
    }

    public void FermetureCoffre()
    {
        _coffreOuvert = false;
    }

    #endregion
}
