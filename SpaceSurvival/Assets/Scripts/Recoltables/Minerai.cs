using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minerai : Recoltable
{
    override public Recoltable Recolte()
    {
        return base.Recolte();
    }

    override public void Lacher(Vector3 dropPosition)
    {
        base.Lacher(dropPosition);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<GravityBody>().enabled = true;
    }
}
