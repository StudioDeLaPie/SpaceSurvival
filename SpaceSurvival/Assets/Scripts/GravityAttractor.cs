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

    public void Attract(Transform body, Rigidbody bodyRb, float weight, bool standUp)
    {
        //ATTRACTION
        Vector3 gravityUp = (body.position - myTransform.position).normalized;
        bodyRb.AddForce(gravityUp * gravity * weight);

        //ROTATION POUR RESTER DEBOUT AUTOUR DE LA PLANETE
        if (standUp)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(body.up, gravityUp) * body.rotation;
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
        }
    }
}
