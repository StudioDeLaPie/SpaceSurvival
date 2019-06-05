using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(GravityBody))]
public class PropsPositionneur : MonoBehaviour
{
    public Prop prop;

    public delegate void PropPlaced();
    public event PropPlaced OnPropPlaced;

    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        OnPropPlaced();
        if (collision.collider.tag == "Ground" && Vector3.Angle(collision.contacts[0].normal, transform.up) < 45)
        {
            Destroy(GetComponent<GravityBody>());
            Destroy(_rb);

            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo, prop.envergure))
            {
                transform.up = hitInfo.normal;
                transform.Translate(-transform.up * hitInfo.distance);
            }

            Destroy(GetComponent<Collider>());
            Destroy(GetComponentInChildren<Collider>());
            gameObject.isStatic = true;
        }
        else
            Destroy(gameObject);
    }
}
