using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Recoltable : MonoBehaviour
{
    public Recoltable_SO data;

    public virtual Recoltable Recolte()
    {
        if (transform.parent == null || transform.parent.name != "_Recoltables_") //Si pas de parent ou que la parent n'est pas _Recoltables_
        {
            transform.parent = GameObject.Find("_Recoltables_").transform;          //Déplacé dans _Recoltable_
        }
        gameObject.SetActive(false);
        return this;
    }

    public virtual void Lacher(Vector3 dropPosition)
    {
        transform.position = dropPosition;
        gameObject.SetActive(true);
    }
}
