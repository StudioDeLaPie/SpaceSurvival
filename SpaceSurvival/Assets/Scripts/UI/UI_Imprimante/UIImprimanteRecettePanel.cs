using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Craft_SO;

public class UIImprimanteRecettePanel : MonoBehaviour
{
    public GameObject caseComposantRecettePrefab;
    [SerializeField] private TextMeshProUGUI textNom;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private RectTransform panelComposants;
    [SerializeField] private Button btnCraft;
    private Imprimante _imprimante;
    private UIImprimante _uiImprimante;
    private Craft_SO _recette;


    public void Init(Craft_SO recette, UIImprimante uiImprimante, Imprimante imprimante)
    {
        _imprimante = imprimante;
        _uiImprimante = uiImprimante;
        _recette = recette;
        textNom.text = recette.nom;
        textDescription.text = recette.description;

        ClearComposantsPanel();
        FillComposantsPanel(recette.composants);
    }

    public void Clear()
    {
        textNom.text = "";
        textDescription.text = "";
        ClearComposantsPanel();
        btnCraft.interactable = false;
    }
    
    public void OnCraftClick()
    {
        _uiImprimante.OnCraftBtnClick(_recette);
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
        btnCraft.interactable = true;
        foreach (ComposantRecette composant in composants)
        {
            UICaseComposantRecette uiCase = Instantiate(caseComposantRecettePrefab, panelComposants).GetComponent<UICaseComposantRecette>();
            int quantiteDispo = _imprimante.QuantiteDisponible(composant.recoltable);
            uiCase.Init(composant.recoltable, composant.quantity, quantiteDispo);

            if (composant.quantity > quantiteDispo) //Si un des composants n'est pas disponible en assez grande quantité
                btnCraft.interactable = false;      //On empêche le craft
        }
    }
}
