using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab = null;
    
    public Transform[] SpawnPoints;
    
    private void Start()
    {
        Debug.Log($"PlayerActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");

        Vector3 spawnPointPosition = SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
        Quaternion spawnPointRotation = SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].rotation;

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPointPosition, spawnPointRotation);
        player.GetComponent<Player.Player>().SetSpawnPoint(spawnPointPosition, spawnPointRotation);
        /*
        playerCamera.GetComponent<MouseLook>().SetPlayerBody(player.transform);
        playerCamera.transform.parent = player.transform;
        playerCamera.transform.position = player.transform.position + new Vector3(0f, 0.64f, 0.3f);
        playerCamera.transform.rotation = player.transform.rotation;
        player.GetComponent<PlayerMovement>().CameraTransform = playerCamera.transform;*/
    }
}
