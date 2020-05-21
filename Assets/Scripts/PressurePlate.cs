using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject GameObject;

    //private Animator m_Animator;
    public static uint PressurePlateActive = 0;
    public int NumberToBeActive = 4;
    public 

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject == null)
        {
            Debug.LogError("[PressurePlate]: GameObject is null!");
            return;
        }/*
        m_Animator = GameObject.GetComponent<Animator>();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PickUp")) return;
        other.gameObject.tag = "!PickUp";
        PressurePlateActive++;
        Debug.Log($"PressurePlates: {PressurePlateActive}");

        if (AreAllPressurePlatesActive())
        {
            GameObject.SetActive(true);
            Debug.Log("All pressure plates are activated!!!!!!");
            //m_Animator.
        }
    }

    private bool AreAllPressurePlatesActive() => PressurePlateActive == NumberToBeActive;
}
