using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float runSpeed, walkSpeed, crouchSpeed, jumpSpeed;
    public float curHealth;
    public float _gravity = 20;
    //Struct - Contains Multiple Variables (eg...3 floats)
    private Vector3 _moveDir;
    private Rigidbody rb;
    //Reference Variable
    public Text hp;

    public bool isZoomedIn;
    public bool damaged;
    public bool isGrounded;

    private float verticalDirection;
    private float horizontalDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        verticalDirection = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        horizontalDirection = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(horizontalDirection, 0, verticalDirection);


        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Move(horizontalDirection, verticalDirection);
    }
    public void Move(float horizontal, float vertical)
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
        else if (isZoomedIn) //isZoomedIn == true
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        //move this direction based off inputs
        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void DamagePlayer(float damage)
    {
        damaged = true;
        curHealth -= damage;
        hp.text = curHealth.ToString();
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }
}
