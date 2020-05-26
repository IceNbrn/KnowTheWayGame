using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject GameObject;

    public static uint PressurePlateActive = 0;
    public int NumberToBeActive = 4;

    private Animator m_Animator;
    private bool m_bIsActivated;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject == null)
        {
            Debug.LogError("[PressurePlate]: GameObject is null!");
            return;
        }
        
        m_Animator = GameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_bIsActivated) return;

        if (!other.CompareTag("PickUp")) return;

        m_bIsActivated = true;

        other.gameObject.tag = "!PickUp";
        other.gameObject.GetComponent<Rigidbody>().useGravity = false;
        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        PressurePlateActive++;

        if (AreAllPressurePlatesActive())
        {
            m_Animator.SetTrigger("OpenDoor");
        }
    }

    private bool AreAllPressurePlatesActive() => PressurePlateActive == NumberToBeActive;

}
