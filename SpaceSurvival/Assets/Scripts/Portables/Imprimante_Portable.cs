using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imprimante_Portable : Portable
{
    public List<MeshRenderer> meshRenderers;

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
            foreach(MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.sharedMaterial = wrongMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = true;
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.sharedMaterial = rightMaterial;
        }
    }

    public override void ObjectInDeplacement()
    {
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
        inDeplacement = true;
        canBeConstruct = true;
        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.sharedMaterial = rightMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(true);
    }

    public override void ObjectPlaced()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.sharedMaterial = baseMaterial;
        inDeplacement = false;
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = false;
        GetComponent<Connexion>().AllLinksDoUpdate(false);
    }
}
