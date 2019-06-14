using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UICoffre : MonoBehaviour, I_UIInventaire
{
    public Inventaire inventaire;
    public GameObject inventairePanel;
    public GameObject coffrePanel;
    public GameObject prefabCaseInventaire;
    public UIDetailsItemInventaire detailsPanel;
    public TextMeshProUGUI coffreCapacityText;
    public TextMeshProUGUI inventaireCapacityText;


    private Coffre _coffre;
    private List<Recoltable> _itemsInventaire;
    private List<Recoltable> _itemsCoffre;

    public Coffre Coffre { get => _coffre; set => _coffre = value; }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDisable()
    {
        _coffre = null;
    }

    //Appelé par UICaseInventaire lorsqu'on clic dessus
    public void ItemClicked(Recoltable item, PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            AfficherInfos(item);
        }
        else
        {
            if (_itemsInventaire.Contains(item)) //Si l'item cliqué est dans l'inventaire
            {
                if (_coffre.AjouterItem(item)) //S'il reste de la place, l'objet est ajouté
                    inventaire.RetirerItem(item); //Vu que l'objet a pu être ajouté au coffre, on le retire de l'inventaire
            }
            else if (_itemsCoffre.Contains(item)) //Si l'item cliqué est dans le coffre
            {
                if (inventaire.AjouterItem(item)) //Idem qu'au dessus
                    _coffre.RetirerItem(item);
            }
            Refresh();
        }
    }


    private void Refresh()
    {
        _itemsInventaire = inventaire.recoltables;
        _itemsCoffre = _coffre.GetRecoltables();

        CleanPanel(inventairePanel);
        CleanPanel(coffrePanel);

        FillPanel(inventairePanel, _itemsInventaire);
        FillPanel(coffrePanel, _itemsCoffre);

        UpdateCapacityTexts();

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

    private void UpdateCapacityTexts()
    {
        inventaireCapacityText.text = _itemsInventaire.Count + " / " + inventaire.capacity;
        coffreCapacityText.text = _itemsCoffre.Count + " / " + _coffre.GetCapacity();
    }

    public void AfficherInfos(Recoltable item)
    {
        detailsPanel.AfficherInfos(item);
    }

}
