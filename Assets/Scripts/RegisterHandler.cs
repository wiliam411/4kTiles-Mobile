using Client;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterHandler : MonoBehaviour
{
    private string name;
    private string email;
    private string password;
    private string confirmPassword;

    [SerializeField]
    InputField InputFieldName;
    [SerializeField]
    InputField InputFieldEmail;
    [SerializeField]
    InputField InputFieldPass;
    [SerializeField]
    InputField InputFieldConfirmPass;
    [SerializeField]
    Text ErrorText;

    // Start is called before the first frame update
    void Start()
    {
        if (InputFieldName != null)
        {
            InputFieldName.onEndEdit.AddListener(s =>
            {
                name = s;
            });
        }

        if (InputFieldEmail != null)
        {
            InputFieldEmail.onEndEdit.AddListener(s =>
            {
                email = s;
            });
        }
        if (InputFieldPass != null)
        {
            InputFieldPass.onEndEdit.AddListener(s =>
            {
                password = s;
            });
        }

        if (InputFieldConfirmPass != null)
        {
            InputFieldConfirmPass.onEndEdit.AddListener(s =>
            {
                confirmPassword = s;
            });
        }
    }

    public void Submit()
    {
        if (password != confirmPassword)
        {
            ErrorText.text = "Confirm Password does not match!";
        }
        else
        {
            var register = new Register()
            {
                email = email,
                userName = name,
                password = password,
            };
            var postData = JsonUtility.ToJson(register);
            StartCoroutine(
            ClientConstants.API.Post("Account/RegisterUser", postData, HttpClientRequest.ConvertToResponseAction<RegisterResponse>(result =>
            {
                if (!result.IsParseSuccess)
                {
                    ErrorText.text = "Create Register failed!";
                    return;
                }
                if (result.Result.errorCode == -1990)
                {
                    ErrorText.text = "Email or Password does not exist!";
                    return;
                }
                if (result.Result.errorCode == 1)
                {
                    ErrorText.text = "Email is Existed!";
                    return;
                }                
                Debug.Log(result.Result.data);
            }))
            );
        }
    }
}
