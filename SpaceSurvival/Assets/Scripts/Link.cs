using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    private Connexion firstConnexion;
    private Connexion secondConnexion;

    private Transform anchor1;
    private Transform anchor2;

    public float distanceMaxLink;
    public Material matDefaultLink;
    public Material matElectricLink;
    public Material matWrong;

    private Transform _transform;
    private Renderer renderer;
    private TypeLink typelink = TypeLink.Normal;
    

    private void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();

        _transform = GetComponent<Transform>();

        //On lui attribut un nouveau parents pour l'amour de la hierarchie
        Transform transformNewParent = GameObject.FindGameObjectWithTag("Links").transform;
        if (transformNewParent != null) _transform.parent = transformNewParent;

        //firstGameObject.GetComponentInChildren<AnchorConnexion>()

        RefreshLine();
    }

    private void Update()
    {
        RefreshLine();
    }

    public void RefreshLine()
    {
        Vector3 _pos1 = anchor1.position;
        Vector3 _pos2 = anchor2.position;
        _transform.position = new Vector3(((_pos1.x + _pos2.x) / 2), ((_pos1.y + _pos2.y) / 2), ((_pos1.z + _pos2.z) / 2));
        _transform.LookAt(anchor1);
        _transform.localScale = new Vector3(1, 1, ((Vector3.Distance(_pos1, _pos2))));

        //MATERIALS
        if (firstConnexion != null && secondConnexion != null)
        {
            if (LinkHasGoodSize())
            {
                switch (typelink)
                {
                    case TypeLink.Normal:
                        renderer.sharedMaterial = matDefaultLink;
                        break;
                    case TypeLink.Electric:
                        renderer.sharedMaterial = matElectricLink;
                        break;
                    default:
                        break;
                }
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
        return Vector3.Distance(anchor1.position, anchor2.position) < distanceMaxLink;
    }


    /// <summary>
    /// Les choses à faire lorsque le lien est terminé et établie
    /// </summary>
    public void LinkCompleted()
    {
        RefreshLine();
        this.enabled = false;
        firstConnexion.AddLink(this);
        secondConnexion.AddLink(this);
    }

    public void DisconnectLink()
    {
        firstConnexion.RemoveLink(this);
        secondConnexion.RemoveLink(this);
    }

    public void SetTypeOfLink(TypeLink type)
    {
        typelink = type;
        RefreshLine();
    }

    public TypeLink GetTypeLink()
    {
        return typelink;
    }

    public void SetFirstConnexion(Connexion co)
    {
        firstConnexion = co;
        if (co.anchorLink != null)
            anchor1 = co.anchorLink;
        else
            anchor1 = co.transform;
    }

    public Connexion GetFirstConnexion()
    {
        return firstConnexion;
    }

    public void SetSecondConnexion(Connexion co)
    {
        secondConnexion = co;
        if (co.anchorLink != null)
            anchor2 = co.anchorLink;
        else
            anchor2 = co.transform;
    }

    public Connexion GetSecondConnexion()
    {
        return secondConnexion;
    }

    public Transform GetTransform1()
    {
        return anchor1;
    }

    public Transform GetTransform2()
    {
        return anchor2;
    }

    /// <summary>
    /// Permet de donner l'anchor du player à la place de secondConnexion 
    /// </summary>
    /// <param name="playerHand"></param>
    public void SetPlayerAnchor(GameObject playerHand)
    {
        anchor2 = playerHand.transform;
    }
}
