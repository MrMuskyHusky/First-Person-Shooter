using Mirror;
using System;
using TMPro;
using UnityEngine;
/// <summary>
/// Communicate with other players
/// </summary>
public class Chat : NetworkBehaviour
{
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);

        OnMessage += HandleNewMessage;
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;
    }
    /// <summary>
    /// Create a new message
    /// </summary>
    /// <param name="message"></param>
    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }
    /// <summary>
    /// Send what the user wrote down in chat
    /// </summary>
    /// <param name="message"></param>
    [Client]
    public void Send(string message)
    {
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        CmdSendMessage(message);

        inputField.text = string.Empty;
    }
    /// <summary>
    /// Recieve message from other clients
    /// </summary>
    /// <param name="message"></param>
    [Command]
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}
