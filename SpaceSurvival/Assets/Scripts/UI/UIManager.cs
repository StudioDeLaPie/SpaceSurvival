using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UIPlayer;
    public GameObject UIInventaire;

    private void Start()
    {
        UIPlayer.SetActive(true);
        UIInventaire.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("OuvrirInventaire"))
        {
            UIPlayer.SetActive(!UIPlayer.activeSelf);
            UIInventaire.SetActive(!UIInventaire.activeSelf);
        }
    }
}
