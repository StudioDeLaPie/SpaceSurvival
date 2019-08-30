using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glace : Recoltable
{
    private Rigidbody _rb;
    private GravityBody _gb;

    override protected void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        _gb = GetComponent<GravityBody>();
    }

    override public void Lacher(Vector3 dropPosition)
    {
        base.Lacher(dropPosition);
        _rb.isKinematic = false;
        _gb.enabled = true;
    }
}
