using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventaire : MonoBehaviour
{
    public Inventaire inventaire;
    public GameObject itemsPanel;
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
            GameObject obj = Instantiate(prefabCaseInventaire, itemsPanel.transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = item.data.nom;
            obj.GetComponentInChildren<Image>().sprite = item.data.sprite;
        }
    }

}
