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
            recoltables[0].Lacher(aim.transform.position + aim.transform.forward * 2);
            recoltables.RemoveAt(0);
        }
    }

    public void AimingRecoltable(Recoltable recoltable)
    {
        Debug.Log("On vise le recoltable " + recoltable.name);
        if (Input.GetButtonDown("Recolte"))
            recoltables.Add(recoltable.Recolte());
    }
}
