using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
/// <summary>
/// Weapon details
/// </summary>
public class Gun : NetworkBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public float range = 100f;
    [SerializeField] public float fireRate = 15f;
    [SerializeField] public float impaceForce = 50f;
    [SerializeField] public float bulletSpeed = 100f;
    [SerializeField] private float nextTimeToFire = 0f;

    [SerializeField] public int maxAmmo = 10;
    [SerializeField] public int currentAmmo = 30;
    [SerializeField] public int clips;
    [SerializeField] public int maxClips;
    [SerializeField] public float reloadTime = 1f;
    [SerializeField] public bool isReloading = false;
    [SerializeField] public bool isZoomingIn = false;
    [SerializeField] public bool isFiring = false;

    [SerializeField] private Gun gun = null;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public AudioSource gunShots;
    public AudioSource reloadSound;
    public GameObject impactEffect;
    public Transform bulletShell;
    public Transform firePoint;
    public GameObject bullet;
    public GameObject myBullet;
    public GameObject bulletHole;
    public Text ammoCount;
    public Text clipsCount;

    public Animator anim;
    private PlayerController player;
    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [Client]
    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
        if(player == null)
        {
            Debug.LogError("dfds");
        }

        if (currentAmmo == -1)
        {
            currentAmmo = maxAmmo;
        }
        isReloading = false;
        anim.SetBool("Reloading", false);
        clipsCount.text = clips.ToString("/ ") + clips;
        clips = maxClips;
    }

    [Client]
    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        if (isReloading)
        {
            return;
        }
        //WAS ALWAYS RUNNING RELOAD....but YEAH NAH MATE
        ///so only run Reload on button press.... NAH YEAH THATS GOOD AYE!
        if (Input.GetButtonDown("Reload"))
        {
            if (currentAmmo < maxAmmo)
            {           
                StartCoroutine(Reload());
            }
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            isFiring = true;
            Shoot();
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            anim.SetBool("Firing", false);
            isFiring = false;
        }
        clipsCount.text = clips.ToString("/ ") + clips;
        Zoom();
        Walking();
        Cursor.lockState = CursorLockMode.Locked;
    }
    //Was Running Every frame...yea no dont do that...thats scrubby
    /// <summary>
    /// Reset ammocount
    /// </summary>
    /// <returns></returns>
    [Client]
    IEnumerator Reload()
    {
        //was running this on button press....but the Reload was constantly waiting....SO WE MOVED IT
        isReloading = true;
        reloadSound.Play();
        anim.SetBool("ZoomIn", false);
        Debug.Log("Reloading...");
        anim.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - .25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        ammoCount.text = currentAmmo.ToString();
        clips--;
        clipsCount.text = clips.ToString("/ ") + clips;
        isReloading = false;
    }
    /// <summary>
    /// Zoom in with there weapon
    /// </summary>
    [Client]
    void Zoom()
    {
        if (Input.GetButton("Fire2"))
        {
            player.isZoomedIn = true;
            isZoomingIn = true;
            Debug.Log("Zoom in weapon");
            anim.SetBool("ZoomIn", true);
            fpsCam.fieldOfView = 30.0f;
        }
        else
        {
            player.isZoomedIn = false;
            isZoomingIn = false;
            anim.SetBool("ZoomIn", false);
            fpsCam.fieldOfView = 60.0f;
        }
    }
    /// <summary>
    /// Play walking animation if walking around
    /// </summary>
    [Client]
    void Walking()
    {
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
    //PEW PEW WAS RUNNING INTO NEGATIVES COS OLD MATE FORGOT A CAP!
    /// <summary>
    /// Be able to shoot with there weapon
    /// </summary>
    [Client]
    public void Shoot()
    {
        //Added the ability to not go below Zero...YEEEE BOI!
        if (currentAmmo > 0)
        {
            gunShots.Play();
            muzzleFlash.Play();
            anim.SetBool("Firing", true);
            currentAmmo--;

            if (ammoCount != null)
            {
                ammoCount.text = currentAmmo.ToString();
            }
                RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log("Hit");
                PlayerController playershooter = hit.collider.GetComponent<PlayerController>();
                if (playershooter != null)
                {
                    Debug.Log("Update Taking Damage");
                    playershooter.TakeDamage(20f);
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                impactGO = Instantiate(bullet, bulletShell.position, transform.rotation);
                impactGO.GetComponent<Rigidbody>().AddForce(bulletShell.right * 250);
                Destroy(impactGO, 2f);
                var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Instantiate(bulletHole, hit.point, hitRotation);
            }
        }
        else
        {
            anim.SetBool("Firing", false);
        }
    }
}
        
