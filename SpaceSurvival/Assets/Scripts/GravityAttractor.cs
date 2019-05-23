using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -10;

    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    public void Attract(Transform body, Rigidbody bodyRb, float weight)
    {
        Vector3 gravityUp = (body.position - myTransform.position).normalized;
        Vector3 bodyUp = body.up;

        bodyRb.AddForce(gravityUp * gravity * weight);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
