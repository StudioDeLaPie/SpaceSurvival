using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Craft_SO;

public class UIImprimanteRecettePanel : MonoBehaviour
{
    public GameObject caseComposantRecettePrefab;
    [SerializeField] private TextMeshProUGUI textNom;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private RectTransform panelComposants;
    private Imprimante _imprimante;
    private Craft_SO _recette;


    public void Init(Craft_SO recette, Imprimante imprimante)
    {
        _imprimante = imprimante;
        _recette = recette;
        textNom.text = recette.nom;
        textDescription.text = recette.description;

        ClearComposantsPanel();
        FillComposantsPanel(recette.composants);
    }
    
    public void OnCraftClick()
    {
        _imprimante.Craft(_recette);
        ClearComposantsPanel();
        FillComposantsPanel(_recette.composants);
    }

    private void ClearComposantsPanel()
    {
        foreach (RectTransform rect in panelComposants.transform)
        {
            Destroy(rect.gameObject);
        }
    }

    private void FillComposantsPanel(List<ComposantRecette> composants)
    {
        foreach (ComposantRecette composant in composants)
        {
            Instantiate(caseComposantRecettePrefab, panelComposants).GetComponent<UICaseComposantRecette>().Init(composant.recoltable, composant.quantity, _imprimante.NbDisponible(composant.recoltable));
        }
    }
}
