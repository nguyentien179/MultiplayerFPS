using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Pool;
using System.Collections;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviourPun
{
    public static SoundManager Instance;
    public AudioSource footstepSource;
    public AudioClip footstepSFX;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource gunShotSource;
    public AudioClip[] gunShotSFX;
    public AudioClip[] equipSFX;
    public AudioClip[] reloadSFX;
    public void PlayFootstep()
    {
        GetComponent<PhotonView>().RPC("PlayFootstep_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayFootstep_RPC()
    {
        footstepSource.clip = footstepSFX;
        
        footstepSource.pitch = Random.Range(0.7f, 1.2f);
        footstepSource.volume = Random.Range(0.2f, 0.35f);
        footstepSource.Play();
    }

    public void PlayShoot(int index)
    {
        GetComponent<PhotonView>().RPC("PlayShoot_RPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void PlayShoot_RPC(int index)
    {
       gunShotSource.clip = gunShotSFX[index];
        
       gunShotSource.pitch = Random.Range(0.7f, 1.2f);
       gunShotSource.volume = Random.Range(0.2f, 0.35f);
       gunShotSource.Play();
    }

    public void PlayEquip(int index)
    {
        GetComponent<PhotonView>().RPC("PlayEquip_RPC", RpcTarget.All, index);
    }
    
    [PunRPC]
    public void PlayEquip_RPC(int index)
    {
        gunShotSource.clip = equipSFX[index];
        
        gunShotSource.pitch = Random.Range(0.7f, 1.2f);
        gunShotSource.volume = Random.Range(0.2f, 0.35f);
        gunShotSource.Play();
    }
    
    public void PlayReload(int index)
    {
        GetComponent<PhotonView>().RPC("PlayReload_RPC", RpcTarget.All, index);
    }
    
    [PunRPC]
    public void PlayReload_RPC(int index)
    {
        gunShotSource.clip = reloadSFX[index];
        
        gunShotSource.pitch = Random.Range(0.7f, 1.2f);
        gunShotSource.volume = Random.Range(0.2f, 0.35f);
        gunShotSource.Play();
    }
}
