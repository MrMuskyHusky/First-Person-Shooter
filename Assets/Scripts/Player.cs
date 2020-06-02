using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //Game mode
    [SerializeField] int playersTeamID;
    public int teamID{ get { return playersTeamID; } }

    Rigidbody playerRigidbody;

    //weapons
    public List<Weapon> weapons;
    int currentWeapon = 0;
    int lastWeapon = 0;
    public float forwardDropOffset;
    public float upDropOffset;

    private void Start()
    {
        SwitchWeapon(currentWeapon);

        playerRigidbody = GetComponent<Rigidbody>();
        if(playerRigidbody == null)
        {
            Debug.LogError("Player Rigidbody not found");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DropWeapon(currentWeapon);
        }
    }

    public void PickUpWeapon(GameObject weaponObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
    {
        SwitchWeapon(weaponID, overrideLock);

        weapons[weaponID].SetWeaponGameObject(teamID, weaponObject, originalLocation);
 
    }

    public void SwitchWeapon(int weaponID, bool overrideLock = false)
    {
        if(!overrideLock && weapons[currentWeapon].isWeaponLocked == true)
        {
            return;
        }

        lastWeapon = currentWeapon;
        currentWeapon = weaponID;

        foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        weapons[currentWeapon].gameObject.SetActive(true);
    }


    public void DropWeapon(int weaponID)
    {
        if (weapons[weaponID].isWeaponDropable)
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward *= forwardDropOffset;
            forward.y = upDropOffset;
            Vector3 dropLocation = transform.position + forward;

            weapons[weaponID].DropWeapon(playerRigidbody, dropLocation);
            weapons[weaponID].worldWeaponGameObject.SetActive(true);


            SwitchWeapon(lastWeapon,true);//if possible
        }
    }

    public void ReturnWeapon(int weaponID)
    {
        if (weapons[weaponID].isWeaponDropable)//flag
        {
            Vector3 returnLocation = weapons[weaponID].originalLocation;

            weapons[weaponID].worldWeaponGameObject.transform.position = returnLocation;
            weapons[weaponID].worldWeaponGameObject.SetActive(true);

            SwitchWeapon(lastWeapon,true);//if possible
        }
    }

    //bad
    public bool IsHoldingFlag()
    {
        if(currentWeapon == 1)
        { 
            return true;
        }

        return false;
    }
    

    public int GetWeaponTeamID()
    {
        return weapons[currentWeapon].teamID;
    }
}
