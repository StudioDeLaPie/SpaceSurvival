using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public float weight = 1;

    private GravityAttractor attractor;
    private Transform myTransform;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        myTransform = transform;
        attractor = GameObject.FindGameObjectWithTag("Ground").GetComponent<GravityAttractor>();
    }

    // Update is called once per frame
    void Update()
    {
        attractor.Attract(myTransform, rb, weight);
    }
}
