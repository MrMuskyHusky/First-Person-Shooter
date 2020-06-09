using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MouseLook : NetworkBehaviour
{
    public enum RotationalAxis
    {
        MouseX,
        MouseY,
    }

    [Header("Rotation Variables")]
    [SerializeField] public RotationalAxis axis = RotationalAxis.MouseX;
    [Range(0, 200)]
    [SerializeField] public float sensitivity = 100;
    [SerializeField] public float minY = -60, maxY = 60;
    [SerializeField] private Camera cam = null;
    private float _rotY;

    public bool isTesting;

    private Controls playerControls;
    private Controls PlayerControls
    {
        get
        {
            if (playerControls != null) return playerControls;
            return playerControls = new Controls();
        }
    }
    private Camera transposer;
    public override void OnStartAuthority()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transposer = gameObject.GetComponent<Camera>();
        cam.gameObject.SetActive(true);
        enabled = true;
    }

    [ClientCallback]
    private void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        if (GetComponent<Camera>())
        {
            axis = RotationalAxis.MouseY;
        }
    }

    [ClientCallback]
    private void OnEnable() => PlayerControls.Enable();

    [ClientCallback]
    private void OnDisable() => PlayerControls.Disable();

    void Update()
    {
        Look();
    }

    private void Look()
    {
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (isTesting == true)
        {
            return;
        }
        if (axis == RotationalAxis.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, 0);
        }
        else
        {
            _rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            _rotY = Mathf.Clamp(_rotY, minY, maxY);
            transform.localEulerAngles = new Vector3(-_rotY, 0, 0);
        }
    }
}   
