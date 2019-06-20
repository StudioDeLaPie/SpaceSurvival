using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutilsConnecteur : MonoBehaviour
{
    public bool connecteurEnabled = false;
    public List<MonoBehaviour> monoBehaviourADesactiver;
    public LayerMask layerMask;

    public GameObject prefabLink;

    [Space]
    public Connexion firstConnexion;
    public Connexion secondConnexion;

    public GameObject playerAnchorPoint;

    [Space]
    public Material matDefaultLink;
    public Material matWrong;
    public float distanceMaxLink;
    public float toleranceLinkBreak;

    private RaycastHit _hitInfo;
    private Transform _transform;
    private Link currentLink;
    private bool linkCanBeComplete = false;

    void Start()
    {
        _hitInfo = new RaycastHit();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Connecteur"))
        {
            connecteurEnabled = !connecteurEnabled;
            ActiveDesactiveAllMonoBehaviours(connecteurEnabled);
        }

        if (connecteurEnabled)
        {
            if (RayCast())
            {
                if (_hitInfo.transform.root.GetComponentInChildren<Connexion>() != null)
                {

                    //TOUCHE UN PREMIER CONNECTABLE
                    if (Input.GetButtonDown("MouseLeft") && firstConnexion == null) //Si on Clic sur quelque chose et qu'on a aucune connexion en cours
                    {
                        firstConnexion = _hitInfo.transform.root.GetComponentInChildren<Connexion>();                         //On recupère la connexion de l'objet touché

                        currentLink = GameObject.Instantiate(prefabLink).GetComponent<Link>();                                //On créer un lien visuel
                        currentLink.firstGameObject = firstConnexion.gameObject;                                              //Entre l'objet touché
                        currentLink.secondGameObject = playerAnchorPoint;                                                     //Et le Player
                    }
                    //TOUCHE UNE DEUXIEME CONNECTABLE
                    else if (Input.GetButtonDown("MouseLeft") && firstConnexion != null)
                    {
                        ConnexionType_SO connexionObjectTouched = _hitInfo.transform.root.GetComponentInChildren<Connexion>().typeConnexion;

                        //Est ce que la connexion du premier objet accept des connexion du deuxième
                        if (firstConnexion.typeConnexion.CanBeConnectWith(connexionObjectTouched))
                        {
                            secondConnexion = _hitInfo.transform.root.GetComponentInChildren<Connexion>();                    //On récupère la connexion du deuxième objet
                            currentLink.secondGameObject = secondConnexion.gameObject;                                        //On met à jour le lien Visuel

                            CheckDistance(); //On recalcule la distance mais avec le nouvel objet

                            //Si les deux objets liés sont tout deux des engin electrique
                            if (firstConnexion.GetComponent<EnginElec>() != null && secondConnexion.GetComponent<EnginElec>() != null)
                            {
                                currentLink.SetTypeOfLink(TypeLink.Electric);
                            }

                            if (linkCanBeComplete)
                            {
                                if (!CompleteConnection())                                                                        //Permet de tester si un lien existe deja ou non
                                {
                                    Stop();
                                }
                            }
                            else
                            {
                                Stop();
                            }
                        }
                        else
                        {
                            Stop();
                        }
                    }
                }
                //TOUCHE UN LINK (pour le supprimer)
                if (Input.GetButtonDown("MouseRight") && currentLink == null && _hitInfo.transform.root.GetComponentInChildren<Link>(true) != null)
                {
                    Link linkTouched = _hitInfo.transform.parent.GetComponentInChildren<Link>(true);
                    Connexion firstCo = linkTouched.firstGameObject.GetComponent<Connexion>();
                    Connexion secondCo = linkTouched.secondGameObject.GetComponent<Connexion>();

                    firstCo.RemoveConnexion(secondCo);
                    if (secondCo != null) secondCo.RemoveConnexion(firstCo);

                    linkTouched.DisconnectLink();
                    Destroy(linkTouched.gameObject);
                }
            }
            if (Input.GetButtonDown("MouseRight") && currentLink != null)                                               //Si clic droit quand on a un lien en main
                Stop();

            CheckDistance();
        }
    }

    private void CheckDistance()
    {
        if (currentLink != null)
        {
            float dist = Vector3.Distance(currentLink.firstGameObject.transform.position, currentLink.secondGameObject.transform.position);
            if (dist > distanceMaxLink + toleranceLinkBreak)
                Stop();
            else if (dist > distanceMaxLink)
                linkCanBeComplete = false;
            else
                linkCanBeComplete = true;
        }
    }


    private bool CompleteConnection()
    {
        if (!firstConnexion.AddConnexion(secondConnexion))                                                               //Si on arrive pas à ajouter la connexion
            return false;

        if (!secondConnexion.AddConnexion(firstConnexion))                                                               //Si on arrive pas à ajouter la connexion
            return false;

        currentLink.LinkCompleted();
        ResetVariables();
        return true;
    }

    private void ResetVariables()
    {
        currentLink = null;
        firstConnexion = null;
        secondConnexion = null;
        linkCanBeComplete = false;
    }

    private void Stop()
    {
        if (currentLink != null)
            Destroy(currentLink.gameObject);
        ResetVariables();
    }

    private bool RayCast()
    {
        return Physics.Raycast(_transform.position, _transform.forward, out _hitInfo, 10, layerMask);
    }

    /// <summary>
    /// Active ou désactive tout les monobehaviour
    /// </summary>
    /// <param name="enable"></param>
    private void ActiveDesactiveAllMonoBehaviours(bool enable)
    {
        foreach (MonoBehaviour mono in monoBehaviourADesactiver)
        {
            mono.enabled = !enable;
        }

        Stop();
    }
}
