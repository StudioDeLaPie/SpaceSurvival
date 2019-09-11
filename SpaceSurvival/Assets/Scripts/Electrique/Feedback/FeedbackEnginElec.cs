using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FeedbackEnginElec : MonoBehaviour
{
    private enum TypeEngin { generateur, consommateur, batterie }
    public enum EtatEngin { onOK, offOK, onPanne, offPanne }

    public TextMeshProUGUI txtLabel;
    public TextMeshProUGUI txtValue;
    public Slider sliderJauge;
    public LED led;

    private EnginElec engin;
    private TypeEngin type;
    private EtatEngin etat;

    // Start is called before the first frame update
    void Start()
    {
        EnginElec _engin = GetComponent<EnginElec>();
        engin = _engin;
        if (_engin is GenerateurElec)
        {
            type = TypeEngin.generateur;
             if (txtLabel) txtLabel.text = "Production :";
        }
        else if (_engin is ConsoElec)
        {
            type = TypeEngin.consommateur;
            if (txtLabel) txtLabel.text = "Consommation :";
        }
        else if (_engin is BatterieElec)
        {
            type = TypeEngin.batterie;
            if (txtLabel) txtLabel.text = "Energie stockée :";
            sliderJauge.maxValue = ((BatterieElec)engin).QuantiteElectriciteMax;
        }
    }

    void LateUpdate()
    {
        RefreshFeedback();
    }

    private void RefreshFeedback()
    {
        RefreshEtat();
        led.SetEtat(etat);
        switch (type)
        {
            case TypeEngin.generateur:
                if (txtValue) txtValue.text = ((GenerateurElec)engin).GetProduction() + " / " + engin.reseauMaitre.ConsoTotale;
                break;
            case TypeEngin.consommateur:
                if (txtValue) txtValue.text = ((ConsoElec)engin).GetConsommation() + " / " + engin.reseauMaitre.ProdTotale;
                break;
            case TypeEngin.batterie:
                if (txtValue) txtValue.text = ((BatterieElec)engin).QuantiteElectricite.ToString("F0") + " / " + ((BatterieElec)engin).QuantiteElectriciteMax;
                sliderJauge.value = ((BatterieElec)engin).QuantiteElectricite;
                break;
        }
    }

    private void RefreshEtat()
    {
        if (engin.GetAlimentationSuffisante())
            if (engin.ON_OffElec)
                etat = EtatEngin.onOK;
            else
                etat = EtatEngin.offOK;
        else
        {
            if (engin.ON_OffElec)
                etat = EtatEngin.onPanne;
            else
                etat = EtatEngin.offPanne;
        }
        //    switch (type)
        //    {
        //        case TypeEngin.generateur:
        //            if (engin.GetAlimentationSuffisante())
        //                if (engin.ON_OffElec)
        //                    etat = EtatEngin.onOK;
        //                else
        //                    etat = EtatEngin.offOK;
        //            else
        //            {
        //                if (engin.ON_OffElec)
        //                    etat = EtatEngin.onPanne;
        //                else
        //                    etat = EtatEngin.offPanne;
        //            }
        //            break;

        //        case TypeEngin.consommateur:
        //            if (engin.GetAlimentationSuffisante())
        //                if (engin.ON_OffElec)
        //                    etat = EtatEngin.onOK;
        //                else
        //                    etat = EtatEngin.offOK;
        //            else
        //            {
        //                if (engin.ON_OffElec)
        //                    etat = EtatEngin.onPanne;
        //                else
        //                    etat = EtatEngin.offPanne;
        //            }
        //            break;
        //        case TypeEngin.batterie:
        //            etat = EtatEngin.onPanne;
        //            Debug.Log("Feedback des batteries à faire");
        //            break;
        //    }
    }
}