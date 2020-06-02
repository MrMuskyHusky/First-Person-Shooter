using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [Header ("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;


    public static string DisplayName { get; private set; }

    private string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetUpInputField();

        if(nameInputField == null)
        {
            Debug.LogError("nameInputField is not attached to PlayerNameInput");
        }
        if (continueButton == null)
        {
            Debug.LogError("continueButton is not attached to PlayerNameInput");
        }
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            continueButton.interactable = false;
            return;
        }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
