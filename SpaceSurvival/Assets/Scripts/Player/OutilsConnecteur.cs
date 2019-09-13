using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutilsConnecteur : MonoBehaviour
{
    public bool connecteurEnabled = false;
    public LayerMask layerMask;
    public GameObject prefabLink;
    public UIManager uiManager;
    public List<MonoBehaviour> monoBehaviourADesactiver;


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
            uiManager.FeedbackOutilConnecteur(connecteurEnabled);
        }

        if (connecteurEnabled)
        {
            if (RayCast())
            {
                //TOUCHE UN CONNECTABLE
                if (_hitInfo.transform.root.GetComponentInChildren<Connexion>() != null)
                {

                    //TOUCHE UN PREMIER CONNECTABLE
                    if (Input.GetButtonDown("MouseLeft") && firstConnexion == null) //Si on Clic sur quelque chose et qu'on a aucune connexion en cours
                    {
                        firstConnexion = _hitInfo.transform.root.GetComponentInChildren<Connexion>();                         //On recupère la connexion de l'objet touché

                        currentLink = GameObject.Instantiate(prefabLink).GetComponent<Link>();                                //On créer un lien visuel
                        currentLink.SetFirstConnexion(firstConnexion);                                                        //Entre l'objet touché
                        currentLink.SetPlayerAnchor(playerAnchorPoint);                                                       //Et le Player
                    }

                    //TOUCHE UNE DEUXIEME CONNECTABLE
                    else if (Input.GetButtonDown("MouseLeft") && firstConnexion != null)
                    {
                        secondConnexion = _hitInfo.transform.root.GetComponentInChildren<Connexion>();                  //On récupère la connexion du deuxième objet
                        PlaceLink(firstConnexion, secondConnexion, currentLink);
                    }
                }
                //TOUCHE UN LINK (pour le supprimer)
                if (Input.GetButtonDown("MouseRight") && currentLink == null && _hitInfo.transform.root.GetComponentInChildren<Link>(true) != null)
                {
                    Link linkTouched = _hitInfo.transform.parent.GetComponentInChildren<Link>(true);
                    RemoveLink(linkTouched);
                }
            }
            if (Input.GetButtonDown("MouseRight") && currentLink != null)                                               //Si clic droit quand on a un lien en main
                Stop();

            CheckDistance();
        }
    }

    /// <summary>
    /// Egalement utilisé par l'interrupteur pour recréer des liens
    /// </summary>
    /// <param name="connexion1"></param>
    /// <param name="connexion2"></param>
    /// <param name="link"></param>
    public void PlaceLink(Connexion connexion1, Connexion connexion2, Link link)
    {
        firstConnexion = connexion1;
        secondConnexion = connexion2;
        currentLink = link;

        //Est ce que la connexion du premier objet accepte des connexions du deuxième
        if (firstConnexion.typeConnexion.CanBeConnectWith(secondConnexion.typeConnexion))
        {
            currentLink.SetFirstConnexion(firstConnexion);
            currentLink.SetSecondConnexion(secondConnexion);                                               //On met à jour le lien Visuel

            CheckDistance();                                                                                //On recalcule la distance mais avec le nouvel objet

            if (linkCanBeComplete)                                                                          //Si a la bonne distance
            {
                if (!ConfigurerConnexion())                                                                  //valide la connexion et la fait électriquement si enginsElec ou renvoie false si la connexion existait déjà
                    Stop();
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

    /// <summary>
    /// Demande la suppression d'un lien
    /// </summary>
    /// <param name="link"></param>
    public void RemoveLink(Link link)
    {
        Connexion firstCo = link.GetFirstConnexion();
        Connexion secondCo = link.GetSecondConnexion();

        firstCo?.RemoveConnexion(secondCo);
        secondCo?.RemoveConnexion(firstCo);

        if (link.GetTypeLink() == TypeLink.Electric)
            RegenererReseauElec(firstCo, secondCo);

        Destroy(link.gameObject);
    }

    /// <summary>
    /// Gère la partie électrique et réseau à la déconnexion entre 2 objets
    /// </summary>
    /// <param name="firstCo"></param>
    /// <param name="secondCo"></param>
    public void RegenererReseauElec(Connexion firstCo, Connexion secondCo)
    {
        ReseauElec reseauMaitreActuel = firstCo.GetComponent<EnginElec>().reseauMaitre;

        List<EnginElec> enginsReseau1 = new List<EnginElec>();
        //On recupère recursivement tout les engins attachés au premier objet
        firstCo.GetAllEnginsConnected(enginsReseau1, reseauMaitreActuel);

        List<EnginElec> enginsReseau2 = new List<EnginElec>();
        //On recupère recursivement tout les engins attachés au dexième objet
        secondCo.GetAllEnginsConnected(enginsReseau2, reseauMaitreActuel);

        //RAPPEL : Si un objet conetenait le reseau maitre il a été placé au début de la liste
        //Si une des deux listes n'a pas le réseau maitre à l'indice [0], c'est qu'il n'y a plus de chemin entre les deux réseaux
        if (enginsReseau1[0].GetComponent<ReseauElec>() != reseauMaitreActuel || enginsReseau2[0].GetComponent<ReseauElec>() != reseauMaitreActuel)
        {
            List<EnginElec> enginsSansReseauMaitre = enginsReseau1[0].GetComponent<ReseauElec>() != reseauMaitreActuel ? enginsReseau1 : enginsReseau2;

            enginsSansReseauMaitre[0].GetComponent<ReseauElec>().CreationReseau(enginsSansReseauMaitre);

            reseauMaitreActuel.DeleteEnginFromLists(enginsSansReseauMaitre);
        }
    }

    private void CheckDistance()
    {
        if (currentLink != null)
        {
            float dist = Vector3.Distance(currentLink.GetTransform1().position, currentLink.GetTransform2().position);
            if (dist > distanceMaxLink + toleranceLinkBreak)
                Stop();
            else if (dist > distanceMaxLink)
                linkCanBeComplete = false;
            else
                linkCanBeComplete = true;
        }
    }

    private bool ConfigurerConnexion()
    {
        if (!firstConnexion.AddConnexion(secondConnexion, currentLink)) //Si on arrive pas à ajouter la connexion
            return false;

        if (!secondConnexion.AddConnexion(firstConnexion, currentLink)) //Si on arrive pas à ajouter la connexion
            return false;

        ConnexionEnginsElec(firstConnexion, secondConnexion, currentLink); //On tente de connecter électriquement

        currentLink.LinkCompleted();
        ResetVariables();
        return true;
    }

    /// <summary>
    /// Permet de gerer les relations electrique dans objet (attribution ,du bon reseau electrique, ...)
    /// </summary>
    /// <param name="engin1"></param>
    /// <param name="engin2"></param>
    public void ConnexionEnginsElec(Connexion connexion1, Connexion connexion2, Link link)
    {
        EnginElec enginElec1 = connexion1.GetComponent<EnginElec>();
        EnginElec enginElec2 = connexion2.GetComponent<EnginElec>();

        if (enginElec1 != null && enginElec2 != null)
        {
            link.SetTypeOfLink(TypeLink.Electric);
            if (enginElec1.reseauMaitre != enginElec2.reseauMaitre) //Si pas déjà connectés sur le même réseau
            {
                //On garde le plus gros reseau
                ReseauElec reseauAGarder = enginElec1.reseauMaitre.NbEngins > enginElec2.reseauMaitre.NbEngins ? enginElec1.reseauMaitre : enginElec2.reseauMaitre;
                enginElec1.reseauMaitre.ChangementReseau(reseauAGarder);
                enginElec2.reseauMaitre.ChangementReseau(reseauAGarder);
                reseauAGarder.actif = true;
                reseauAGarder.ProcessReseau(); //MAJ du réseau et de l'état des engins
            }
        }
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
        firstConnexion?.RemoveConnexion(secondConnexion);
        secondConnexion?.RemoveConnexion(firstConnexion);
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
