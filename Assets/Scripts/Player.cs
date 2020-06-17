using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour
{
    //Game mode
    [SerializeField] int playersTeamID;
    [SerializeField] public int teamID{ get { return playersTeamID; } }

    Rigidbody playerRigidbody;

    //weapons
    [SerializeField] public List<Weapon> weapons;
    [SerializeField] int currentWeapon = 0;
    [SerializeField] int lastWeapon = 0;
    [SerializeField] public float forwardDropOffset;
    [SerializeField] public float upDropOffset;

    private Controls playerControls;
    private Controls PlayerControls
    {
        get
        {
            if (playerControls != null) return playerControls;
            return playerControls = new Controls();
        }
    }
    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void OnEnable() => PlayerControls.Enable();

    [ClientCallback]
    private void OnDisable() => PlayerControls.Disable();

    [Client]
    private void Start()
    {
        SwitchWeapon(currentWeapon);

        playerRigidbody = GetComponent<Rigidbody>();
        if(playerRigidbody == null)
        {
            Debug.LogError("Player Rigidbody not found");
        }
    }

    [Client]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DropWeapon(currentWeapon);
        }
    }
    /// <summary>
    /// Pick up a weapon/flag
    /// </summary>
    /// <param name="weaponObject"></param>
    /// <param name="originalLocation"></param>
    /// <param name="teamID"></param>
    /// <param name="weaponID"></param>
    /// <param name="overrideLock"></param>
    [Client]
    public void PickUpWeapon(GameObject weaponObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
    {
        SwitchWeapon(weaponID, overrideLock);

        weapons[weaponID].SetWeaponGameObject(teamID, weaponObject, originalLocation);
 
    }
    /// <summary>
    /// Switch weapons
    /// </summary>
    /// <param name="weaponID"></param>
    /// <param name="overrideLock"></param>
    [Client]
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
    /// <summary>
    /// Player able to drop his weapon/flag.
    /// </summary>
    /// <param name="weaponID"></param>
    [Client]
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
    /// <summary>
    /// Switch back to the last weapon equipped
    /// </summary>
    /// <param name="weaponID"></param>
    [Client]
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
    [Client]
    public bool IsHoldingFlag()
    {
        if(currentWeapon == 1)
        { 
            return true;
        }

        return false;
    }

    [Client]
    public int GetWeaponTeamID()
    {
        return weapons[currentWeapon].teamID;
    }
}
