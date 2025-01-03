using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public Transform camera;
    public GameObject muzzleFlash;

    [Header("Ammo")] 
    public float reloadTime;
    public int magSize = 30; // Magazine size
    public int totalBullets = 120; // Total bullets available
    public int bulletsInMag;
    public bool isReloading;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    [Header("SFX")] public int shootSFXIndex = 0;
    
    private float nextFire;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        bulletsInMag = magSize; // Start with a full magazine
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        MyInput();
        UpdateAmmoUI();
    }

    private void MyInput()
    {
        if (Input.GetKey(KeyCode.Mouse0) && nextFire <= 0 && !isReloading)
        {
            nextFire = 1 / fireRate;
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsInMag < magSize && !isReloading)
        {
            Reload();
        }
    }

    void Reload()
    {
        if (isReloading || totalBullets <= 0) return;

        isReloading = true;
        animator.SetTrigger("Reload");
        SoundManager.Instance.PlayReload(shootSFXIndex);
        
        Invoke(nameof(PerformReload), reloadTime);
    }

    void PerformReload()
    {
        // Calculate bullets to reload
        int bulletsNeeded = magSize - bulletsInMag;

        // If total bullets are less than needed, refill only what's available
        int bulletsToReload = Mathf.Min(bulletsNeeded, totalBullets);

        bulletsInMag += bulletsToReload;
        totalBullets -= bulletsToReload;

        isReloading = false;
    }

    void Fire()
    {
        if (bulletsInMag <= 0)
        {
            Reload();
        }
        else
        {
            bulletsInMag--;
            muzzleFlash.GetComponent<ParticleSystem>().Play();
            animator.SetTrigger("Shoot");
            SoundManager.Instance.PlayShoot(shootSFXIndex);

            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
            {
                if (hit.transform.gameObject.GetComponent<Health>())
                {
                    if (damage >= hit.transform.gameObject.GetComponent<Health>().health)
                    {
                        RoomManager.Instance.kills++;
                        RoomManager.Instance.SetHashes();
                    }
                    hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
                    
                }
            }
        }
    }

    void UpdateAmmoUI()
    {
        ammoText.text = $"{bulletsInMag} / {totalBullets}";
    }
    
}

