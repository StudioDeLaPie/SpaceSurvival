using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDetailsItemInventaire : MonoBehaviour
{
    public TextMeshProUGUI nom;
    public TextMeshProUGUI description;
    public Image image;

    private void Start()
    {
        Clean();
    }

    public void AfficherInfos(Recoltable recoltable)
    {
        if (recoltable == null)
            Clean();
        else
        {
            nom.text = recoltable.data.nom;
            description.text = recoltable.data.description;
            image.sprite = recoltable.data.sprite;
        }
    }

    public void Clean()
    {
        nom.text = "Nom";
        description.text = "Description";
        image.sprite = null;
    }
}
