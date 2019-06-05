using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsPositionneur : MonoBehaviour
{
    public Prop prop;

    [HideInInspector] public PropsSpawner propsSpawner;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (GetComponent<Rigidbody>() == null || GetComponent<GravityBody>() == null)
            Debug.LogError("Manque un RigidBody ou un GravityBody");
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground" && Vector3.Angle(collision.contacts[0].normal, transform.up) < 45)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo, prop.envergure))
            {
                transform.up = hitInfo.normal;
                transform.Translate(-transform.up * hitInfo.distance);
            }
            gameObject.isStatic = true;

            Destroy(GetComponent<GravityBody>());
            Destroy(_rb);
            Destroy(GetComponent<Collider>());
            Destroy(GetComponentInChildren<Collider>());
            propsSpawner.PropPlaced();
            Destroy(this);
        }
        else
        {
            propsSpawner.PropPlaced();
            Destroy(gameObject);
        }
    }
}
