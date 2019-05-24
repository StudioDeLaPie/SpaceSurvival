using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionHelper : MonoBehaviour
{
    public bool canBeConstruct = true;

    private Vector3 _gravityUp;
    private Vector3 _planetePosition;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _planetePosition = GameObject.FindGameObjectWithTag("Planete").GetComponent<Transform>().position;
    }

    private void Update()
    {
        _gravityUp = (transform.position - _planetePosition).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(_transform.up, _gravityUp) * _transform.rotation;
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        canBeConstruct = false;
    }

    private void OnTriggerExit(Collider other)
    {
        canBeConstruct = true;
    }
}
