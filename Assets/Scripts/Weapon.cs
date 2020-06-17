using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
/// <summary>
/// Check teamID, isWeaponLocked? isWeaponDropable.
/// </summary>
public class Weapon : NetworkBehaviour
{
    [SerializeField] public int teamID;
    [SerializeField] public bool isWeaponLocked = false;
    [SerializeField] public bool isWeaponDropable = false;

    [SerializeField] public GameObject worldWeaponGameObject;
    public Vector3 originalLocation;

    [SerializeField] private Weapon weapon = null;


    public override void OnStartAuthority()
    {
        enabled = true;
        weapon.gameObject.SetActive(true);
    }
    /// <summary>
    /// Set weapon to the current teamID player.
    /// </summary>
    /// <param name="teamID"></param>
    /// <param name="worldGameObject"></param>
    /// <param name="originalLocation"></param>
    [Client]
    public void SetWeaponGameObject(int teamID, GameObject worldGameObject, Vector3 originalLocation)
    {
        this.teamID = teamID;
        if (worldGameObject != null)
        {
            worldWeaponGameObject = worldGameObject;
        }
        this.originalLocation = originalLocation;
    }
    /// <summary>
    /// Drop weapon at set dropLocation
    /// </summary>
    /// <param name="player"></param>
    /// <param name="dropLocation"></param>
    [Client]
    public void DropWeapon(Rigidbody player, Vector3 dropLocation)
    {
        float distanceToDrop = Vector3.Distance(Camera.main.transform.position, dropLocation);
        Vector3 directionToDrop = (dropLocation - Camera.main.transform.position).normalized;


        //ray to drop location
        Ray rayToDropLocation = new Ray(Camera.main.transform.position, directionToDrop * distanceToDrop);
        RaycastHit raycastHit;
        //Debug.DrawRay(Camera.main.transform.position, directionToDrop, Color.green, 10f);
        if (Physics.Raycast(rayToDropLocation, out raycastHit, distanceToDrop))
        {
            dropLocation = raycastHit.point;
        }

        //set position in the world
        worldWeaponGameObject.transform.position = dropLocation;


        //ray down
        Renderer rend = worldWeaponGameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            Vector3 topPoint = rend.bounds.center;
            topPoint.y += rend.bounds.extents.y;

            float height = rend.bounds.extents.y * 2;

            //Debug.Log(height);
            //Debug.DrawRay(topPoint, Vector3.down * height * 1.1f, Color.red,10f);

            Ray rayDown = new Ray(topPoint, Vector3.down);
            RaycastHit raycastHitDown = new RaycastHit();
            if (Physics.Raycast(rayDown, out raycastHitDown, height * 1.1f))
            {
                dropLocation = raycastHitDown.point;
                dropLocation.y += rend.bounds.extents.y * 1.1f ;
            }
        }

        worldWeaponGameObject.transform.position = dropLocation;



        Rigidbody flagRigidbody = worldWeaponGameObject.GetComponent<Rigidbody>();
        if (flagRigidbody != null && player != null)
        {
            flagRigidbody.velocity = player.velocity;
        }
    }
}
