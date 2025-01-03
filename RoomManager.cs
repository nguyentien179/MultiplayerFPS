using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public GameObject player;

    [Space] public Transform spawnPoint;
    [Space] public GameObject roomCam;

    [Space]
    public GameObject nameUI;

    public GameObject connectingUI;

    private string characterName = "JohnDoe";

    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;

    public string roomNameToJoin = "test";
    
    

    private void Awake()
    {
        Instance = this;
        string userId = PhotonNetwork.LocalPlayer.UserId; 
        Debug.Log(userId);
        
    }
    

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        
        roomCam.SetActive(false);

        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        
        _player.GetComponent<PhotonView>().RPC("SetCharacterName", RpcTarget.AllBuffered, characterName);

        PhotonNetwork.LocalPlayer.NickName = characterName;
    }

    public void ChangeName(string _name)
    {
        characterName = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting");
        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    
    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            SavePlayerStatsToFirebase();
        }
        catch
        {
           //idk
        }
    }
    
    private void SavePlayerStatsToFirebase()
    {
        string userId = PhotonNetwork.LocalPlayer.UserId; 
        Debug.Log(userId);
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
            { "name", characterName },
            { "kills", kills },
            { "deaths", deaths }
        };
        FirebaseManager.Instance.UpdatePlayerData(userId, playerData);
    }
}
