using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImprimante : MonoBehaviour
{
    public GameObject prefabCaseRecette;
    public RectTransform listRecettesPanel;
    public UIImprimanteRecettePanel recettePanel;

    private Imprimante _imprimante;


    public Imprimante Imprimante
    {
        set
        {
            _imprimante = value;
            _imprimante.Activate();
            Refresh();
            recettePanel.Clear();
        }
    }

    public void OnRecetteClick(Craft_SO recette)
    {
        recettePanel.Init(recette, this, _imprimante);
    }

    public void OnCraftBtnClick(Craft_SO recette)
    {
        _imprimante.Craft(recette);
        Refresh();  //Reset la liste des recettes faisables
        recettePanel.Init(recette, this, _imprimante); //Refresh de la recette
    }

    private void Refresh()
    {
        foreach (RectTransform rect in listRecettesPanel.transform)
        {
            Destroy(rect.gameObject);
        }
        foreach (Craft_SO recette in _imprimante.craftPossibles)
        {
            Instantiate(prefabCaseRecette, listRecettesPanel).GetComponent<UICaseRecette>().Init(recette, this, _imprimante.TestRessourcesNecessaires(recette));
        }
    }

    private void OnDisable()
    {
        if (_imprimante != null)
        {
            _imprimante.Deactivate();
        }
        _imprimante = null;
    }
}
