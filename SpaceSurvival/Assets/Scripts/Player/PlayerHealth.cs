using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxVie;
    private float _vie;

    public float MaxVie { get => _maxVie;}
    public float Vie { get => _vie;}

    private void Start()
    {
        _vie = _maxVie;
    }

    public void PerteVie(float quantity)
    {
        _vie -= quantity;
        if (_vie <= 0)
        {
            _vie = 0;
            Mort();
        }
    }

    private void Mort()
    {
        //Debug.Log("Vous êtes mort");
    }
}
