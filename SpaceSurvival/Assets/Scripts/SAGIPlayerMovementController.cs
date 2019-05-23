using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SAGIPlayerMovementController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 65;   // Speed when walking forward
        public float StrafeSpeed = 45;    // Speed when walking sideways or backwards
        public float JumpForce = 150;
        public float groundCheckDistance;
        public float shellOffset;

        [HideInInspector] public float CurrentTargetSpeed;

        public void UpdateDesiredTargetSpeed(Vector2 input)
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
                CurrentTargetSpeed = ForwardSpeed;
            }
        }
    }

    [Serializable]
    public class Audio
    {
        public AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        public AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        public AudioClip m_LandSound;           // the sound played when character touches back on ground.
        public float m_StepInterval;

        [HideInInspector] public AudioSource m_AudioSource;
        private float m_NextStep, m_StepCycle;

        public void Init()
        {
            m_NextStep = 0f;
            m_StepCycle = 0f;
        }

        public void PlayLandingSound()
        {
            m_NextStep = m_StepCycle + .5f;
            if (m_AudioSource.isPlaying)
            {
                return;
            }
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
        }

        public void PlayJumpSound()
        {
            m_AudioSource.PlayOneShot(m_JumpSound);
        }

        public void ProgressStepCycle(float speed, bool isGrounded)
        {
            m_StepCycle += Time.deltaTime;

            if (!(m_StepCycle > m_NextStep) || speed == 0 || !isGrounded)
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }
    }

    public Camera cam;
    public LayerMask walkableLayers;
    public MovementSettings movementSettings = new MovementSettings();
    public SAGIMouseLook mouseLook = new SAGIMouseLook();
    [SerializeField, Range(0f, 1f)] private float decelerationPercentage = 0.1f;
    public Audio soundManager = new Audio();


    private Rigidbody m_RigidBody;
    private bool m_Jump, m_Jumping, m_IsGrounded, m_PreviouslyGrounded;
    private CapsuleCollider m_Capsule;



    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        soundManager.m_AudioSource = GetComponent<AudioSource>();
        soundManager.Init();
    }

    private void OnEnable()
    {
        mouseLook.Init(transform, cam.transform);
        //UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }


    private void Update()
    {
        RotateView();

        if (Input.GetButtonDown("Jump") && !m_Jump)
        {
            m_Jump = true;
        }

    }

    private void FixedUpdate()
    {
        GroundCheck();
        Vector2 input = GetInput();

        movementSettings.UpdateDesiredTargetSpeed(input);

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon))
        {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;

            desiredMove = Vector3.ProjectOnPlane(desiredMove, transform.up).normalized;
            desiredMove *= movementSettings.CurrentTargetSpeed;

            if (m_RigidBody.velocity.sqrMagnitude <
                (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
            {
                m_RigidBody.AddForce(desiredMove, ForceMode.Impulse);
            }

            soundManager.ProgressStepCycle(desiredMove.magnitude, m_IsGrounded);
        }
        if (!m_PreviouslyGrounded && m_IsGrounded)
            soundManager.PlayLandingSound();

        if (m_IsGrounded && m_Jump)
        {
            soundManager.PlayJumpSound();
            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
            m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
        }

        ApplyDeceleration(); //Application de la resistance au sol et a l'air
        m_Jump = false;
    }


    private Vector2 GetInput()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        // Rotate the rigidbody velocity to match the new direction that the character is looking
        Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        m_RigidBody.velocity = velRotation * m_RigidBody.velocity;

    }

    private void ApplyDeceleration()
    {
        float yAxisVelocity = m_RigidBody.velocity.y;
        m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x * decelerationPercentage, yAxisVelocity, m_RigidBody.velocity.z * decelerationPercentage);
    }

    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - movementSettings.shellOffset), -transform.up, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + movementSettings.groundCheckDistance, walkableLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = LayerMask.LayerToName(hitInfo.collider.gameObject.layer) != "PlaqueAPulte";
        }
        else
        {
            m_IsGrounded = false;
        }
    }

}
