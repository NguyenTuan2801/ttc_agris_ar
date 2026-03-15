using EasyUI.Progress;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterView : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmPasswordInput;
    [SerializeField] private Button registerButton;
    //[SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject registerCanvas;

    [SerializeField] private Button backButton;

    [SerializeField] private Button ShowPasswordButtonForPasswordInput;
    [SerializeField] private Button HidePasswordButtonForPasswordInput;
    [SerializeField] private Button ShowPasswordButtonForConfirmPasswordInput;
    [SerializeField] private Button HidePasswordButtonForConfirmPasswordInput;

    void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        backButton.onClick.AddListener(BackLoginCanvas);
        ShowPasswordButtonForPasswordInput.onClick.AddListener(ShowPasswordForPasswordInput);
        HidePasswordButtonForPasswordInput.onClick.AddListener(HidePasswordForPasswordInput);
        ShowPasswordButtonForConfirmPasswordInput.onClick.AddListener(ShowPasswordForConfirmPasswordInput);
        HidePasswordButtonForConfirmPasswordInput.onClick.AddListener(HidePasswordForConfirmPasswordInput);

        passwordInput.contentType = TMP_InputField.ContentType.Password;
        confirmPasswordInput.contentType = TMP_InputField.ContentType.Password;
    }

    private void BackLoginCanvas()
    {
        registerCanvas.SetActive(false);
        loginCanvas.SetActive(true);
    }

    private void ShowPasswordForPasswordInput()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Standard;
        passwordInput.ForceLabelUpdate();
        ShowPasswordButtonForPasswordInput.gameObject.SetActive(false);
        HidePasswordButtonForPasswordInput.gameObject.SetActive(true);
    }
    private void HidePasswordForPasswordInput()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();
        ShowPasswordButtonForPasswordInput.gameObject.SetActive(true);
        HidePasswordButtonForPasswordInput.gameObject.SetActive(false);
    }
    private void ShowPasswordForConfirmPasswordInput()
    {
        confirmPasswordInput.contentType = TMP_InputField.ContentType.Standard;
        confirmPasswordInput.ForceLabelUpdate();
        ShowPasswordButtonForConfirmPasswordInput.gameObject.SetActive(false);
        HidePasswordButtonForConfirmPasswordInput.gameObject.SetActive(true);
    }
    private void HidePasswordForConfirmPasswordInput()
    {
        confirmPasswordInput.contentType = TMP_InputField.ContentType.Password;
        confirmPasswordInput.ForceLabelUpdate();
        ShowPasswordButtonForConfirmPasswordInput.gameObject.SetActive(true);
        HidePasswordButtonForConfirmPasswordInput.gameObject.SetActive(false);
    }

    void OnRegisterButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            //errorText.text = "All fields are required!";
            ShowError("Chưa nhập email hoặc mật khẩu!");
            return;
        }

        if (password != confirmPassword)
        {
            //errorText.text = "Passwords do not match!";
            ShowError("Mật khẩu không khớp!");
            return;
        }

        StartCoroutine(Register(email, password, confirmPassword));
    }

    IEnumerator Register(string email, string password, string confirmPassword)
    {
        // Tạo JSON data
        RegisterData data = new RegisterData
        {
            email = email,
            password = password,
            confirmPassword = confirmPassword
        };

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest($"{GlobalVariable.baseUrl}/Users/register", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            Debug.Log($"Status Code: {www.responseCode}");
            Debug.Log($"Response: {www.downloadHandler.text}");

            if (www.result == UnityWebRequest.Result.Success && www.responseCode == 200)
            {
                ShowSuccess("Đăng ký thành công!");
                HideLoading();
                ShowLoading(" ");
                yield return new WaitForSeconds(2);
                if (registerCanvas != null) registerCanvas.SetActive(false);
                if (loginCanvas != null) loginCanvas.SetActive(true);
                else Debug.LogError("loginCanvas is not assigned!");
            }
            else
            {
                
                //string errorMessage = www.isNetworkError ? www.error : www.downloadHandler.text;
                //if (string.IsNullOrEmpty(errorMessage)) errorMessage = "Unknown error occurred.";
                //errorText.text = $"Error: {errorMessage}";
            }
        }
    }

    // Class để serialize thành JSON
    [Serializable]
    private class RegisterData
    {
        public string email;
        public string password;
        public string confirmPassword;
    }

    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }

    private IEnumerator HideProgressBar()
    {
        yield return new WaitForSeconds(0.5f);
        Progress.Hide();
    }

    private IEnumerator ShowRegisterFailure(string message)
    {
        Show_Toast.Instance.ShowToast("failure", message);
        yield return new WaitForSeconds(0.25f);
        yield return Show_Toast.Instance.Set_Instance_Status_False();
    }

    private IEnumerator ShowRegisterSuccess(string message)
    {
        Show_Toast.Instance.ShowToast("success", message);
        yield return new WaitForSeconds(2);
        yield return Show_Toast.Instance.Set_Instance_Status_False();
    }

    public void ShowLoading(string title) => ShowProgressBar(title, " ");
    public void HideLoading() => StartCoroutine(HideProgressBar());
    public void ShowError(string message) => StartCoroutine(ShowRegisterFailure(message));
    public void ShowSuccess(string message) => StartCoroutine(ShowRegisterSuccess(message));

    void Update()
    {

    }
}