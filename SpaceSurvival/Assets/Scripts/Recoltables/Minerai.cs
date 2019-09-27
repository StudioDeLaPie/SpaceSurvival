using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minerai : Recoltable
{
    private Mine mine;

    public void Init(Mine mine)
    {
        this.mine = mine;
    }

    override public Recoltable Recolte()
    {
        if (mine)
        {
            mine.RetraitMinerai();
            mine = null;
        }
        return base.Recolte();
    }

    override public void Lacher(Vector3 dropPosition)
    {
        base.Lacher(dropPosition);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<GravityBody>().enabled = true;
    }
}
