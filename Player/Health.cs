using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool isLocalPlayer;
    [Header("UI")]
    public TextMeshProUGUI healthText;

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = health.ToString();
        if (health <= 0)
        {
            if (isLocalPlayer)
            {
                RoomManager.Instance.RespawnPlayer();
                RoomManager.Instance.deaths++;
                RoomManager.Instance.SetHashes();
            }
            Destroy(gameObject);
        }
    }
}
