using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviourPun
    {
        [SerializeField, InspectorName("Default Health")]
        private int m_DefaultHealth;
        
        private CharacterController m_CharacterController;
        private Vector3 m_SpawnPosition;
        private Quaternion m_SpawnRotation;
        private int m_Health;
        private string m_Username;

        void Start()
        {
            m_DefaultHealth = 10;
            m_Health = m_DefaultHealth;
            m_Username = photonView.Owner.NickName;
            m_CharacterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            // TODO: Remove this in the final version!
            if(Input.GetKeyDown(KeyCode.R))
                TakeDamage(20);

            IsAlive();
        }

        private void TakeDamage(int damage)
        {
            if (!photonView.IsMine) return;

            Debug.Log($"Taking Damage... (Value: {damage})");
            m_Health -= damage;
            Debug.Log($"Health: {m_Health}");
            
        }

        private bool IsAlive()
        {
            if (m_Health <= 0)
            {
                Respawn();
                return true;
            }
            return false;
        }
        
        private void Respawn()
        {
            Debug.Log("Respawning...");
            m_CharacterController.enabled = false;
            m_CharacterController.transform.position = m_SpawnPosition;
            m_CharacterController.transform.rotation = m_SpawnRotation;
            m_Health = m_DefaultHealth;
            m_CharacterController.enabled = true;
            Debug.Log("Respawned!");
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!photonView.IsMine) return;

            Debug.Log("Collision");
            if (collider.CompareTag("AcidWater"))
                Respawn();
            else if (collider.CompareTag("Bullet"))
                Respawn();
                
        }

        public string Username => m_Username;
        public int Health => m_Health;

        public void SetSpawnPoint(Vector3 position, Quaternion rotation)
        {
            m_SpawnPosition = position;
            m_SpawnRotation = rotation;
        }

    }
}

