using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public GameObject Ui;
    public string TagToTrigger = "Player";

    private float m_RadiusCheck = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(TagToTrigger)) return;
        if (other.gameObject.GetComponent<Inventory>())
        {
            Debug.Log($"PlayerName {other.gameObject.GetComponent<PhotonView>().Owner.NickName}");
            Inventory inventory = other.gameObject.GetComponent<Inventory>();
           
            Debug.Log($"Contains Keycard");
            bool result = IsPlayerNearby(other.gameObject.transform.position);

            if (result)
            {
                if (inventory.items.Contains("Keycard"))
                {
                    Ui.SetActive(true);
                    StartCoroutine(LoadNextLevelCorountine());
                }
            }
            
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

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
