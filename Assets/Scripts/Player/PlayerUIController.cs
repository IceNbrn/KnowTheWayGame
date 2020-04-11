using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

namespace Player
{
    public class PlayerUIController : MonoBehaviourPunCallbacks
    {
        public string MainMenuScene = String.Empty;
        public GameObject UI = null;

        private DJA m_Controls;

        private DJA Controls
        {
            get
            {
                if (m_Controls != null) return m_Controls;
                return m_Controls = new DJA();
            }
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                Controls.UI.Menu.performed += MenuOnPerformed;
            }
        }

        public override void OnEnable() => Controls.UI.Enable();

        public override void OnDisable() => Controls.UI.Disable();

        public void Resume()
        {
            ChangeUIStatus(false);
        }

        public void LeaveMatch()
        {
            Debug.Log("Leaving Match...");
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        private void MenuOnPerformed(InputAction.CallbackContext obj)
        {
            if (photonView.IsMine)
            {
                // Toggle the UI Status
                ChangeUIStatus(!UI.activeSelf);
            }
        }

        private void ChangeUIStatus(bool status)
        {
            UI.SetActive(status);
            if (status)
            {
#if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.None;
#else
                Cursor.lockState = CursorLockMode.Confined;
#endif
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public bool IsUIActive => UI.activeSelf;

    }
}


