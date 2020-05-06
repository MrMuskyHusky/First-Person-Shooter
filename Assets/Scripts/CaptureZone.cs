using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
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
        WeaponSwitch player = other.GetComponent<WeaponSwitch>();

        if(player != null && gameModeCTF != null)
        {
            if (player.isHoldingFlag())
            {
                gameModeCTF.AddScore(0, 1);
                player.ReturnWeapon(1);
            }
        }
    }
}
