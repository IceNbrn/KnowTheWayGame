using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickup : MonoBehaviour
{
    private bool m_IsColliding = false;

    private Vector3 m_SafePosition;

    // Update is called once per frame
    void Update()
    {
        if (!m_IsColliding)
            m_SafePosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PickUp"))
        {
            m_IsColliding = true;
            transform.position = m_SafePosition;
        }
    }

    public bool IsColliding
    {
        get { return m_IsColliding; }
        set { m_IsColliding = value; }
    }

    public Vector3 SafePosition
    {
        get { return m_SafePosition; }
        set { m_SafePosition = value; }
    }

}

