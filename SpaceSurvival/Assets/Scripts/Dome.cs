using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    [SerializeField] private bool ON_OFF = false;

    [Space]
    public GameObject brasAllume;
    public GameObject brasEteint;

    private void Start()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        ON_OFF = true;
        brasAllume.SetActive(true);
        brasEteint.SetActive(false);
    }

    public void TurnOff()
    {
        ON_OFF = false;
        brasAllume.SetActive(false);
        brasEteint.SetActive(true);
    }
}
