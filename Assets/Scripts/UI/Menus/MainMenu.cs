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

        private string roomName;

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

            //PhotonNetwork.OfflineMode = toggle.isOn;

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

        public void CreateRoom(TMP_InputField roomName)
        {
            PhotonNetwork.CreateRoom(roomName.text, new RoomOptions {MaxPlayers = 2}, TypedLobby.Default);
        }

        public void JoinRoom(TMP_InputField roomName)
        {
            isConnecting = true;

            findPlayerPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching . . .";

            this.roomName = roomName.text;

            if (PhotonNetwork.IsConnected)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.CustomRoomPropertiesForLobby = new[] {roomName.text};
                roomOptions.CustomRoomProperties = new Hashtable() { { "RoomName", roomName.text } };
                roomOptions.MaxPlayers = 2;
                PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, null);
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
            //FindFriend();
        }
        /*
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
        }*/
        public void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master");

            if (isConnecting)
            {
                if(roomName == null)
                    PhotonNetwork.JoinRandomRoom();
                else
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.CustomRoomPropertiesForLobby = new[] { roomName };
                    roomOptions.CustomRoomProperties = new Hashtable() { { "RoomName", roomName } };
                    roomOptions.MaxPlayers = 2;
                    PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
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

