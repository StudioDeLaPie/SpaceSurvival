using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Glace : Recoltable
{
    [SerializeField] private float timeToLive;
    [SerializeField] private Transform meshTransform;

    private Rigidbody _rb;
    private GravityBody _gb;

    private Vector3 baseScale;
    private float minScale = 0.3f;
    private float droppedSince = 0;
    private bool dropped = false;

    override protected void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        _gb = GetComponent<GravityBody>();
        dropped = gameObject.activeInHierarchy;
        baseScale = meshTransform.localScale;
    }

    private void Update()
    {
        if (dropped)
        {
            droppedSince += Time.deltaTime;
            if (droppedSince > timeToLive)
            {
                Destroy(gameObject);
            }
            else
            {
                float scale = Mathematiques.Map(droppedSince, 0, timeToLive, 1, minScale);
                meshTransform.localScale = baseScale * scale;
            }
        }
    }

    public override Recoltable Recolte()
    {
        dropped = false;
        meshTransform.localScale = baseScale;
        return base.Recolte();
    }

    override public void Lacher(Vector3 dropPosition)
    {
        base.Lacher(dropPosition);
        _rb.isKinematic = false;
        _gb.enabled = true;
        dropped = true;
        droppedSince = 0;
    }
}
