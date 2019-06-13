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

    private RaycastHit _hitInfo;
    private Transform _transform;
    private Link currentLink;
    private bool updateLink = false;
    private EConnexionType typeConnexion;

    void Start()
    {
        _hitInfo = new RaycastHit();
        _transform = GetComponent<Transform>();
        typeConnexion = EConnexionType.Null;
    }

    void Update()
    {
        if (Input.GetButtonDown("Connecteur"))
        {
            connecteurEnabled = !connecteurEnabled;
            ActiveDesactive(connecteurEnabled);
        }

        if (connecteurEnabled)
        {
            switch (typeConnexion)
            {
                case EConnexionType.Null:
                    break;
                case EConnexionType.Coffre:
                    ConnectingCoffre();
                    break;
                case EConnexionType.Imprimante:
                    break;
                default:
                    break;
            }

            if (RayCast())
            {
                //TOUCHE UN CONNECTABLE
                if (Input.GetButtonDown("MouseLeft") && _hitInfo.transform.root.GetComponentInChildren<Connexion>() != null && typeConnexion == EConnexionType.Null)                                  //Si on touche quelque chose && qu'on a rien toucher jusqu'à présent
                {
                    firstConnexion = _hitInfo.transform.GetComponent<Connexion>();                      //On recupère la connexion de l'objet touché
                    typeConnexion = firstConnexion.type;                                                //Maintenant on sait qu'on va traiter tel type de connexion

                    currentLink = GameObject.Instantiate(prefabLink).GetComponent<Link>();              //On créer un lien visuel
                    currentLink.firstGameObject = firstConnexion.gameObject;                            //Entre l'objet touché
                    currentLink.secondGameObject = playerAnchorPoint;                                   //Et le Player
                }

                //TOUCHE UN LINK (pour le supprimer)
                if (Input.GetButtonDown("MouseRight") && currentLink == null && _hitInfo.transform.root.GetComponentInChildren<Link>(true) != null)
                {
                    Link linkTouched = _hitInfo.transform.root.GetComponentInChildren<Link>(true);
                    Connexion firstCo = linkTouched.firstGameObject.GetComponent<Connexion>();
                    Connexion secondCo = linkTouched.secondGameObject.GetComponent<Connexion>();

                    firstCo.RemoveConnexion(secondCo);
                    if (secondCo != null) secondCo.RemoveConnexion(firstCo);

                    linkTouched.DisconnectLink();
                    Destroy(linkTouched.transform.root.gameObject);
                }
            }
            if (Input.GetButtonDown("MouseRight") && currentLink != null) //Si clic droit
                Stop();
        }
    }

    private void ConnectingCoffre()
    {
        if (connecteurEnabled && Input.GetButtonUp("MouseLeft") && RayCast())                                             //Si le mode connecteur est activé ET qu'on a touché quelque chose
        {
            //Si on arrive ici c'est qu'on a deja un premier objet de toucher et qu'on vient juste de toucher un dexième objet
            Coffre coffreTouche = _hitInfo.transform.root.GetComponentInChildren<Coffre>();
            if (coffreTouche != null && coffreTouche != firstConnexion.transform.root.GetComponentInChildren<Coffre>())                //Si il a un Coffre et qu'il est différent du premier
            {
                secondConnexion = _hitInfo.transform.GetComponent<Connexion>();         //On récupère la connexion du deuxième objet
                currentLink.secondGameObject = secondConnexion.gameObject;              //On met à jour le lien Visuel

                if (!CompleteConnection())
                {
                    Stop();
                }
            }
        }
    }

    private bool CompleteConnection()
    {
        if (!firstConnexion.AddConnexion(secondConnexion))                               //Si on arrive pas à ajouter la connexion
            return false;

        if (!secondConnexion.AddConnexion(firstConnexion))                              //Si on arrive pas à ajouter la connexion
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
        typeConnexion = EConnexionType.Null;
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
    private void ActiveDesactive(bool enable)
    {
        foreach (MonoBehaviour mono in monoBehaviourADesactiver)
        {
            mono.enabled = !enable;
        }

        Stop();
    }
}
