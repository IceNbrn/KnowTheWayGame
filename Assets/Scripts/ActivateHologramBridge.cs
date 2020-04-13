using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ActivateHologramBridge : MonoBehaviourPun
{
    public GameObject BridgeToToggle;
    public Light[] Lights;
    private bool m_Status;
    public void ToggleBridgeStatus()
    {
        photonView.RPC("RPC_ToggleBridgeStatus", RpcTarget.AllBuffered, !BridgeToToggle.activeSelf);
    }

    [PunRPC]
    private void RPC_ToggleBridgeStatus(bool status)
    {
        BridgeToToggle.SetActive(status);
        for (int i = 0; i < Lights.Length; i++)
        {
            Color color;
            if (BridgeToToggle.activeSelf)
                color = Color.green;
            else
                color = Color.red;

            Lights[i].color = color;
        }
    }
}
