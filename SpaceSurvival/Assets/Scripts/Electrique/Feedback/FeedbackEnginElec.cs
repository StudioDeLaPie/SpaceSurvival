using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackEnginElec : MonoBehaviour
{
    private enum TypeEngin { generateur, consommateur, batterie }
    public enum EtatEngin { onOK, offOK, onPanne, offPanne }

    public TextMeshProUGUI txtLabel;
    public TextMeshProUGUI txtValue;
    public LED led;

    private EnginElec engin;
    private TypeEngin type;
    private EtatEngin etat;

    // Start is called before the first frame update
    void Start()
    {
        EnginElec _engin = GetComponent<EnginElec>();
        if (_engin is GenerateurElec)
        {
            engin = (GenerateurElec)_engin;
            type = TypeEngin.generateur;
            txtLabel.text = "Production :";
        }
        else if (_engin is ConsoElec)
        {
            engin = (ConsoElec)_engin;
            type = TypeEngin.consommateur;
            txtLabel.text = "Consommation :";
        }
        else if (_engin is BatterieElec)
        {
            engin = (BatterieElec)_engin;
            type = TypeEngin.batterie;
            txtLabel.text = "Energie :";
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
                txtValue.text = ((GenerateurElec)engin).GetProduction() + " / " + ((GenerateurElec)engin).reseauMaitre.ConsoTotale;
                break;
            case TypeEngin.consommateur:
                txtValue.text = ((ConsoElec)engin).GetConsommation() + " / " + ((ConsoElec)engin).consommation;
                break;
            case TypeEngin.batterie:
                txtValue.text = "Pas fait. [FeedbackEnginElec.cs]";
                break;
        }
    }

    private void RefreshEtat()
    {
        switch (type)
        {
            case TypeEngin.generateur:
                if (((GenerateurElec)engin).GetAlimentationSuffisante())
                    if (((GenerateurElec)engin).ON_OffElec)
                        etat = EtatEngin.onOK;
                    else
                        etat = EtatEngin.offOK;
                else
                {
                    if (((GenerateurElec)engin).ON_OffElec)
                        etat = EtatEngin.onPanne;
                    else
                        etat = EtatEngin.offPanne;
                }
                break;

            case TypeEngin.consommateur:
                if (((ConsoElec)engin).GetAlimentationSuffisante())
                    if (((ConsoElec)engin).ON_OffElec)
                        etat = EtatEngin.onOK;
                    else
                        etat = EtatEngin.offOK;
                else
{
                    if (((ConsoElec)engin).ON_OffElec)
                        etat = EtatEngin.onPanne;
                    else
                        etat = EtatEngin.offPanne;
                }
                break;
            case TypeEngin.batterie:
                etat = EtatEngin.onPanne;
                Debug.Log("Feedback des batteries à faire");
                break;
        }
    }
}