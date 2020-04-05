using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LagCompensation : MonoBehaviour, IPunObservable
{
    private Rigidbody m_Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_Rigidbody.position);
            stream.SendNext(m_Rigidbody.rotation);
            stream.SendNext(m_Rigidbody.velocity);
        }
        else
        {
            m_Rigidbody.position = (Vector3)stream.ReceiveNext();
            m_Rigidbody.rotation = (Quaternion)stream.ReceiveNext();
            m_Rigidbody.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            m_Rigidbody.position += m_Rigidbody.velocity * lag;
        }
    }
}
