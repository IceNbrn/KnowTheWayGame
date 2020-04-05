using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 8.0f;

    [SerializeField] private Transform playerBody;

    private float m_RotationX = 0.0f;
    private DJA m_Controls;

    private void Awake()
    {
        if(m_Controls == null)
            m_Controls = new DJA();
    }
    private void OnEnable() => m_Controls.Player.Enable();
    private void OnDisable() => m_Controls.Player.Disable();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = m_Controls.Player.Look.ReadValue<Vector2>();

        movement.x *= mouseSensitivity * Time.deltaTime;
        movement.y *= mouseSensitivity * Time.deltaTime;
        
        m_RotationX -= movement.y;
        m_RotationX = Mathf.Clamp(m_RotationX, -90.0f, 90.0f);
        transform.localRotation = Quaternion.Euler(m_RotationX, 0f, 0f);
        playerBody.Rotate(Vector3.up * movement.x);
    }

    public void SetPlayerBody(Transform body) => playerBody = body;
}
