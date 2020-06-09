using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float runSpeed, walkSpeed, crouchSpeed, jumpSpeed;
    [SerializeField] public float curHealth;
    [SerializeField] public float _gravity = 20;
    //Struct - Contains Multiple Variables (eg...3 floats)
    private Vector3 _moveDir;
    [SerializeField] private Rigidbody rb;
    //Reference Variable
    [SerializeField] public Text hp;

    [SerializeField] public bool isZoomedIn;
    [SerializeField] public bool damaged;
    [SerializeField] public bool isGrounded;

    [SerializeField] private float verticalDirection;
    [SerializeField] private float horizontalDirection;

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

        PlayerControls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        PlayerControls.Player.Move.canceled += ctx => ResetMovement();
    }

    [ClientCallback]
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [ClientCallback]
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

    [ClientCallback]
    private void OnEnable() => PlayerControls.Enable();

    [ClientCallback]
    private void OnDisable() => PlayerControls.Disable();

    [Client]
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

    [Client]
    public void DamagePlayer(float damage)
    {
        damaged = true;
        curHealth -= damage;
        hp.text = curHealth.ToString();
    }

    [Client]
    void OnCollisionStay()
    {
        isGrounded = true;
    }

    [Client]
    private void SetMovement(Vector3 movement)
    {
        _moveDir = movement;
    }

    [Client]
    private void ResetMovement()
    {
        _moveDir = Vector3.zero;
    }
}
