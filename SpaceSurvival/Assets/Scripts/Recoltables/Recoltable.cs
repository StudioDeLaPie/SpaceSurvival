using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Recoltable : MonoBehaviour
{
    public Recoltable_SO data;

    public virtual Recoltable Recolte()
    {
        if (transform.parent.name != "_Recoltables_")
        {
            transform.parent = GameObject.Find("_Recoltables_").transform;
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
