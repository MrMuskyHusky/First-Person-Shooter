using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impaceForce = 50f;
    public float bulletSpeed = 100f;

    public int maxAmmo = 10;
    public int currentAmmo = 30;
    public float reloadTime = 1f;
    public bool isReloading = false;
    public bool isZoomingIn = false;
    public bool isFiring = false;

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

    public Animator anim; 

    private float nextTimeToFire = 0f;

    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        if(player == null)
        {
            Debug.LogError("dfds");
        }

        if (currentAmmo == -1)
        {
            currentAmmo = maxAmmo;
        }
    }

    void OnEnable()
    {
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

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
        Zoom();
        Walking();
        Cursor.lockState = CursorLockMode.Locked;
    }
    //Was Running Every frame...yea no dont do that...thats scrubby
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
        isReloading = false;
    }

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
                Debug.Log(hit.transform.name);

                Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    enemy.Damage(damage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-transform.position * impaceForce);
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
        
