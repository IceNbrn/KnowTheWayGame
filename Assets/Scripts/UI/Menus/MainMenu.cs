using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace UI.Menus
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public struct LobbyFriend
        {
            private string username;
            private string password;

            public string Username
            {
                get { return username; }
                set { username = value; }
            }

            public string Password
            {
                get { return password; }
                set { password = value; }
            }

            public override string ToString()
            {
                return $"Username: {username}, Password: {password}";
            }
        }

        [SerializeField] private string sceneToLoad = String.Empty;
        [SerializeField] private GameObject findPlayerPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;
        [SerializeField] private Toggle toggle = null;

        private LobbyFriend lobbyFriend;
        private bool IsHost;

        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MaxPlayerPerRoom = 2;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void Start()
        {
            lobbyFriend = new LobbyFriend();
        }

        public void FindPlayer()
        {
            isConnecting = true;
            
            findPlayerPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching . . .";

            PhotonNetwork.OfflineMode = toggle.isOn;

            if (PhotonNetwork.IsConnected)
            {
                // Means the player is already connected
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void Host(TMP_InputField password)
        {
            if (password == null || password.text == String.Empty) return;
            isConnecting = true;

            findPlayerPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching . . .";

            IsHost = true;
            lobbyFriend.Password = password.text;

        }

        public void FriendFriendSetName(TMP_InputField friendName)
        {
            if (friendName == null || friendName.text == String.Empty) return;
            lobbyFriend.Username = friendName.text;
        }
        public void FriendFriendSetPassword(TMP_InputField password)
        {
            if (password == null || password.text == String.Empty) return;
            lobbyFriend.Password = password.text;
            FindFriend();
        }

        public void FindFriend()
        {
            isConnecting = true;

            findPlayerPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching . . .";
            
            if (PhotonNetwork.IsConnected)
            {
                // Means the player is already connected
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.CustomRoomPropertiesForLobby = new[] { lobbyFriend.Username, lobbyFriend.Password};
                roomOptions.CustomRoomProperties = new Hashtable { { lobbyFriend.Username, lobbyFriend.Password } };
                roomOptions.MaxPlayers = MaxPlayerPerRoom;

                PhotonNetwork.JoinOrCreateRoom(lobbyFriend.Username, roomOptions, TypedLobby.Default);
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master!");

            if (isConnecting)
            {
                if (IsHost)
                {
                    string playerName = PhotonNetwork.LocalPlayer.NickName;

                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.CustomRoomPropertiesForLobby = new[] { playerName, lobbyFriend.Password };
                    roomOptions.CustomRoomProperties = new Hashtable { { playerName, lobbyFriend.Password } };
                    roomOptions.MaxPlayers = MaxPlayerPerRoom;

                    PhotonNetwork.CreateRoom(playerName, roomOptions, TypedLobby.Default);
                }
                else
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findPlayerPanel.SetActive(true);

            Debug.Log($"Disconnected due to : {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No clients are waiting for a player, creating a new room...");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayerPerRoom});
        }

        // This method works for the player who is joining the lobby
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            Debug.Log($"Offline Mode = {PhotonNetwork.OfflineMode}");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (playerCount != MaxPlayerPerRoom)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.LoadLevel(sceneToLoad);
                }
                waitingStatusText.text = "Waiting for a player ...";
                Debug.Log("Client is waiting for a player");
            }
            else
            {
                waitingStatusText.text = "Player found";
                Debug.Log("0 | Matching is ready to begin");
            }
        }

        // This is for the player who is already in the lobby
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayerPerRoom)
            {
                //PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                waitingStatusText.text = "Player found";

                Debug.Log("1 | Matching is ready to begin");
                PhotonNetwork.LoadLevel(sceneToLoad);
            }
        }
    }
}

