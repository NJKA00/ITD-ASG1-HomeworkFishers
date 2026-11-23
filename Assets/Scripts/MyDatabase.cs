using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class MyDatabase : MonoBehaviour
{
    DatabaseReference dbRoot;

    void Awake()
    {
        dbRoot = FirebaseDatabase
            .GetInstance("https://okay-141e7-default-rtdb.asia-southeast1.firebasedatabase.app")
            .RootReference;
    }

    
    public void WriteProfile(string uid, string name, string email, int age)
    {
        User profile = new User(name, email, age);
        string json = JsonUtility.ToJson(profile);

        dbRoot.Child("users").Child(uid).SetRawJsonValueAsync(json);
    }

    public void ReadProfile(string uid)
    {
        dbRoot.Child("users").Child(uid).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Could not load profile: " + task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;
                if (!snapshot.Exists)
                {
                    Debug.Log("Finished.");
                    return;
                }

                User profile = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                profile.DisplayUserInfo();
            });
    }
}