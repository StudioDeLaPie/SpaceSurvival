﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject UIPlayer;
    public GameObject UIInventaire;

    [Space]
    public List<MonoBehaviour> ComposantsADesactiverEnPause;
    [Space]
    public MovementController movementController;

    private MouseLook mouseLook;
    private bool cursorLocked;
    private bool enJeu;

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

    private void Start()
    {
        mouseLook = movementController.mouseLook;
        EnJeu(true);
        UIInventaire.SetActive(false);
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
         *      enJeu true
         *      souris bloquée
         * 
         * 
         * */
         if (enJeu)
        {
            if (cursorLocked && Input.GetButtonDown("Escape"))
            {
                CursorLocked = false;
            }
            else if (!cursorLocked && (Input.GetButtonDown("Escape") || ClicSurJeuHorsUI()))
            {
                CursorLocked = true;
            }

            if (Input.GetButtonDown("OuvrirInventaire"))
            {
                EnJeu(false);
                UIInventaire.SetActive(true);
            }
        }
         else if (UIInventaire.activeSelf && (Input.GetButtonDown("Escape") || Input.GetButtonDown("OuvrirInventaire")))
        {
            UIInventaire.SetActive(false);
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
