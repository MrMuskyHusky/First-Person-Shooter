using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void Start()
    {
        if(networkManager == null)
        {
            Debug.LogError("networkManager is not attached to JoinLobbyMenu");
        }
        if (landingPagePanel == null)
        {
            Debug.LogError("landingPagePanel is not attached to JoinLobbyMenu");
        }
        if (ipAddressInputField == null)
        {
            Debug.LogError("ipAddressInputField is not attached to JoinLobbyMenu");
        }
        if (joinButton == null)
        {
            Debug.LogError("joinButton is not attached to JoinLobbyMenu");
        }
    }

    private void OnEnable()
    {
        networkManager.OnClientConnected += HandleClientConnected;
        networkManager.OnClientDisconnected += HandleClientDisconnected;
    }
    private void OnDisable()
    {
        networkManager.OnClientConnected -= HandleClientConnected;
        networkManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}