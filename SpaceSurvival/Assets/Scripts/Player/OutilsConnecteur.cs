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


                            if (linkCanBeComplete)      //Si a la bonne distance
                            {
                                if (!CompleteConnection())                             //valide la connexion et la fait électriquement si enginsElec ou renvoie false si la connexion existait déjà
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
                }
                //TOUCHE UN LINK (pour le supprimer)
                if (Input.GetButtonDown("MouseRight") && currentLink == null && _hitInfo.transform.root.GetComponentInChildren<Link>(true) != null)
                {
                    Link linkTouched = _hitInfo.transform.parent.GetComponentInChildren<Link>(true);
                    Connexion firstCo = linkTouched.firstGameObject.GetComponent<Connexion>();
                    Connexion secondCo = linkTouched.secondGameObject.GetComponent<Connexion>();

                    firstCo.RemoveConnexion(secondCo);
                    if (secondCo != null) secondCo.RemoveConnexion(firstCo);

                    if (linkTouched.GetTypeLink() == TypeLink.Electric)
                        DestructElectricLink(firstCo, secondCo);

                    linkTouched.DisconnectLink();
                    Destroy(linkTouched.gameObject);
                }
            }
            if (Input.GetButtonDown("MouseRight") && currentLink != null)                                               //Si clic droit quand on a un lien en main
                Stop();

            CheckDistance();
        }
    }

    private void DestructElectricLink(Connexion firstCo, Connexion secondCo)
    {
        ReseauElec reseauMaitreActuel = firstCo.GetComponent<EnginElec>().reseauMaitre;

        List<EnginElec> enginsReseau1 = new List<EnginElec>();
        //On recupère recursivement tout les engins attachés au premier objet
        firstCo.GetAllEnginsConnected(enginsReseau1, reseauMaitreActuel);

        List<EnginElec> enginsReseau2 = new List<EnginElec>();
        //On recupère recursivement tout les engins attachés au dexième objet
        secondCo.GetAllEnginsConnected(enginsReseau2, reseauMaitreActuel);

        //Si un objet conetenait le reseau maitre il a été placé au début
        //Si un des deux objets ne possède pas d'engin qui possède le reseau mairte c'est qu'il n'y a plus aucun chemin entre les deux reseaux
        if (enginsReseau1[0].GetComponent<ReseauElec>() != reseauMaitreActuel || enginsReseau2[0].GetComponent<ReseauElec>() != reseauMaitreActuel)
        {
            List<EnginElec> enginsSansReseauMaitre = enginsReseau1[0].GetComponent<ReseauElec>() != reseauMaitreActuel ? enginsReseau1 : enginsReseau2;

            enginsSansReseauMaitre[0].GetComponent<ReseauElec>().CreationReseau(enginsSansReseauMaitre);

            reseauMaitreActuel.DeleteEnginToLists(enginsSansReseauMaitre);
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

    /// <summary>
    /// Permet de gerer les relations electrique dans objet (attribution ,du bon reseau electrique, ...)
    /// </summary>
    /// <param name="engin1"></param>
    /// <param name="engin2"></param>
    private void ConnexionEnginElec(EnginElec engin1, EnginElec engin2)
    {
        if (engin1.reseauMaitre != engin2.reseauMaitre)
        {
            //On garde le plus gros reseau
            ReseauElec reseauAGarder = engin1.reseauMaitre.NbEngins > engin2.reseauMaitre.NbEngins ? engin1.reseauMaitre : engin2.reseauMaitre;
            engin1.reseauMaitre.ChangementReseau(reseauAGarder);
            engin2.reseauMaitre.ChangementReseau(reseauAGarder);
            reseauAGarder.actif = true;
        }
    }

    private bool CompleteConnection()
    {
        if (!firstConnexion.AddConnexion(secondConnexion))                                                               //Si on arrive pas à ajouter la connexion
            return false;

        if (!secondConnexion.AddConnexion(firstConnexion))                                                               //Si on arrive pas à ajouter la connexion
            return false;

        //Si les deux objets liés sont tout deux des engins electriques
        if (firstConnexion.GetComponent<EnginElec>() != null && secondConnexion.GetComponent<EnginElec>() != null)
        {
            currentLink.SetTypeOfLink(TypeLink.Electric);
            ConnexionEnginElec(firstConnexion.GetComponent<EnginElec>(), secondConnexion.GetComponent<EnginElec>());
        }

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
