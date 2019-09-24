using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Debug.Log("Bonjour j'essaye actuellement de quitter ce jeu vraiment cool");
            Application.Quit();
        }
    }
}
