using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public PhotonView playerSetupView;
    private int selectedWeapon = 0;
    private int previousSelectedWeapon = -1; // Track previous weapon to avoid redundant animations
    private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        HandleWeaponSwitch();
    }

    void HandleWeaponSwitch()
    {
        previousSelectedWeapon = selectedWeapon; // Store the currently selected weapon

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        playerSetupView.RPC("SetTPWeapon", RpcTarget.All, selectedWeapon);
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                PlayEquipAnimation(weapon);
                SoundManager.Instance.PlayEquip(selectedWeapon);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    void PlayEquipAnimation(Transform weapon)
    {
        // Ensure the weapon has an animator and play the equip animation
        Animator weaponAnimator = weapon.GetComponent<Animator>();
        if (weaponAnimator != null)
        {
            weaponAnimator.SetTrigger("Equip");
        }
    }
}