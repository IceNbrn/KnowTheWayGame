using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    [SerializeField]
    private Transform holdObjectPosition = null;
    
    private DJA m_Controls;

    private RaycastHit m_Hit;
    private GameObject m_GrabbedObject;
    private Transform m_CameraPosition;

    private void Awake()
    {
        if (m_Controls == null)
            m_Controls = new DJA();

        m_Controls.Player.Interaction.performed += OnPerformedInteraction;
        m_Controls.Player.Interaction.canceled += OnCanceledInteraction;
        m_Controls.Player.Throw.performed += OnPerformedInteraction;
        m_Controls.Player.Throw.canceled += OnCanceledInteraction2;
    }

    private void OnEnable()
    {
        m_Controls.Player.Interaction.Enable();
        m_Controls.Player.Throw.Enable();
    }
    private void OnDisable()
    {
        m_Controls.Player.Interaction.Disable();
        m_Controls.Player.Throw.Disable();
    }

    private void Start()
    {
        m_CameraPosition = GetComponent<PlayerCameraController>().CameraTransform;
    }
    
    private void OnPerformedInteraction(InputAction.CallbackContext obj)
    {
        Debug.Log($"Pressing");
        if (Physics.Raycast(m_CameraPosition.position, m_CameraPosition.forward, out m_Hit, 2f) && m_Hit.transform.gameObject.CompareTag("PickUp") &&
            m_Hit.transform.GetComponent<Rigidbody>())
        {
            PhotonView view = m_Hit.transform.GetComponent<PhotonView>();
            view.TransferOwnership(PhotonNetwork.LocalPlayer);
            Debug.Log($"Pressing 2");
            m_Hit.transform.GetComponent<Rigidbody>().useGravity = false;
            m_Hit.transform.GetComponent<Rigidbody>().isKinematic = true;

            m_GrabbedObject = m_Hit.transform.gameObject;
        }
    }

    private void OnCanceledInteraction(InputAction.CallbackContext obj)
    {
        if(m_GrabbedObject == null) return;

        m_GrabbedObject.transform.GetComponent<Rigidbody>().useGravity = true;
        m_GrabbedObject.transform.GetComponent<Rigidbody>().isKinematic = false;
        
        m_GrabbedObject = null;
    }
    private void OnCanceledInteraction2(InputAction.CallbackContext obj)
    {
        if(m_GrabbedObject == null) return;

        m_GrabbedObject.transform.GetComponent<Rigidbody>().useGravity = true;
        m_GrabbedObject.transform.GetComponent<Rigidbody>().isKinematic = false;
        m_GrabbedObject.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 5.0f, ForceMode.Impulse);

        m_GrabbedObject = null;
    }

    private void Update()
    {
        if (m_GrabbedObject)
        {
            m_GrabbedObject.transform.position = holdObjectPosition.position;
            m_GrabbedObject.transform.rotation = holdObjectPosition.rotation;
        }
    }


}
