using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauSolaire : MonoBehaviour, I_Elec
{
    //Ne s'éteint pas lorsqu'on le déplace pour éviter que la potentielle seule source d'énergie disparaisse et couille tout les autres engins
    public void TurnOff()
    {
    }

    public void TurnOn()
    {
    }
}
