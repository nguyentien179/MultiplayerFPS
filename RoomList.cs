using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance;

    public GameObject roomManagerGameObject;
    public RoomManager roomManager;
    
    [Header("UI")] public Transform roomListParent;
    public GameObject roomListItemPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Update cachedRoomList properly
        foreach (var room in roomList)
        {
            int index = cachedRoomList.FindIndex(r => r.Name == room.Name);

            if (room.RemovedFromList)
            {
                
                if (index != -1)
                    cachedRoomList.RemoveAt(index);
            }
            else
            {
                
                if (index != -1)
                {
                    cachedRoomList[index] = room;
                }
                else
                {
                    cachedRoomList.Add(room);
                }
            }
        }

        // Update the UI after modifying the cachedRoomList
        UpdateUI();
    }

    void UpdateUI()
    {
        // Clear existing room items
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        // Populate room items
        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            // Ensure components are enabled before setting values
            roomItem.SetActive(true);
            foreach (Transform child in roomItem.transform)
            {
                child.gameObject.SetActive(true);
            }

            TextMeshProUGUI roomNameText = roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI playerCountText = roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            RoomItemButton roomButton = roomItem.GetComponent<RoomItemButton>();

            if (roomNameText != null)
                roomNameText.text = room.Name;

            if (playerCountText != null)
                playerCountText.text = room.PlayerCount + "/16";

            if (roomButton != null)
                roomButton.roomName = room.Name;
        }
    }


    public void JoinRoomByName(string _name)
    {
        roomManager.roomNameToJoin = _name;
        roomManagerGameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ChangeRoomToCreateName(string _roomName)
    {
        roomManager.roomNameToJoin = _roomName;
    }
}
