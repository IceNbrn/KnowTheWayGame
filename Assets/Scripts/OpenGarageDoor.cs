using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class OpenGarageDoor : MonoBehaviourPun
{
    public Light Light;
    public GameObject ObjectToOpen;

    private Animator m_Animator;
    private bool m_bIsOpen = false;

    private void Start()
    {
        m_Animator = ObjectToOpen.GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (m_bIsOpen) return;
        
        photonView.RPC("RPC_OpenDoor", RpcTarget.AllBuffered, true);
        m_Animator.SetTrigger("OpenDoor");
    }

    [PunRPC]
    private void RPC_OpenDoor(bool isOpen)
    {
        m_bIsOpen = isOpen;
        Color color = Color.green;
        Light.color = color;
        
    }
}
