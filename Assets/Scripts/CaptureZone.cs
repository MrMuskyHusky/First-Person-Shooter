﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] int teamID;

    GameModeCTF gameModeCTF;
    private void Start()
    {
        gameModeCTF = FindObjectOfType<GameModeCTF>();

        if(gameModeCTF == null)
        {
            Debug.LogError("Could not find GameModeCTF");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player != null && gameModeCTF != null)
        {
            if (player.GetWeaponTeamID() != teamID)
            {
                return;
            }

            if (player.IsHoldingFlag())
            {
                gameModeCTF.AddScore(player.teamID, 1);
                player.ReturnWeapon(1);
            }
        }
    }
}
