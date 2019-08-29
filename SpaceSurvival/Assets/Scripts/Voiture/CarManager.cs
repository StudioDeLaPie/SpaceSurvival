using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{

    [SerializeField] private CarController _controller;
    [SerializeField] private CarAirControl _airControl;
    [SerializeField] private CarControlsCustom _controlsCustom;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _getOffPosition;
    private bool activated = false;

    private GameObject _player;

    private void Start()
    {
        _controller.enabled = false;
        _controlsCustom.enabled = false;
        _camera.SetActive(false);
    }

    public void GetInCar(GameObject player)
    {
        _player = player;
        _player.SetActive(false);

        _controller.enabled = true;
        _controlsCustom.enabled = true;
        _camera.SetActive(true);

        activated = true;
    }

    private void Update()
    {
        if (activated && Input.GetButtonDown("Action"))
        {
            GetOffCar();
        }
    }

    private void GetOffCar()
    {
        _controller.enabled = false;
        _controlsCustom.enabled = false;
        _camera.SetActive(false);

        _player.transform.position = _getOffPosition.position;
        _player.SetActive(true);

        activated = false;
    }
}
