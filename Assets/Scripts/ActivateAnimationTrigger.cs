using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateAnimationTrigger : MonoBehaviour
{
    public GameObject GameObject;
    public GameObject Ui;
    public string TagToTrigger = "Player";

    private Animator m_Animator;
    private float m_RadiusCheck = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject == null)
        {
            Debug.LogError("[ActivateAnimation]: GameObject is null!");
            return;
        }
        m_Animator = GameObject.GetComponent<Animator>();

        if(m_Animator == null) 
            Debug.LogError("Animator is null!");
    }

    void Update()
    {
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(TagToTrigger)) return;
        bool result = IsPlayerNearby(other.gameObject.transform.position);

        if (result)
        {
            Ui.SetActive(true);
            m_Animator.SetTrigger("CloseDoor");
            StartCoroutine(LoadNextLevelCorountine());
            
        } 
            
    }
    
    private bool IsPlayerNearby(Vector3 playerPosition)
    {
        //if (!PhotonNetwork.IsMasterClient) return false;
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, m_RadiusCheck);
        int countPlayers = 0;
        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag(TagToTrigger))
                countPlayers++;
            if (countPlayers == 2)
                return true;
        }

        return false;
    }

    IEnumerator LoadNextLevelCorountine()
    {
        yield return new WaitForSeconds(5);

        Ui.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
}
