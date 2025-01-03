using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public DatabaseReference Database;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Database = FirebaseDatabase.GetInstance("https://game-f2119-default-rtdb.asia-southeast1.firebasedatabase.app/").RootReference;
                Debug.Log("Firebase Database initialized successfully.");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }
    public void UpdatePlayerData(string userId, Dictionary<string, object> data)
    {
        Database.Child("players").Child(userId).UpdateChildrenAsync(data)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player data updated successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to update player data: {task.Exception}");
                }
            });
    }
}