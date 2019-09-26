using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField/*, HideInInspector*/] private Connexion firstConnexion;
    [SerializeField/*, HideInInspector*/] private Connexion secondConnexion;
    [SerializeField/*, HideInInspector*/] private Transform anchor1;
    [SerializeField/*, HideInInspector*/] private Transform anchor2;
    [SerializeField/*, HideInInspector*/] private TypeLink typelink = TypeLink.Recoltables;
    
    public float distanceMaxLink;
    public Material matDefaultLink;
    public Material matElectricLink;
    public Material matElectricMortLink;
    public Material matWrong;


    private Transform _transform;
    private Renderer _renderer;
    

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();

        _transform = GetComponent<Transform>();

        //On lui attribut un nouveau parent pour l'amour de la hierarchie
        if (transform.parent == null || transform.parent.tag != "Links")
        {
            transform.parent = GameObject.FindGameObjectWithTag("Links").transform;
        }

        Debug.Log("Init fait ! " + name);

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
                    case TypeLink.Recoltables:
                        _renderer.sharedMaterial = matDefaultLink;
                        break;
                    case TypeLink.Electric:
                        _renderer.sharedMaterial = matElectricLink;
                        break;
                    case TypeLink.ElectricMort:
                        _renderer.sharedMaterial = matElectricMortLink;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _renderer.sharedMaterial = matWrong;
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
