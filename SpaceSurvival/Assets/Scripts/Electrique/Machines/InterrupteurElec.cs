using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrupteurElec : ConsoElec
{

    /// <summary>
    /// Override de la base pour ne pas prendre en compte si l'alimentation est suffisante ou non (on s'en moque pour un interrupteur)
    /// </summary>
    /// <param name="state"></param>
    public override void ActiveEngin(bool state)
    {
        ON_OffElec = state;
        if (ON_OffElec)
            engin.TurnOn();
        else
            engin.TurnOff();
    }

    /// <summary>
    /// Méthode appelée lors de la connexion. Si interrupteur éteint, rappelle TurnOff pour désactiver les liens.
    /// </summary>
    public override void CheckReseauToTurnOn()
    {
        if (!ON_OffElec) 
            engin.TurnOff();
    }

    /// <summary>
    /// idem
    /// </summary>
    /// <param name="suffisant"></param>
    public override void AlimentationSuffisante(bool suffisant)
    {
        
    }
}
