using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
/// <summary>
/// Be able to look around
/// </summary>
public class MouseLook : NetworkBehaviour
{
    Vector2 rotation = new Vector2(0, 0);
    public float speed = 3;

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (isPaused == false)
        {
            rotation.y += Input.GetAxis("Mouse X");
            rotation.x += -Input.GetAxis("Mouse Y");
            rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
            transform.eulerAngles = new Vector2(0, rotation.y) * speed;
            Camera.main.transform.localRotation = Quaternion.Euler(rotation.x * speed, 0, 0);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}   
