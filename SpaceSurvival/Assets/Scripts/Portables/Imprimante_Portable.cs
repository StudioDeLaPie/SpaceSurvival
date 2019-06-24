using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imprimante_Portable : Portable
{
    public List<MeshRenderer> meshRenderers;

    public Material baseMaterial;
    public Material rightMaterial;
    public Material wrongMaterial;

    private List<Collider> _colliders = new List<Collider>();

    protected override void Start()
    {
        base.Start();
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            _colliders.Add(col);
        }
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
        _colliders.ForEach(col => col.isTrigger = true);

        inDeplacement = true;
        canBeConstruct = true;
        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.sharedMaterial = rightMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(true);
    }

    public override void ObjectPlaced()
    {
        _colliders.ForEach(col => col.isTrigger = false);
        inDeplacement = false;
        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.sharedMaterial = baseMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(false);
    }
}
