using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAirControl : MonoBehaviour
{
    public float angularDragInAir;

    private Rigidbody _rb;

    private float defaultAngDrag;

    private void OnEnable()
    {
        _rb.angularDrag = angularDragInAir;
    }

    private void OnDisable()
    {
        _rb.angularDrag = defaultAngDrag;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        defaultAngDrag = _rb.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float lacet = Input.GetAxis("Lacet");
        _rb.angularVelocity = transform.TransformDirection(v, lacet, -h);
    }
}
