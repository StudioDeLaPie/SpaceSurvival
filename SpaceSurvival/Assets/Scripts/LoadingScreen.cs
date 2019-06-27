using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI textProgress;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void PercentPropsPlaced(float percent)
    {
        textProgress.text = "Placement aléatoire des mines";
        slider.value = percent;
        //Debug.Log(percent);
    }
}
