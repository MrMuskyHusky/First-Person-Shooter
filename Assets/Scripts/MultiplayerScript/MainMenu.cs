using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;

    private void Start()
    {
        if(networkManager == null)
        {
            Debug.LogError("networkManager is not attached to MainMenu");
        }
        if(landingPagePanel == null)
        {
            Debug.LogError("landingPagePanel is not attached to MainMenu");
        }
    }



    public void HostLobby()
    {
        networkManager.StartHost();

        landingPagePanel.SetActive(false);
    }
}
