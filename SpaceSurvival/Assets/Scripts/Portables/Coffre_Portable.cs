using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffre_Portable : Portable
{
    public MeshRenderer meshRenderer;

    public Material baseMaterial;
    public Material rightMaterial;
    public Material wrongMaterial;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerStay(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = false;
            meshRenderer.sharedMaterial = wrongMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = true;
            meshRenderer.sharedMaterial = rightMaterial;
        }
    }

    public override void ObjectInDeplacement()
    {
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
        inDeplacement = true;
        canBeConstruct = true;
        meshRenderer.sharedMaterial = rightMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(true);
    }

    public override void ObjectPlaced()
    {
        meshRenderer.sharedMaterial = baseMaterial;
        inDeplacement = false;
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = false;
        GetComponent<Connexion>().AllLinksDoUpdate(false);
    }
}
