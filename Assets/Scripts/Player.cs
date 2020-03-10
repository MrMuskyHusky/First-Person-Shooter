using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;

    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from user
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        // Get character (cube) to move
        Move(inputH, inputV);
    }

    public void Move(float inputH, float inputV)
    {
        // Get input from user into 'inputDirection' vector
        Vector3 inputDirection = new Vector3(inputH, 0, inputV) * moveSpeed;
        // Convert inputDirection from localspace to worldspace and store in 'direction' variable
        Vector3 direction = transform.TransformDirection(inputDirection);
        // Copy rigid.velocity to 'velocity' vector
        Vector3 velocity = rigid.velocity;
        // Apply new velocity to 'rigid.velocity' 
        rigid.velocity = new Vector3(direction.x, velocity.y, direction.z);
    }
}
