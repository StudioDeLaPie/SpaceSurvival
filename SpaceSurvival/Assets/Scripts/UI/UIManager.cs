﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject UIPlayer;
    public GameObject UIInventaire;
    public GameObject UICoffre;

    [Space]
    public List<MonoBehaviour> ComposantsADesactiverEnPause;
    [Space]
    public MovementController movementController;

    private MouseLook mouseLook;
    private bool cursorLocked;
    private bool enJeu;
    private UICoffre _uiCoffre;

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

    /// <summary>
    /// Appelé par Aiming pour avertir qu'on vise un coffre
    /// </summary>
    /// <param name="coffre"></param>
    public void AimingCoffre(Coffre coffre)
    {
        if (Input.GetButtonUp("Action"))
            OuvertureCoffre(coffre);
    }

    public void AimingImprimante(Imprimante i)
    {
        if (Input.GetButtonDown("Action"))
            Debug.Log("Imprimante UI");
    }

    public void OuvertureCoffre(Coffre coffre)
    {
        EnJeu(false);
        _uiCoffre.Coffre = coffre;
        UICoffre.SetActive(true);
    }

    public void FermetureCoffre()
    {
        UICoffre.SetActive(false);
        EnJeu(true);
    }


    private void Start()
    {
        _uiCoffre = UICoffre.GetComponent<UICoffre>();
        mouseLook = movementController.mouseLook;
        EnJeu(true);
        UIInventaire.SetActive(false);
        UICoffre.SetActive(false);
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
         * si coffreOuvert && (echap ou Action) //Vu que c'est Action qui a permis d'ouvrir le coffre
         *      fermer coffre
         *      EnJeu true
         * 
         * */
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
                UIInventaire.SetActive(true);
            }
        }
        else if (UIInventaire.activeSelf && (Input.GetButtonUp("Escape") || Input.GetButtonUp("OuvrirInventaire")))
        {
            UIInventaire.SetActive(false);
            EnJeu(true);
        }
        else if (UICoffre.activeSelf && (Input.GetButtonUp("Action") || Input.GetButtonUp("Escape")))
        {
            Debug.Log("Fermeture coffre");
            FermetureCoffre();
            EnJeu(true);
        }
    }

    /// <summary>
    /// Si on est en jeu, les outils sont activés, la souris bloquée et l'UIPlayer affichée
    /// </summary>
    /// <param name="etat"></param>
    private void EnJeu(bool etat)
    {
        UIPlayer.SetActive(etat);

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
