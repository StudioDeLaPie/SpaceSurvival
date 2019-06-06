using System.Collections;
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
        if (Input.GetButtonDown("OuvrirInventaire"))
        {
            EnJeu(!enJeu);
            UIInventaire.SetActive(!UIInventaire.activeSelf);
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

    ///INUTILE POUR LE MOMENT
    //When Touching UI
    //private bool IsPointerOverUIObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}
}
