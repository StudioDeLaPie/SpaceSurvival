using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICaseRecette : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color couleurFaisable;
    [SerializeField] private Color couleurInfaisable;

    private Craft_SO _recette;
    private UIImprimante _uiImprimante;

    public void Init(Craft_SO recette, UIImprimante uIImprimante, bool faisable)
    {
        GetComponent<Image>().color = faisable ? couleurFaisable : couleurInfaisable;
        _recette = recette;
        _image.sprite = _recette.sprite;
        _text.text = _recette.nom;
        _uiImprimante = uIImprimante;
    }

    public void OnClick()
    {
        _uiImprimante.OnRecetteClick(_recette);
    }
}
