using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIInventaire : MonoBehaviour, I_Inventaire
{
    public Inventaire inventaire;
    public GameObject itemsPanel;
    public UIDetailsItemInventaire detailsPanel;
    public GameObject prefabCaseInventaire;

    private bool _inventaireOpen;
    private List<Recoltable> _items;

    private void OnEnable()
    {
        _items = inventaire.recoltables;
        Refresh();
    }

    /// <summary>
    /// Appelé par les cases d'inventaire lorsqu'elles sont cliquées
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pointerEventData"></param>
    public void ItemClicked(Recoltable item, PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
            AfficherInfos(item);
        else if (pointerEventData.button == PointerEventData.InputButton.Right)
            LacherItem(item);
    }

    private void Refresh()
    {
        foreach (RectTransform item in itemsPanel.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Recoltable item in _items)
        {
            UICaseInventaire uiCase = Instantiate(prefabCaseInventaire, itemsPanel.transform).GetComponent<UICaseInventaire>();
            uiCase.Inventaire = this;
            uiCase.Item = item;
        }
        detailsPanel.Clean();
    }

    public void LacherItem(Recoltable item)
    {
        inventaire.LacherItem(item);
        Refresh();
    }

    public void AfficherInfos(Recoltable item)
    {
        detailsPanel.AfficherInfos(item);
    }

    
}
