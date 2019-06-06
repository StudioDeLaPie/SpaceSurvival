using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public List<Recoltable> recoltables;
    public Aim aim;

    // Start is called before the first frame update
    void Start()
    {
        recoltables = new List<Recoltable>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("LacherInventaire") && recoltables.Count > 0)
        {
            LacherItem(recoltables[0]);
        }
    }

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
}
