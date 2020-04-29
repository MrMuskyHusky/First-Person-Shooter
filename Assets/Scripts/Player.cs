using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float runSpeed, walkSpeed, crouchSpeed, jumpSpeed;
    public float curHealth;
    public float _gravity = 20;
    //Struct - Contains Multiple Variables (eg...3 floats)
    private Vector3 _moveDir;
    //Reference Variable
    private CharacterController _charController;
    public Text hp;

    public bool isZoomedIn;
    public bool damaged;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Move(horiz, vert);
        hp.text = "" + curHealth;
    }
    public void Move(float horizontal, float vertical)
    {
        _charController = GetComponent<CharacterController>();
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
            else if(isZoomedIn) //isZoomedIn == true
            {
                moveSpeed = crouchSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }

            //move this direction based off inputs
            _moveDir = transform.TransformDirection(new Vector3(horizontal, 0, vertical) * moveSpeed);
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

    public void DamagePlayer(float damage)
    {
        damaged = true;
        curHealth -= damage;
    }
}
