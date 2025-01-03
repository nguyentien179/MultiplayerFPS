using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Linq;
using Photon.Pun.UtilityScripts;

public class Leaderboard : MonoBehaviour
{
    public GameObject playerHolder;

    [Header("Options")] public float refreshRate = 1f;

    [Header("UI")] 
    public GameObject[] slots;

    [Space] 
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] KDTexts;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    // Update is called once per frame
    void Update()
    {
        playerHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }

    public void Refresh()
    {
        // Reset all slots
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        // Sort players by score in descending order
        var sortedPlayerList =
            PhotonNetwork.PlayerList.OrderByDescending(player => player.GetScore()).ToList();

        // Populate the slots with sorted player data
        int i = 0;
        foreach (var player in sortedPlayerList)
        {
            slots[i].SetActive(true);

            // Set player name
            string playerName = string.IsNullOrEmpty(player.NickName) ? "John Doe" : player.NickName;
            nameTexts[i].text = playerName;

            // Set player score
            scoreTexts[i].text = player.GetScore().ToString();

            // Set kills/deaths (default to "0/0" if missing)
            if (player.CustomProperties.TryGetValue("kills", out var kills) &&
                player.CustomProperties.TryGetValue("deaths", out var deaths))
            {
                KDTexts[i].text = $"{kills}/{deaths}";
            }
            else
            {
                KDTexts[i].text = "0/0";
            }

            i++;
        }
    }

    
    
}
