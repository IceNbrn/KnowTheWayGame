using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;

        private const string PlayerPrefsNameKey = "PlayerName";
        void Start()
        {
            SetupInputField();
        }

        private void SetupInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string defaultName)
        {
            continueButton.interactable = !string.IsNullOrEmpty(defaultName);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;

            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
        }

    }
}

