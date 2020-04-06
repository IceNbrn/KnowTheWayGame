using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickButton : MonoBehaviour
{
    private RaycastHit m_Hit;
    private DJA m_Controls;
    private RotateFloor m_ObjectToRotate;
    private Transform m_CameraPosition;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_Controls == null)
            m_Controls = new DJA();

        m_Controls.Player.InteractionButton.performed += OnPerformedInteraction;
    }

    void Start()
    {
        m_CameraPosition = GetComponent<PlayerCameraController>().CameraTransform;
    }

    private void OnPerformedInteraction(InputAction.CallbackContext obj)
    {    
        if (Physics.Raycast(m_CameraPosition.position, m_CameraPosition.forward, out m_Hit, 2f) &&
            m_Hit.transform.CompareTag("Button") && m_Hit.transform.GetComponent<RotateFloor>())
        {
            m_ObjectToRotate = m_Hit.transform.GetComponent<RotateFloor>();
            
            PhotonView view = m_ObjectToRotate.ObjectToRotate.GetComponent<PhotonView>();
            view.TransferOwnership(PhotonNetwork.LocalPlayer);
            m_ObjectToRotate.Rotate();
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
