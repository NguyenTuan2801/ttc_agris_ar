using EasyUI.Progress;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginView : MonoBehaviour, ILoginView
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private string targetSceneName;

    [SerializeField] private Button ShowPasswordButton;
    [SerializeField] private Button HidePasswordButton;

    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject registerCanvas;

    private string email = "";
    private string password = "";

    private readonly Dictionary<string, string> staffAccounts = new()
    {
        {"ttc", "123456"},
        {"admin", "123456"},
        {"nhut", "123456"},
        {"my", "123456"}
    };
    public LoginPresenter _presenter;

    void Awake()
    {
        StopAllCoroutines();
        StartCoroutine(Init());
        Screen.orientation = ScreenOrientation.Portrait;
        PopulateFieldsIfLoggedIn();
    }

    private IEnumerator Init()
    {
        yield return new WaitUntil(() => ManagerLocator.Instance.CompanyManager != null &&
            ManagerLocator.Instance.GrapperManager != null);
        _presenter = new LoginPresenter(this,
            ManagerLocator.Instance.CompanyManager._ICompanyService,
            ManagerLocator.Instance.GrapperManager._IGrapperService,
            ManagerLocator.Instance.JBManager._IJBService,
            ManagerLocator.Instance.ImageManager._IImageService,
            ManagerLocator.Instance.ModuleManager._IModuleService,
            ManagerLocator.Instance.MccManager._IMccService,
            ManagerLocator.Instance.FieldDeviceManager._IFieldDeviceService,
            ManagerLocator.Instance.DeviceManager._IDeviceService,
            ManagerLocator.Instance.ModuleSpecificationManager._IModuleSpecificationService,
            ManagerLocator.Instance.AdapterSpecificationManager._IAdapterSpecificationService,
            ManagerLocator.Instance.RackManager._IRackService
        );
    }

    private void OnEnable()
    {
        GlobalVariable.APIRequestType.Clear();
        loginButton.onClick.RemoveAllListeners(); 
        SetupButtonListeners(); 
    }

    private void Start()
    {
        SetupButtonListeners(); 
    }

    private void Update()
    {
        HandleExitInput();
    }

    private void PopulateFieldsIfLoggedIn()
    {
        if (GlobalVariable.loginSuccess &&
            !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.email) &&
            !string.IsNullOrWhiteSpace(GlobalVariable.accountModel.password))
        {
            userNameField.text = GlobalVariable.accountModel.email;
            passwordField.text = GlobalVariable.accountModel.password;
        }
    }

    private void HandleExitInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Gamepad.current?.buttonEast?.wasPressedThisFrame == true ||
            Keyboard.current?.escapeKey.wasPressedThisFrame == true)
        {
            Application.Quit();
        }
    }

    private void HandleLogin()
    {
        email = userNameField.text.Trim();
        password = passwordField.text.Trim();

        if (staffAccounts.TryGetValue(email.ToLower(), out string correctPassword) &&
            password == correctPassword)
        {
            LoginProcess();
        }
        else
        {
            StartCoroutine(LoginWithApi(email, password));
        }

    }

    private void OpenRegisterScene()
    {
        loginCanvas.SetActive(false);
        registerCanvas.SetActive(true);
    }

    private void ShowPassword()
    {
        passwordField.contentType = TMP_InputField.ContentType.Standard;
        passwordField.ForceLabelUpdate();
        ShowPasswordButton.gameObject.SetActive(false);
        HidePasswordButton.gameObject.SetActive(true);
    }

    private void HidePassword()
    {
        passwordField.contentType = TMP_InputField.ContentType.Password;
        passwordField.ForceLabelUpdate();
        HidePasswordButton.gameObject.SetActive(false);
        ShowPasswordButton.gameObject.SetActive(true);
    }

    private void LoginProcess()
    {
        _presenter.LoginProcess();
    }

    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }

    public void ShowLoading(string title) => ShowProgressBar(title, "Đang đăng nhập...");
    public void HideLoading() => StartCoroutine(HideProgressBar());
    public void ShowError(string message) => StartCoroutine(ShowLoginFailure(message));
    public void ShowSuccess() => StartCoroutine(NavigateToScene(targetSceneName));
    public IEnumerator NavigateToScene(string sceneName)
    {
        SetLoginSuccessData();
        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator ShowLoginFailure(string message)
    {
        Show_Toast.Instance.ShowToast("failure", message);
        GlobalVariable.loginSuccess = false;
        yield return new WaitForSeconds(0.25f);
        yield return Show_Toast.Instance.Set_Instance_Status_False();
    }

    private void SetLoginSuccessData()
    {
        GlobalVariable.accountModel.email = email;
        GlobalVariable.accountModel.password = password;
        GlobalVariable.ready_To_Nav_New_Scene = true;
        GlobalVariable.recentScene = targetSceneName;
        GlobalVariable.previousScene = MyEnum.LoginScene.GetDescription();
        GlobalVariable.loginSuccess = true;
    }

    private IEnumerator HideProgressBar()
    {
        yield return new WaitForSeconds(0.5f);
        Progress.Hide();
    }

    // Coroutine để gọi API login
    private IEnumerator LoginWithApi(string email, string password)
    {
        //ShowLoading("Đang đăng nhập...");

        // Tạo JSON data
        LoginData data = new LoginData
        {
            email = email,
            password = password
        };

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest($"{GlobalVariable.baseUrl}/Users/login", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            Debug.Log($"Status Code: {www.responseCode}");
            Debug.Log($"Response: {www.downloadHandler.text}");
            Debug.Log($"Error (if any): {www.error}");

            if (www.result == UnityWebRequest.Result.Success && www.responseCode == 200)
            {
                string response = www.downloadHandler.text;
                LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);
                if (!string.IsNullOrEmpty(loginResponse.token))
                {
                    PlayerPrefs.SetString("AuthToken", loginResponse.token); // Lưu token
                    HideLoading();
                    LoginProcess(); // Gọi LoginProcess thay vì ShowSuccess để giữ logic ban đầu
                }
                else
                {
                    HideLoading();
                    ShowError("Không nhận được token từ server!");
                }
            }
            else
            {
                HideLoading();
                //string errorMessage = www.isNetworkError ? www.error : www.downloadHandler.text;
                //if (string.IsNullOrEmpty(errorMessage)) errorMessage = "Lỗi không xác định khi đăng nhập.";
                //ShowError(errorMessage);
                ShowError("Email hoặc mật khẩu không chính xác!");
            }
        }
    }

    // Phương thức để gắn listener
    private void SetupButtonListeners()
    {
        loginButton.onClick.AddListener(HandleLogin);
        registerButton.onClick.AddListener(OpenRegisterScene);
        ShowPasswordButton.onClick.AddListener(ShowPassword);
        HidePasswordButton.onClick.AddListener(HidePassword);

        passwordField.contentType = TMP_InputField.ContentType.Password;
    }

    // Class để serialize thành JSON
    [System.Serializable]
    private class LoginData
    {
        public string email;
        public string password;
    }

    // Class để deserialize response từ API
    [System.Serializable]
    private class LoginResponse
    {
        public string token;
    }
}