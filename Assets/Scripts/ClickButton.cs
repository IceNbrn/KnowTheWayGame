using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ClickButton : MonoBehaviour
{
    private RaycastHit m_Hit;
    private DJA m_Controls;
    private Transform m_CameraPosition;
    private ActivateHologramBridge m_TempBridge = null;
    private Inventory m_PlayerInventory;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (m_Controls == null)
            m_Controls = new DJA();

        m_Controls.Player.InteractionButton.performed += OnPerformedInteraction;
        m_Controls.Player.InteractionButton.started += InteractionButtonOnStarted;
        m_Controls.Player.InteractionButton.canceled += InteractionButtonOnCanceled;

        m_PlayerInventory = GetComponent<Inventory>();
    }

    private void InteractionButtonOnCanceled(InputAction.CallbackContext obj)
    {
        if (m_TempBridge != null)
        {
            m_TempBridge.SetActive(false);
            m_TempBridge = null;
        }
    }

    private void InteractionButtonOnStarted(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(m_CameraPosition.position, m_CameraPosition.forward, out m_Hit, 2f))
        {
            if (m_Hit.transform.GetComponent<ActivateHologramBridge>())
            {
                ActivateHologramBridge bridge = m_Hit.transform.GetComponent<ActivateHologramBridge>();
                bridge.ToggleBridgeStatus();
                m_TempBridge = bridge;
            }
            else if (m_Hit.transform.GetComponent<OpenGarageDoor>())
            {
                OpenGarageDoor openDoor = m_Hit.transform.GetComponent<OpenGarageDoor>();
                openDoor.OpenDoor();
            }
            else if (m_Hit.transform.GetComponent<Inventory>())
            {
                Inventory objectInventory = m_Hit.transform.GetComponent<Inventory>();
                m_PlayerInventory.Add(objectInventory.items);
                objectInventory.items.Clear();
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

    private void Update()
    {
        if (m_TempBridge != null)
        {
            float distance = Vector3.Distance(transform.position, m_TempBridge.transform.position);
            Debug.Log($"Distance: {distance}");
            if (distance > 3f)
            {
                m_TempBridge.SetActive(false);
                m_TempBridge = null;
            }
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
