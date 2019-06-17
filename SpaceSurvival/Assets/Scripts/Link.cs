using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public GameObject firstGameObject;
    public GameObject secondGameObject;

    public float distanceMaxLink;
    public Material matDefaultLink;
    public Material matWrong;

    private Transform _transform;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();

        _transform = GetComponent<Transform>();

        //On lui attribut un nouveau parents pour l'amour de la hierarchie
        Transform transformNewParent = GameObject.FindGameObjectWithTag("Links").transform;
        if (transformNewParent != null) _transform.parent = transformNewParent;

        RefreshLine();
    }

    private void Update()
    {
        RefreshLine();
    }

    public void RefreshLine()
    {
        Vector3 _pos1 = firstGameObject.transform.position;
        Vector3 _pos2 = secondGameObject.transform.position;
        _transform.position = new Vector3(((_pos1.x + _pos2.x) / 2), ((_pos1.y + _pos2.y) / 2), ((_pos1.z + _pos2.z) / 2));
        _transform.LookAt(firstGameObject.transform);
        _transform.localScale = new Vector3(1, 1, ((Vector3.Distance(_pos1, _pos2))));

        //MATERIALS
        if (firstGameObject != null && secondGameObject != null)
        {
            if (LinkHasGoodSize())
            {
                renderer.sharedMaterial = matDefaultLink;
            }
            else
            {
                renderer.sharedMaterial = matWrong;
            }
        }
    }

    /// <summary>
    /// Check la distance entre les deux extrémité du link
    /// </summary>
    /// <returns>True = l'objet est assez proche | False = l'object a depacer la limite</returns>
    public bool LinkHasGoodSize()
    {
        return Vector3.Distance(firstGameObject.transform.position, secondGameObject.transform.position) < distanceMaxLink;
    }


    /// <summary>
    /// Les choses à faire lorsque le lien est terminé et établie
    /// </summary>
    public void LinkCompleted()
    {
        RefreshLine();
        this.enabled = false;
        firstGameObject.GetComponent<Connexion>().AddLink(this);
        secondGameObject.GetComponent<Connexion>().AddLink(this);
    }

    public void DisconnectLink()
    {
        firstGameObject.GetComponent<Connexion>().RemoveLink(this);
        secondGameObject.GetComponent<Connexion>().RemoveLink(this);
    }
}
