using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Controllers
{
    public class PlayerCameraController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float mouseSensitivity = 8.0f;

        [SerializeField, InspectorName("PlayerBody")] private Transform m_PlayerBody = null;
        [SerializeField, InspectorName("Camera")] private Camera m_Camera = null;

        private PlayerUIController m_PlayerUI;

        public Transform CameraTransform => m_Camera.transform;

        private float m_RotationX = 0.0f;

        private DJA m_Controls;

        private DJA Controls
        {
            get
            {
                if (m_Controls != null) return m_Controls;
                return m_Controls = new DJA();
            }
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                m_Camera.gameObject.SetActive(true);

                enabled = true;

                Cursor.lockState = CursorLockMode.Locked;

                //m_Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
            }
        }

        public override void OnEnable() => Controls.Player.Look.Enable();

        public override void OnDisable() => Controls.Player.Look.Disable();

        private void Start()
        {
            m_PlayerUI = GetComponent<PlayerUIController>();
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                // If Player UI is active means that the player can't look around.
                if (m_PlayerUI.IsUIActive) return;

                Vector2 movement = Controls.Player.Look.ReadValue<Vector2>();

                movement.x *= mouseSensitivity * Time.deltaTime;
                movement.y *= mouseSensitivity * Time.deltaTime;

                m_RotationX -= movement.y;
                m_RotationX = Mathf.Clamp(m_RotationX, -90.0f, 90.0f);
                m_Camera.transform.localRotation = Quaternion.Euler(m_RotationX, 0f, 0f);
                m_PlayerBody.Rotate(Vector3.up * movement.x);
            }
        }

        private void Look(Vector2 lookAxis)
        {
            /*
            lookAxis.x *= mouseSensitivity * Time.deltaTime;
            lookAxis.y *= mouseSensitivity * Time.deltaTime;

            rotationX -= lookAxis.y;
            rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
            camera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            playerBody.Rotate(Vector3.up * lookAxis.x);*/
        }
    }
}

