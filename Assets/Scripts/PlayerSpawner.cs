using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Camera playerCamera = null;
    /*[SerializeField] private static Dictionary<bool, Transform> playerSpawnsDictionary;
    [SerializeField] private Transform[] playerSpawns;*/
    public Transform[] SpawnPoints;
    

    private void Start()
    {
        /*
        for (int i = 0; i < playerSpawns.Length; i++)
        {
            playerSpawnsDictionary.Add(false, playerSpawns[i]);
        }*/

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, SpawnPoints[0].position, Quaternion.identity);
        playerCamera.GetComponent<MouseLook>().SetPlayerBody(player.transform);
        playerCamera.transform.parent = player.transform;
        playerCamera.transform.position = player.transform.position + new Vector3(0f, 0.64f, 0.3f);
        playerCamera.transform.rotation = player.transform.rotation;
        player.GetComponent<PlayerMovement>().CameraTransform = playerCamera.transform;
    }
}
