using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public float weight = 1;
    public bool standUp = true;

    private GravityAttractor _attractor;
    private Transform _transform;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.useGravity = false;
        _transform = transform;
        _attractor = GameObject.FindGameObjectWithTag("Planete").GetComponent<GravityAttractor>();
    }

    // Update is called once per frame
    void Update()
    {
        _attractor.Attract(_transform, _rb, weight, standUp);
    }
}
