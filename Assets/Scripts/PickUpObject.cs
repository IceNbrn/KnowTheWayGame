using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private Transform holdObject = null;

    private DJA m_Controls;

    private RaycastHit m_Hit;
    private GameObject m_GrabbedObject;
    private Transform m_CameraTransform;
    private BoxPickup m_BoxPickup;
    private MeshCollider m_BoxCollider;

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
        m_CameraTransform = GetComponent<PlayerCameraController>().CameraTransform;
    }

    private void OnPerformedInteraction(InputAction.CallbackContext obj)
    {
        if (Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out m_Hit, 2f) &&
            m_Hit.transform.gameObject.CompareTag("PickUp") &&
            m_Hit.transform.GetComponent<Rigidbody>())
        {
            PhotonView view = m_Hit.transform.GetComponent<PhotonView>();
            view.TransferOwnership(PhotonNetwork.LocalPlayer);

            m_GrabbedObject = m_Hit.transform.gameObject;
            m_BoxPickup = m_GrabbedObject.GetComponent<BoxPickup>();
            m_BoxCollider = m_GrabbedObject.GetComponent<MeshCollider>();
            m_BoxCollider.isTrigger = true;

            m_Hit.transform.GetComponent<Rigidbody>().useGravity = false;
            m_Hit.transform.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Clicked!");
        }
    }

    private void OnCanceledInteraction(InputAction.CallbackContext obj)
    {
        if (m_GrabbedObject == null) return;

        m_GrabbedObject.transform.GetComponent<Rigidbody>().useGravity = true;
        m_GrabbedObject.transform.GetComponent<Rigidbody>().isKinematic = false;

        ResetBoxStatus();
    }

    private void OnCanceledInteraction2(InputAction.CallbackContext obj)
    {
        if (m_GrabbedObject == null) return;

        m_GrabbedObject.transform.GetComponent<Rigidbody>().useGravity = true;
        m_GrabbedObject.transform.GetComponent<Rigidbody>().isKinematic = false;
        m_GrabbedObject.transform.GetComponent<Rigidbody>()
            .AddForce(m_CameraTransform.forward * 5.0f, ForceMode.Impulse);

        ResetBoxStatus();
    }

    private void Update()
    {
        if (m_GrabbedObject != null)
        {
            Debug.Log($"Holding object");
            m_GrabbedObject.transform.position = holdObject.position;
            m_GrabbedObject.transform.rotation = holdObject.rotation;
        }

        if (m_BoxCollider != null && m_BoxPickup != null)
        {
            if(m_GrabbedObject != null && m_BoxPickup.IsColliding)
            {
                Debug.Log($"Setting safe position {m_BoxPickup.SafePosition}");
                m_GrabbedObject.transform.GetComponent<Rigidbody>().useGravity = true;
                m_GrabbedObject.transform.GetComponent<Rigidbody>().isKinematic = false;
                
                ResetBoxStatus();
            }
        }
    }

    private void ResetBoxStatus()
    {
        m_BoxCollider.isTrigger = false;
        m_BoxPickup.IsColliding = false;
        m_GrabbedObject = null;
        m_BoxPickup = null;
        m_BoxCollider = null;
    }
}