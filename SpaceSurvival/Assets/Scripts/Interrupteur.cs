using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour, I_Elec
{
    private OutilsConnecteur connecteur;

    private void Start()
    {
        connecteur = GameObject.FindGameObjectWithTag("Player").GetComponent<OutilsConnecteur>();
    }

    public void TurnOff()
    {
        
    }

    public void TurnOn()
    {
        
    }
}
