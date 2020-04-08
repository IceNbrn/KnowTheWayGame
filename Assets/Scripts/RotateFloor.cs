using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFloor : MonoBehaviour
{
    public Transform ObjectToRotate;
    private Vector3 m_RotationValue;


    public void Start()
    {
        // Rotates 45 degrees around the z-axis
        m_RotationValue = new Vector3(0.0f, 0.0f, 45.0f);
    }

    public void Rotate()
    {
        ObjectToRotate.Rotate(m_RotationValue);
    }
}
