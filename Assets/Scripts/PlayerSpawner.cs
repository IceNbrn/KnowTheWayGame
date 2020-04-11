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

        Transform spawnPoint = SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        player.GetComponent<Player.Player>().SetSpawnPoint(spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"SpawnPoint: {spawnPoint.position}");
    }
}
