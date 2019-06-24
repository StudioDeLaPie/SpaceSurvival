using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICaseComposantRecette : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textNom;
    [SerializeField] private TextMeshProUGUI textQuantite;
    [SerializeField] private Color couleurValide;
    [SerializeField] private Color couleurInvalide;
    private Image caseBg;
    private Recoltable_SO recoltable;

    public void Init(Recoltable_SO composant, int nbRequis, int nbPossede)
    {
        caseBg = GetComponent<Image>();
        caseBg.color = nbPossede >= nbRequis ? couleurValide : couleurInvalide;
        recoltable = composant;
        image.sprite = recoltable.sprite;
        textNom.text = recoltable.nom;
        textQuantite.text = nbPossede + " / " + nbRequis;
    }
}
