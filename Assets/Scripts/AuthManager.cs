using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField ageInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text statusText;      
    [SerializeField] private MyDatabase database;       

    public bool isSignUpMode;                        

    FirebaseAuth auth;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    
    public void OnSubmitClicked()
    {
        Debug.Log("Submit pressed. Sign up mode = " + isSignUpMode);

        if (isSignUpMode) SignUp();
        else Login();
    }

    void SignUp()
    {
        string userName = nameInput.text.Trim();
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(userName))
        {
            ShowStatus("Please type a name first.");
            return;
        }
        if (!int.TryParse(ageInput.text, out int age))
        {
            ShowStatus("Please enter a valid age.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ShowStatus(GetErrorMessage(task.Exception));
                    return;
                }

                FirebaseUser newUser = task.Result.User;
                ShowStatus("Account created: " + newUser.Email);

                // hand the profile over to the database script
                database.WriteProfile(newUser.UserId, userName, newUser.Email, age);
            });
    }

    void Login()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ShowStatus(GetErrorMessage(task.Exception));
                    return;
                }

                FirebaseUser user = task.Result.User;
                ShowStatus("Logged in as " + user.Email);

                database.ReadProfile(user.UserId);
            });
    }

    public void OnLogoutClicked()
    {
        auth.SignOut();
        ShowStatus("Logged out.");
    }

    string GetErrorMessage(System.AggregateException ex)
    {
        FirebaseException fbEx = ex?.GetBaseException() as FirebaseException;
        if (fbEx == null) return "Something went wrong. Try again.";

        switch ((AuthError)fbEx.ErrorCode)
        {
            case AuthError.EmailAlreadyInUse: return "That email is already registered.";
            case AuthError.InvalidEmail:      return "That email address isn't valid.";
            case AuthError.WeakPassword:      return "Password must be at least 6 characters.";
            case AuthError.WrongPassword:     return "Wrong password.";
            case AuthError.UserNotFound:      return "No account with that email.";
            default:                          return "Error: " + fbEx.Message;
        }
    }

    void ShowStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null) statusText.text = message;
    }
}