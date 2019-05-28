using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructeur : MonoBehaviour
{
    public LayerMask layerMask;
    public int maxRange;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange, layerMask))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
