﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minerai : Recoltable
{
    public SpawnerMinerais Mine { set => mine = value; }

    private Minerai_SO _SO;
    private SpawnerMinerais mine;
    private Rigidbody _rb;
    private GravityBody _gb;

    protected override void Start()
    {
        base.Start();
        _SO = data as Minerai_SO;
        _rb = GetComponent<Rigidbody>();
        _gb = GetComponent<GravityBody>();
    }

    public override Recoltable Recolte()
    {
        if (mine != null)
        {
            mine.MineraiRecolte();
            mine = null;
        }
        return base.Recolte();
    }

    override public void Lacher(Vector3 dropPosition)
    {
        base.Lacher(dropPosition);
        _rb.isKinematic = false;
        _gb.enabled = true;
        Debug.Log("je suis " + _SO.nom);
    }
}
