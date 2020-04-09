using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        public float defaultSpeed = 5f;
        public float gravity = -9.81f;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        private float m_Speed;
        private float m_SprintSpeed;
        private CharacterController m_Controller;
        private DJA m_Controls;
        private Vector3 m_Velocity;
        private bool m_bIsGrounded;
        private bool m_bIsCrouched;

        private void Awake()
        {
            if (m_Controls == null)
                m_Controls = new DJA();

            m_Controls.Player.Jump.performed += JumpOnperformed;
            m_Controls.Player.Sprint.performed += SprintOnperformed;
            m_Controls.Player.Sprint.canceled += SprintOncanceled;
        }

        public override void OnEnable()
        {
            m_Controls.Player.Enable();
        }

        public override void OnDisable() => m_Controls.Player.Disable();

        // Start is called before the first frame update
        void Start()
        {
            m_Speed = defaultSpeed;
            m_SprintSpeed = m_Speed * 1.5f;
            m_Controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                m_bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                if (m_bIsGrounded && m_Velocity.y < 0)
                    m_Velocity.y = -2f;

                Move();

                if (Input.GetKeyDown(KeyCode.LeftControl) && !m_bIsCrouched)
                {
                    // Player is Crouching
                    m_Speed /= m_SprintSpeed;
                    m_bIsCrouched = true;
                    transform.localScale = new Vector3(1f, 0.7f, 1f);
                    transform.position = new Vector3(1f, transform.position.y - 0.7f, 1f);
                }
                else if (Input.GetKeyDown(KeyCode.LeftControl) && m_bIsCrouched)
                {
                    // Player is not Crouching
                    m_Speed *= m_SprintSpeed;
                    m_bIsCrouched = false;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    transform.position = new Vector3(1f, transform.position.y + 0.7f, 1f);
                }

                m_Velocity.y += gravity * Time.deltaTime;
                m_Controller.Move(m_Velocity * Time.deltaTime);
            }
        }

        private void Move()
        {
            Vector2 movementInput = m_Controls.Player.Move.ReadValue<Vector2>();
            Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;

            m_Controller.Move(move * (m_Speed * Time.deltaTime));
        }

        private void JumpOnperformed(InputAction.CallbackContext obj)
        {
            if (m_bIsGrounded)
                m_Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        private void SprintOnperformed(InputAction.CallbackContext obj)
        {
            if (!m_bIsCrouched)
                m_Speed = m_SprintSpeed;
        }

        private void SprintOncanceled(InputAction.CallbackContext obj)
        {
            if (!m_bIsCrouched)
                m_Speed = defaultSpeed;
        }
    }
}

