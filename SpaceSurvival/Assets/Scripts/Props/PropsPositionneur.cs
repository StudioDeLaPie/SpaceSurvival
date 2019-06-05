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

    protected virtual void OnCollisionStay(Collision collision)
    {
        propsSpawner.PropPlaced();

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
