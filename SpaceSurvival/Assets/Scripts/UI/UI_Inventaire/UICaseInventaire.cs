using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UICaseInventaire : EventTrigger
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    private I_UIInventaire _inventaire; //Le script qui a créé cette case et qui doit être averti des clics
    private Recoltable _item;

    public I_UIInventaire Inventaire
    {
        set { _inventaire = value; }
    }

    public Recoltable Item
    {
        set
        {
            _item = value;
            _image.sprite = _item.data.sprite;
            _text.text = _item.data.nom;
        }
    }

    public override void OnPointerClick(PointerEventData data)
    {
        _inventaire.ItemClicked(_item, data);
    }
}
