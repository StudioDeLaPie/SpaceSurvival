using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Recoltable : MonoBehaviour
{
    public Recoltable_SO data;

    private Transform _transform;

    protected virtual void Start()
    {
        _transform = transform;
    }

    public virtual Recoltable Recolte()
    {
        gameObject.SetActive(false);
        return this;
    }

    public virtual void Lacher(Vector3 dropPosition)
    {
        _transform.position = dropPosition;
        gameObject.SetActive(true);
        Debug.Log("Drop " + name);
    }
}
