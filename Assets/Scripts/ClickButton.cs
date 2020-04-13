using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickButton : MonoBehaviour
{
    private RaycastHit m_Hit;
    private DJA m_Controls;
    private Transform m_CameraPosition;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_Controls == null)
            m_Controls = new DJA();

        m_Controls.Player.InteractionButton.performed += OnPerformedInteraction;
        m_Controls.Player.InteractionButton.started += InteractionButtonOnStarted;
        m_Controls.Player.InteractionButton.canceled += InteractionButtonOnStarted;
    }

    private void InteractionButtonOnStarted(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(m_CameraPosition.position, m_CameraPosition.forward, out m_Hit, 2f)
        )
        {
            if (m_Hit.transform.GetComponent<ActivateHologramBridge>())
            {
                ActivateHologramBridge bridge = m_Hit.transform.GetComponent<ActivateHologramBridge>();
                bridge.ToggleBridgeStatus();
            }
        }
    }

    void Start()
    {
        m_CameraPosition = GetComponent<PlayerCameraController>().CameraTransform;
    }

    private void OnPerformedInteraction(InputAction.CallbackContext obj)
    {    
        if (Physics.Raycast(m_CameraPosition.position, m_CameraPosition.forward, out m_Hit, 2f)
            )
        {
            if (m_Hit.transform.GetComponent<RotateObject>())
            {
                RotateObject objectToRotate = m_Hit.transform.GetComponent<RotateObject>();

                PhotonView view = objectToRotate.ObjectToRotate.GetComponent<PhotonView>();
                view.TransferOwnership(PhotonNetwork.LocalPlayer);

                if (view.IsMine)
                {
                    if (m_Hit.transform.CompareTag("Button"))
                    {
                        objectToRotate.Rotate(new Vector3(0.0f, 0.0f, 45.0f), view);
                    }
                    else if (m_Hit.transform.CompareTag("Door"))
                    { 
                        objectToRotate.Rotate(new Vector3(0.0f, -88f, 0.0f), view);
                    }
                }
            }/*
            else if (m_Hit.transform.GetComponent<ActivateHologramBridge>())
            {
                ActivateHologramBridge bridge = m_Hit.transform.GetComponent<ActivateHologramBridge>();
                bridge.ToggleBridgeStatus();
            }*/
        }
    }

    private void OnEnable()
    {
        m_Controls.Player.InteractionButton.Enable();
    }
    private void OnDisable()
    {
        m_Controls.Player.InteractionButton.Disable();
    }

}
