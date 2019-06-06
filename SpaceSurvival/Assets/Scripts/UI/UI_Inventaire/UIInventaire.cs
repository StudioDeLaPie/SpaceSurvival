using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventaire : MonoBehaviour
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

    private void Refresh()
    {
        foreach (RectTransform item in itemsPanel.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Recoltable item in _items)
        {
            Instantiate(prefabCaseInventaire, itemsPanel.transform).GetComponent<UICaseInventaire>().Item = item;
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
