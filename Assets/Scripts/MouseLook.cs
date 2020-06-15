using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MouseLook : NetworkBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };
    public RotationAxes axes = RotationAxes.MouseXAndY;
    [SerializeField] public float sensitivityX = 15F;
    [SerializeField] public float sensitivityY = 15F;

    [SerializeField] public float minimumX = -360F;
    [SerializeField] public float maximumX = 360F;

    [SerializeField] public float minimumY = -90F;
    [SerializeField] public float maximumY = 90F;

    [SerializeField] float rotationX = 0F;
    [SerializeField] float rotationY = 0F;

    [SerializeField] private Camera cam = null;

    Quaternion originalRotation;
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
        originalRotation = transform.localRotation;
    }

    [ClientCallback]
    private void OnEnable() => PlayerControls.Enable();

    [ClientCallback]
    private void OnDisable() => PlayerControls.Disable();

    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            // Read the mouse input axis
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}   
