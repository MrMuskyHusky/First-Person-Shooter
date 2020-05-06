using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Captured the flag");

        WeaponSwitch player = other.GetComponent<WeaponSwitch>();

        if(player != null)
        {
            player.SwitchWeapon(2);

            gameObject.SetActive(false);
        }
    }
}
