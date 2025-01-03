using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class PlayerSetup : MonoBehaviour
{
    public PlayerMovement movement;

    public Sliding sliding;

    public GameObject camera;
    
    public string characterName;
    
    public TextMeshPro nameText;

    public Transform TPWeaponHolder;
    
    // Start is called before the first frame update
    public void IsLocalPlayer()
    {
        movement.enabled = true;
        sliding.enabled = true;
        camera.SetActive(true);
        
        TPWeaponHolder.gameObject.SetActive(false);
    }

    [PunRPC]
    public void SetCharacterName(string _name)
    {
        characterName = _name;
        nameText.text = _name;
    }
    
    [PunRPC]
    public void SetTPWeapon(int _weaponIndex)
    {
        foreach (Transform _weapon in TPWeaponHolder)
        {
            _weapon.gameObject.SetActive(false);
        }
        
        TPWeaponHolder.GetChild(_weaponIndex).gameObject.SetActive(true);
    }
   
    
}
