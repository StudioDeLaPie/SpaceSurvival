using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarControlsCustom : MonoBehaviour
{
    public float jumpForce;
    public float boostForce;

    private CarController _car; // the car controller we want to use
    private CarAirControl _airControl;
    private Rigidbody _rb;
    private bool grounded = false;

    // Start is called before the first frame update
    void Awake()
    {
        _car = GetComponent<CarController>();
        _airControl = GetComponent<CarAirControl>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // pass the input to the car!
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        float handbrake = 0;
        _car.Move(h, v, v, handbrake);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Sprint"))
        {
            _rb.AddForce(transform.forward * boostForce, ForceMode.Impulse);
        }
        if (grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _rb.AddForce((transform.up + _rb.velocity.normalized / 2) * jumpForce, ForceMode.Impulse);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
        _airControl.enabled = false;
        _car.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        grounded = false;
        _airControl.enabled = true;
        _car.enabled = false;
    }
}
