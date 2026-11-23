using UnityEngine;

public class User
{

    public string Username;
    public string Email;
    public int Age;

    public User() { }

    // Constructor
    public User(string username, string email, int age)
    {
        this.Username = username;
        this.Email = email;
        this.Age = age;
    }

    public void DisplayUserInfo()
    {
        Debug.Log($"Username: {Username}, Email: {Email}, Age: {Age}");
    }
}