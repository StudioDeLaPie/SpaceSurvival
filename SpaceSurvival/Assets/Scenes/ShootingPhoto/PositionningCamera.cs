using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PositionningCamera : MonoBehaviour
{
    [Range(1, 20)]
    public float distanceToCenter = 10;
    //1.5 pour les minerais

    public GameObject center;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(center.transform);
        Vector3 direction = transform.position - center.transform.position;
        if(Vector3.Distance(center.transform.position, transform.position) > distanceToCenter)
            transform.position -= direction.normalized;
        else if(Vector3.Distance(center.transform.position, transform.position) < distanceToCenter)
            transform.position += direction.normalized;
    }
}
