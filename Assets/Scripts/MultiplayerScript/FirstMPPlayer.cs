﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FirstMPPlayer : NetworkBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3();

    [Client]
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdMove();
        }
    }

    [Command]
    private void CmdMove()
    {
        RpcMove();
    }

    [ClientRpc]
    private void RpcMove()
    {
        transform.Translate(movement);
        Debug.Log(movement);
    }
}
