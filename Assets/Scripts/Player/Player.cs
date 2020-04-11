using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviourPun
    {
        [SerializeField, InspectorName("Default Health")]
        private int m_DefaultHealth;

        private Vector3 m_SpawnPosition;
        private Quaternion m_SpawnRotation;
        private int m_Health;
        private string m_Username;

        public bool CanMove { get; set; }

        void Start()
        {
            m_DefaultHealth = 10;
            m_Health = m_DefaultHealth;
            m_Username = photonView.Owner.NickName;
            CanMove = true;
        }

        void Update()
        {
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
                Die();
                return true;
            }
            return false;
        }

        private void Die()
        {
            Debug.Log("Dying...");
            Respawn();
            m_Health = m_DefaultHealth;
        }

        private void Respawn()
        {
            Debug.Log("Respawning...");
            gameObject.transform.position = m_SpawnPosition;
            gameObject.transform.rotation = m_SpawnRotation;
            CanMove = true;
            Debug.Log($"SpawnPosition: {m_SpawnPosition}\n" +
                      $"SpawnRotation: {m_SpawnRotation}\n" +
                      $"TransformPosition: {gameObject.transform.position}\n" +
                      $"TransformRotation: {gameObject.transform.rotation}");
            Debug.Log("Respawned!");
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!photonView.IsMine) return;

            Debug.Log("Collision");
            if (collider.CompareTag("AcidWater"))
            {
                CanMove = false;
                StartCoroutine(WaitToTakeDamage());
            }
                
        }

        public string Username => m_Username;
        public int Health => m_Health;

        public void SetSpawnPoint(Vector3 position, Quaternion rotation)
        {
            m_SpawnPosition = position;
            m_SpawnRotation = rotation;
            Debug.Log($"SpawnPoint Saved \n" +
                      $"Position : {m_SpawnPosition}\n" +
                      $"Rotation : {m_SpawnRotation}");
        }

        IEnumerator WaitToTakeDamage()
        {
            yield return new WaitForSeconds(2.0f);
            Die();
        }
    }
}

