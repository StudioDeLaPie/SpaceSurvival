using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeElec : ConsoElec
{
    private Dome dome;

    private void Start()
    {
        dome = GetComponent<Dome>();
    }

    public override void AlimentationSuffisante(bool suffisant)
    {
        //Si on rentre ici c'est que le reseau vient de changer d'état
        alimentationSuffisante = suffisant;
        
        if(ON_OffElec && suffisant)
        {
            dome.TurnOn();
        }
        else
        {
            dome.TurnOff();
        }
    }

    public new void ActiveEngin(bool state)
    {
        base.ActiveEngin(state);

        if(state && alimentationSuffisante)
        {
            dome.TurnOn();
        }
        else
        {
            dome.TurnOff();
        }
    }

    [ContextMenu("ActiveEngin")]
    public new void ActiveEngin()
    {
        Debug.Log("Activation Dome");
        ActiveEngin(true);
    }

    [ContextMenu("DesactiveEngin")]
    public new void DesactiveEngin()
    {
        Debug.Log("Désactivation Dome");
        ActiveEngin(false);
    }
}
