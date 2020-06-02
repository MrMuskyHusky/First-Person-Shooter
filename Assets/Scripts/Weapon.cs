using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int teamID;
    public bool isWeaponLocked = false;
    public bool isWeaponDropable = false;

    public GameObject worldWeaponGameObject;
    public Vector3 originalLocation;

    public void SetWeaponGameObject(int teamID, GameObject worldGameObject, Vector3 originalLocation)
    {
        this.teamID = teamID;
        if (worldGameObject != null)
        {
            worldWeaponGameObject = worldGameObject;
        }
        this.originalLocation = originalLocation;
    }

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
