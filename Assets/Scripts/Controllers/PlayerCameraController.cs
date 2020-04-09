using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace Controllers
{
    public class PlayerCameraController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float mouseSensitivity = 8.0f;

        [SerializeField, InspectorName("PlayerBody")] private Transform m_PlayerBody = null;
        [SerializeField, InspectorName("Camera")] private Camera m_Camera = null;

        public Transform CameraTransform => m_Camera.transform;

        private float m_RotationX = 0.0f;

        private DJA controls;

        private DJA Controls
        {
            get
            {
                if (controls != null) return controls;
                return controls = new DJA();
            }
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                m_Camera.gameObject.SetActive(true);

                enabled = true;

                Cursor.lockState = CursorLockMode.Locked;

                //Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
            }
        }

        public override void OnEnable() => Controls.Enable();

        public override void OnDisable() => Controls.Disable();

        private void Update()
        {
            if (photonView.IsMine)
            {
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

