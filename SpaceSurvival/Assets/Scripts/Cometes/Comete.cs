using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GravityBody))]
public class Comete : MonoBehaviour
{
    public delegate void CometeDestroyed();
    public event CometeDestroyed OnCometeDestroyed;

    private Rigidbody _rb;
    private Transform _transform;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private bool _crashed = false; //on a touché le sol
    private bool _onSurface = false; //on est fixé au sol

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();

        Vector3 planetNormal = (_transform.position - GameObject.FindGameObjectWithTag("Ground").transform.position).normalized; //normale entre la planete et la comete
        Vector3 bodyUp = _transform.up;
        _transform.rotation = Quaternion.FromToRotation(bodyUp, planetNormal) * _transform.rotation;
        _transform.Rotate(_transform.up, Random.Range(0, 360), Space.World);

        _rb.AddForce(_transform.forward * Random.Range(_minSpeed, _maxSpeed), ForceMode.Impulse);//* Random.Range(_minSpeed, _maxSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        if (!_crashed)
            _transform.forward = _rb.velocity;
        else if (!_onSurface)
        {
            Crash();
        }
    }

    private void Crash()
    {
        GetComponent<GravityBody>().enabled = false;
        _rb.isKinematic = true;
        _transform.Translate(0, -GetComponent<CapsuleCollider>().radius, 0);
        _onSurface = true;
        GetComponent<DestructTime>().CoutDownStarted = true;
        //Debug.Log("Fixé");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _crashed = true;
            if (_rb.velocity.magnitude > 3)
            {
                //Debug.Log("Touché");
                GetComponent<GravityBody>().standUp = true;
                _crashed = true;
            }
            else
            {
                Crash();
            }
        }
    }

    private void OnDestroy()
    {
        OnCometeDestroyed();
    }

}
