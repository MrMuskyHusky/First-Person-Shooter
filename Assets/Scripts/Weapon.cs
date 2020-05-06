using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isWeaponLocked = false;
    public bool isWeaponDropable = false;

    public GameObject worldWeaponGameObject;

    public Vector3 originalLocation;

    private void Start()
    {
        if(worldWeaponGameObject != null)
        {
            originalLocation = worldWeaponGameObject.transform.position;
        }
    }
}
