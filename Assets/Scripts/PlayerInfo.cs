using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInfo : MonoBehaviourPun
{
    private string m_Username;
    private uint m_Health;
    
    void Start()
    {
        m_Username = photonView.Owner.NickName;
        m_Health = 10;
    }

    public string Username => m_Username;
    public uint Health => m_Health;
}
