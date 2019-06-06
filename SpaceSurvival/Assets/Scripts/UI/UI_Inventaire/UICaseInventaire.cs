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
    private UIInventaire _UIInventaire;
    private Recoltable _item;

    private void Start()
    {
        _UIInventaire = GetComponentInParent<UIInventaire>();
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
        Debug.Log("OnPointerClick called.");
        if (data.button == PointerEventData.InputButton.Left)
            _UIInventaire.AfficherInfos(_item);
        else if (data.button == PointerEventData.InputButton.Right)
            _UIInventaire.LacherItem(_item);
    }
}
