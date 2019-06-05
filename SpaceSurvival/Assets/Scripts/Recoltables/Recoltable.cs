using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Recoltable : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public Recoltable Recolte()
    {
        gameObject.SetActive(false);
        Debug.Log("Je suis récolté " + name);
        return this;
    }

    public void Lacher(Vector3 dropPosition)
    {
        _transform.position = dropPosition;
        gameObject.SetActive(true);
        Debug.Log("Drop " + name);
    }
}
