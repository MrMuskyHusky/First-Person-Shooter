using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float runSpeed, walkSpeed, crouchSpeed, jumpSpeed;
    private float _gravity = 20;
    //Struct - Contains Multiple Variables (eg...3 floats)
    private Vector3 _moveDir;
    //Reference Variable
    private CharacterController _charController;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        // If we are grounded
        if (_charController.isGrounded)
        {
            bool isCrouchPressed = Input.GetButton("Crouch");
            bool isSprintPressed = Input.GetButton("Sprint");

            //set speed
            if (isCrouchPressed && isSprintPressed)
            {
                moveSpeed = walkSpeed;
            }
            else if (isSprintPressed)
            {
                moveSpeed = runSpeed;
            }
            else if (isCrouchPressed)
            {
                moveSpeed = crouchSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }

            //move this direction based off inputs
            _moveDir = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed);
            if (Input.GetButton("Jump"))
            {
                _moveDir.y = jumpSpeed;
            }
        }
        //Regardless if we are grounded or not
        //apply grvity
        _moveDir.y -= _gravity * Time.deltaTime;
        //apply mo
        _charController.Move(_moveDir * Time.deltaTime);
    }
}
