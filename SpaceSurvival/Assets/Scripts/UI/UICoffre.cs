using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICoffre : MonoBehaviour, I_Inventaire
{
    public Inventaire inventaire;
    public GameObject inventairePanel;
    public GameObject coffrePanel;
    public GameObject prefabCaseInventaire;
    public UIDetailsItemInventaire detailsPanel;


    private Coffre _coffre;
    private List<Recoltable> _itemsInventaire;
    private List<Recoltable> _itemsCoffre;

    public Coffre Coffre { set => _coffre = value; }

    private void OnEnable()
    {
        if (_coffre == null)
        {
            Debug.LogError("Coffre non défini");
            return;
        }
        _itemsInventaire = inventaire.recoltables;
        _itemsCoffre = _coffre.GetRecoltables();
        Refresh();
    }

    private void OnDisable()
    {
        _coffre = null;
    }

    public void ItemClicked(Recoltable item, PointerEventData pointerEventData)
    {
        throw new System.NotImplementedException();
    }


    private void Refresh()
    {
        CleanPanel(inventairePanel);
        CleanPanel(coffrePanel);

        FillPanel(inventairePanel, _itemsInventaire);
        FillPanel(coffrePanel, _itemsCoffre);
        detailsPanel.Clean();
    }

    //Supprime toutes les cases d'un panel
    private void CleanPanel(GameObject panel)
    {
        foreach (RectTransform rect in panel.transform)
        {
            Destroy(rect.gameObject);
        }
    }

    //Remplit le panel donné avec les items donnés
    private void FillPanel(GameObject panel, List<Recoltable> items)
    {
        foreach (Recoltable item in items)
        {
            UICaseInventaire uiCase = Instantiate(prefabCaseInventaire, panel.transform).GetComponent<UICaseInventaire>();
            uiCase.Inventaire = this;
            uiCase.Item = item;
        }
    }

}
