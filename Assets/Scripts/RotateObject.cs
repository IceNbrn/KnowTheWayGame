using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private Transform m_ObjectToRotate;
    private bool m_IsRotated;
    public Transform ObjectToRotate
    {
        get { return m_ObjectToRotate; }
    }

    private void Start()
    {
        m_ObjectToRotate = GetComponent<Transform>();
    }

    public void Rotate(Vector3 value, PhotonView pv)
    {
        pv.RPC("RPC_Rotate", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    private void RPC_Rotate(Vector3 value)
    {
        if (!m_IsRotated)
        {
            m_IsRotated = true;
            m_ObjectToRotate.Rotate(value);
        }
        else
        {
            m_IsRotated = false;
            m_ObjectToRotate.Rotate(-value);
        }

    }

}
