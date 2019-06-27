using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsPositionneur : MonoBehaviour
{
    public Prop_SO prop;
    public LayerMask layerMask;

    private Transform _transform;


    public void Placer(PropsSpawner propsSpawner, MinMax minMaxPlanet)
    {
        _transform = GetComponent<Transform>();

        RaycastHit hit;
        //Debug.DrawRay(transform.position, (Vector3.zero - transform.position).normalized * Vector3.Distance(transform.position, Vector3.zero), Color.blue, 50);
        if (Physics.Raycast(_transform.position, (Vector3.zero - _transform.position).normalized, out hit, Vector3.Distance(_transform.position, Vector3.zero), layerMask))
        {
            if (hit.collider.tag == "Ground" && (Vector3.Distance(Vector3.zero, hit.point) < minMaxPlanet.Min + 3)) //Si ce qu'on a touché est le sol et que ca se touve à moins de 3 mètres au dessus de la surface
            {
                //Place sur le sol
                _transform.position = hit.point;

                //Oriente
                _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.FromToRotation(_transform.up, (_transform.position - Vector3.zero).normalized) * _transform.rotation, 50 * Time.deltaTime);

                gameObject.isStatic = true;
                Destroy(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);
    }
}
