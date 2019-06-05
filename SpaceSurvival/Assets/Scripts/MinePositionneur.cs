using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePositionneur : PropsPositionneur
{
    private MinMax minMaxPlanet;
    private Transform _transform;
    public LayerMask layerMask;

    private void Start()
    {
        minMaxPlanet = GameObject.FindGameObjectWithTag("Planete").GetComponent<Planet>().GetMinMax;
        _transform = GetComponent<Transform>();

        RaycastHit hit;
        //Debug.DrawRay(transform.position, (Vector3.zero - transform.position).normalized * Vector3.Distance(transform.position, Vector3.zero), Color.blue, 50);
        if (Physics.Raycast(_transform.position, (Vector3.zero- _transform.position).normalized, out hit, Vector3.Distance(_transform.position, Vector3.zero) , layerMask))
        {
            Vector3 pointTouched = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            if(hit.collider.tag == "Ground" && (Vector3.Distance(Vector3.zero, pointTouched) < minMaxPlanet.Min + 3))
            {
                _transform.position = pointTouched;
                propsSpawner.PropPlaced();

                gameObject.isStatic = true;
                Destroy(GetComponent<GravityBody>(), 1);
                Destroy(GetComponent<Rigidbody>(), 1.1f);
                Destroy(this);
            }
            else
            {
                propsSpawner.PropPlaced();
                Destroy(gameObject);
            }
        }
        
    }
    
    /*override
    protected void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground" && (Vector3.Distance(Vector3.zero, _transform.position) < minMaxPlanet.Min + 3))
        {
            propsSpawner.PropPlaced();
            Destroy(this);
        }

        else
        {
            propsSpawner.PropPlaced();
            Destroy(gameObject);
        }
    }*/
}