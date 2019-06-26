using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject UIPlayer_go;
    public GameObject UIInventaire_go;
    public GameObject UICoffre_go;
    public GameObject UIImprimante_go;
    public GameObject UIOutilConnecteur_go;

    [Space]
    public List<MonoBehaviour> ComposantsADesactiverEnPause;
    [Space]
    public MovementController movementController;

    private MouseLook mouseLook;
    private bool hideUI = false;
    private bool cursorLocked;
    private bool enJeu;
    private UICoffre _uiCoffre;
    private UIImprimante _uiImprimante;

    public bool CursorLocked
    {
        set
        {
            cursorLocked = value;
            mouseLook.cursorLocked = value;
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void FeedbackOutilConnecteur(bool etat)
    {
        UIOutilConnecteur_go.SetActive(etat);
        UIPlayer_go.GetComponent<UIPlayer>().ShowReticule(!etat);
    }

    // Appelé par Aiming pour avertir qu'on vise un coffre
    public void AimingCoffre(Coffre coffre)
    {
        if (Input.GetButtonUp("Action")) //Ouverture du coffre
        {
            EnJeu(false);
            _uiCoffre.Coffre = coffre;
            UICoffre_go.SetActive(true);
        }
    }

    public void AimingImprimante(Imprimante imprimante)
    {
        if (Input.GetButtonUp("Action") && imprimante.fonctionnel) //Ouverture de l'UI de l'imprimante
        {
            EnJeu(false);
            _uiImprimante.Imprimante = imprimante;
            UIImprimante_go.SetActive(true);
        }
    }


    private void Start()
    {
        _uiCoffre = UICoffre_go.GetComponent<UICoffre>();
        _uiImprimante = UIImprimante_go.GetComponent<UIImprimante>();
        mouseLook = movementController.mouseLook;
        EnJeu(true);
        UIInventaire_go.SetActive(false);
        UICoffre_go.SetActive(false);
        UIImprimante_go.SetActive(false);
        UIOutilConnecteur_go.SetActive(false);
    }

    private void Update()
    {
        /*
         * si enJeu, souris bloquée et que Echap
         *      Débloque souris
         * si enJeu, souris non bloquée et que (echap OU clic sur jeu)
         *      Bloque souris
         * 
         * si enJeu et que OuvrirInventaire
         *      Débloque souris
         *      enJeu false
         *      affiche inventaire
         *      
         * Si inventaire ouvert && (echap ou OuvrirInventaire)
         *      fermer inventaire
         *      EnJeu true
         *      
         * si (coffre ou imprimante Ouvert) && (echap ou Action) //Vu que c'est Action qui a permis d'ouvrir le coffre / imprimante
         *      fermer coffre et imprimante
         *      EnJeu true
         * 
         * */
        if (Input.GetButtonDown("MasquerUI"))
        {
            hideUI = !hideUI;
            GetComponent<Canvas>().enabled = !hideUI;
        }

        if (enJeu)
        {
            if (cursorLocked && Input.GetButtonUp("Escape"))
            {
                CursorLocked = false;
            }
            else if (!cursorLocked && (Input.GetButtonUp("Escape") || ClicSurJeuHorsUI()))
            {
                CursorLocked = true;
            }

            if (Input.GetButtonUp("OuvrirInventaire"))
            {
                EnJeu(false);
                UIInventaire_go.SetActive(true);
            }
        }
        else if (UIInventaire_go.activeSelf && (Input.GetButtonUp("Escape") || Input.GetButtonUp("OuvrirInventaire")))
        {
            UIInventaire_go.SetActive(false);
            EnJeu(true);
        }
        else if ((UICoffre_go.activeSelf || UIImprimante_go.activeSelf) && (Input.GetButtonUp("Action") || Input.GetButtonUp("Escape")))
        {
            UIImprimante_go.SetActive(false);
            UICoffre_go.SetActive(false);
            EnJeu(true);
        }
    }

    /// <summary>
    /// Si on est en jeu, les outils sont activés, la souris bloquée et l'UIPlayer affichée
    /// </summary>
    /// <param name="etat"></param>
    private void EnJeu(bool etat)
    {
        UIPlayer_go.SetActive(etat);

        foreach (MonoBehaviour m in ComposantsADesactiverEnPause)
            m.enabled = etat;
        CursorLocked = etat;
        enJeu = etat;
    }

    /// <summary>
    /// Renvoie vrai si un clic est enregistré et qu'il n'est pas sur un élément d'UI
    /// </summary>
    /// <returns></returns>
    private bool ClicSurJeuHorsUI()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            return !IsPointerOverUIObject();
        else
            return false;
    }

    //When Touching UI
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
