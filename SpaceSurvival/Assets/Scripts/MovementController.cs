using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 30;   // Speed when walking forward
        public float StrafeSpeed = 20;    // Speed when walking sideways or backwards
        public float SprintSpeed = 45;
        public float JumpForce = 150;
        public float groundCheckDistance;
        public float shellOffset;

        [HideInInspector] public float CurrentTargetSpeed;

        public void UpdateDesiredTargetSpeed(Vector2 input, bool sprinting)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0) //strafe
            {
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0) //backwards
            {
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y > 0) //forwards
            {
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                if (sprinting)
                    CurrentTargetSpeed = SprintSpeed;
                else
                    CurrentTargetSpeed = ForwardSpeed;
            }
        }
    }

    public Camera cam;
    public LayerMask walkableLayers;
    public MovementSettings movementSettings = new MovementSettings();
    public MouseLook mouseLook = new MouseLook();
    [Range(0f, 1f)] public float decelerationRatio = 0.1f;

    private Transform _transform;
    private Transform _camTransform;
    private Rigidbody _rb;
    private CapsuleCollider _capsule;
    private bool _jump, _jumping, _isGrounded, _previouslyGrounded, _sprinting;
    private Vector2 _input = new Vector2();
    private Vector3 _moveDir;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsule = GetComponent<CapsuleCollider>();
        _transform = transform;
        _camTransform = cam.transform;
        mouseLook.Init(_transform, _camTransform);
    }

    // Update is called once per frame
    void Update()
    {
        RotateView();
        if (Input.GetButtonDown("Jump") && !_jump)
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        GroundCheck();
        movementSettings.UpdateDesiredTargetSpeed(_input, _sprinting);

        if ((Mathf.Abs(_input.x) > float.Epsilon || Mathf.Abs(_input.y) > float.Epsilon))
        {
            _moveDir = new Vector3(_input.x, 0, _input.y) * movementSettings.CurrentTargetSpeed;
            _rb.MovePosition(_rb.position + _transform.TransformDirection(_moveDir) * Time.deltaTime);
        }

        if (_isGrounded && _jump)
        {
            Vector3 localVel = _transform.InverseTransformDirection(_rb.velocity);
            localVel.y = 0;
            _rb.velocity = _transform.TransformDirection(localVel);
            _rb.AddForce(_transform.TransformDirection(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
        }

        ApplyDeceleration(); //Application de la resistance au sol et a l'air
        _jump = false;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = _transform.eulerAngles.y;

        mouseLook.LookRotation(_transform, _camTransform);

        // Rotate the rigidbody velocity to match the new direction that the character is looking
        Quaternion velRotation = Quaternion.AngleAxis(_transform.eulerAngles.y - oldYRotation, _transform.up);
        _rb.velocity = velRotation * _rb.velocity;
    }

    private void GetInput()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
        _input.Normalize();
        _sprinting = Input.GetButton("Sprint");
    }

    private void GroundCheck()
    {
        _previouslyGrounded = _isGrounded;
        RaycastHit hitInfo;

        if (Physics.SphereCast(_transform.position, _capsule.radius * (1.0f - movementSettings.shellOffset), -_transform.up, out hitInfo,
                               ((_capsule.height / 2f) - _capsule.radius) + movementSettings.groundCheckDistance, walkableLayers, QueryTriggerInteraction.Ignore))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void ApplyDeceleration()
    {
        Vector3 localVel = _transform.InverseTransformDirection(_rb.velocity);
        localVel.Set(localVel.x * decelerationRatio, localVel.y, localVel.z * decelerationRatio);
        _rb.velocity = _transform.TransformDirection(localVel);
    }
}
