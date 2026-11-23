using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject formPanel;
   
    [SerializeField] private TMP_Text submitButtonLabel;  
    [SerializeField] private AuthManager AuthManager;

    // name and age are only needed when creating an account
    [SerializeField] private GameObject nameField;
    [SerializeField] private GameObject nameLabel;
    [SerializeField] private GameObject ageField;
    [SerializeField] private GameObject ageLabel;

    void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        formPanel.SetActive(false);
    }

    public void ShowLogin()
    {
        AuthManager.isSignUpMode = false;
        
        if (submitButtonLabel != null) submitButtonLabel.text = "Login";
        SetProfileFields(false);
        OpenForm();
    }

    public void ShowSignUp()
    {
        AuthManager.isSignUpMode = true;
        
        if (submitButtonLabel != null) submitButtonLabel.text = "Sign Up";
        SetProfileFields(true);
        OpenForm();
    }

    void SetProfileFields(bool show)
    {
        nameField.SetActive(show);
        nameLabel.SetActive(show);
        ageField.SetActive(show);
        ageLabel.SetActive(show);
    }

    void OpenForm()
    {
        menuPanel.SetActive(false);
        formPanel.SetActive(true);
    }
}