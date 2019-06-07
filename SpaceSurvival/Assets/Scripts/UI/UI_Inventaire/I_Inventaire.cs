using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface I_Inventaire
{
    void ItemClicked(Recoltable item, PointerEventData pointerEventData);
}
