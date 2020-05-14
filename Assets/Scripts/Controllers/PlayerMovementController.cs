using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviourPunCallbacks
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
        private PlayerUIController m_PlayerUI;
        private Animator m_Animator;

        private void Awake()
        {
            if (!photonView.IsMine) return;
            if (m_Controls == null)
                m_Controls = new DJA();

            m_Controls.Player.Jump.performed += JumpOnperformed;
            m_Controls.Player.Sprint.performed += SprintOnperformed;
            m_Controls.Player.Sprint.canceled += SprintOncanceled;
        }

        public override void OnEnable()
        {
            if (!photonView.IsMine) return;
            m_Controls.Player.Enable();
        }

        public override void OnDisable() => m_Controls.Player.Disable();

        // Start is called before the first frame update
        void Start()
        {
            m_Speed = defaultSpeed;
            m_SprintSpeed = m_Speed * 1.5f;
            m_Controller = GetComponent<CharacterController>();
            m_PlayerUI = GetComponent<PlayerUIController>();
            m_Animator = GetComponentInChildren<Animator>();
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

                //if (Input.GetKeyDown(KeyCode.LeftControl) && !m_bIsCrouched)
                //{
                //    // Player is Crouching
                //    m_Speed /= m_SprintSpeed;
                //    m_bIsCrouched = true;
                //    transform.localScale = new Vector3(1f, 0.7f, 1f);
                //    transform.position = new Vector3(1f, transform.position.y - 0.7f, 1f);
                //}
                //else if (Input.GetKeyDown(KeyCode.LeftControl) && m_bIsCrouched)
                //{
                //    // Player is not Crouching
                //    m_Speed *= m_SprintSpeed;
                //    m_bIsCrouched = false;
                //    transform.localScale = new Vector3(1f, 1f, 1f);
                //    transform.position = new Vector3(1f, transform.position.y + 0.7f, 1f);
                //}

                m_Velocity.y += gravity * Time.deltaTime;
                m_Controller.Move(m_Velocity * Time.deltaTime);
            }
        }

        private void Move()
        {
            // If Player UI is active means that the player can't move.
            if (m_PlayerUI.IsUIActive) return;

            Vector2 movementInput = m_Controls.Player.Move.ReadValue<Vector2>();
            if (movementInput.x > 0.0f || movementInput.y > 0.0f)
            {
                Debug.Log($"MovementInput: {movementInput}");
                m_Animator.SetBool("isWalking", true);
            }
            else if (movementInput.x < 0.0f || movementInput.y < 0.0f)
            {
                m_Animator.SetBool("isWalkingBackwards", true);
            }
            else
            {
                m_Animator.SetBool("isWalking", false);
                m_Animator.SetBool("isWalkingBackwards", false);
            }

            Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;

            m_Controller.Move(move * (m_Speed * Time.deltaTime));
            
        }

        private void JumpOnperformed(InputAction.CallbackContext obj)
        {
            if (!photonView.IsMine) return;

            if (m_bIsGrounded)
                m_Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            m_Animator.SetTrigger("Jumping");
        }

        private void SprintOnperformed(InputAction.CallbackContext obj)
        {
            m_Speed = m_SprintSpeed;
            m_Animator.SetBool("isRunning", true);
        }

        private void SprintOncanceled(InputAction.CallbackContext obj)
        {
            m_Speed = defaultSpeed;
            m_Animator.SetBool("isRunning", false);
        }

        public CharacterController CharacterController => m_Controller;
    }
}

