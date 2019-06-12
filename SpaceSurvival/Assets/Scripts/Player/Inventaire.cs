using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
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
        if (_coffreOuvert) //L'inventaire ne réagit plus si on est dans un coffre
        {
            if (Input.GetButtonDown("Action"))
            {
                Debug.Log("Je suis l'inventaire, on m'a dit que le coffre se fermait");
                uiManager.FermetureCoffre();
                _coffreOuvert = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("LacherInventaire") && recoltables.Count > 0)
            {
                LacherItem(recoltables[0]);
            }
        }

    }

    public void AjouterItem(Recoltable item)
    {
        recoltables.Add(item);
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
        if (Input.GetButtonDown("Recolte"))
            recoltables.Add(recoltable.Recolte());
    }

    public void LacherItem(Recoltable item)
    {
        item.Lacher(aim.transform.position + aim.transform.forward * 2);
        recoltables.Remove(item);
    }

    #endregion

    #region Coffre
    /// <summary>
    /// Appelé par Aiming pour avertir qu'on vise un coffre
    /// </summary>
    /// <param name="coffre"></param>
    public void AimingCoffre(Coffre coffre)
    {
        if (Input.GetButtonDown("Action"))
        {
            _coffreOuvert = true;
            Debug.Log("Ouverture du coffre " + coffre.name);
            uiManager.OuvertureCoffre(coffre);
        }
    }

    #endregion
}
