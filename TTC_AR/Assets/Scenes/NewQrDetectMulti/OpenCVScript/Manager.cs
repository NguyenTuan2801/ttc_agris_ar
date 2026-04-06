using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity.UnityUtils.Helper;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text title;
    [SerializeField] private ARHelperMulti aRHelper;
    [SerializeField] private Button backBtn;
    [SerializeField] private EventPublisher eventPublisher;
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    public GameObject arGameObjectPrefab;
    public float relativeScale = 1.2f;        
    public Vector3 localOffset = new Vector3(0, 0.18f, 0);  

    private VuforiaBarcodeARManager arManager;

    private string activeScene;

    public bool IsCanvasOpen => canvas.gameObject.activeSelf;
    public bool enableQRCodeDetection = true;
    private bool enableQRCodeDetectionByActiveCameraIcon = true;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(CloseCanvas);
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        arManager = FindObjectOfType<VuforiaBarcodeARManager>();
    }

    private void Update()
    {
    }

    // Hàm này được gọi từ AR Button
    public void OpenModuleFromAR(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName)) return;

        GlobalVariable.objectName = moduleName;
        title.text = "Tủ " + moduleName;

        OpenCanvas();

        InitModuleScanQRView view = FindObjectOfType<InitModuleScanQRView>();
        if (view != null)
        {
            view.LoadListModule();           // Load danh sách module
            Debug.Log($"Đang load dữ liệu cho module: {moduleName}");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy InitModuleScanQRView");
        }

        OpenModuleGeneralPanelView detailView = FindObjectOfType<OpenModuleGeneralPanelView>();
        if (detailView != null)
        {
            detailView.LoadModuleInfor();    // Load thông tin chi tiết module
        }
    }

    public void OpenCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(true);
            enableQRCodeDetection = false;

            // Ẩn tất cả AR Button khi mở canvas
            var allARButtons = GameObject.FindObjectsOfType<ARQRMarker>(true);
            foreach (var marker in allARButtons)
            {
                if (marker.gameObject != null)
                    marker.gameObject.SetActive(false);
            }

            // Tắt BarcodeScanner để ngừng quét
            var barcodeScanner = FindObjectOfType<VuforiaBarcodeARManager>();
            if (barcodeScanner != null)
                barcodeScanner.enabled = false;

            Debug.Log("Đã ẩn hết AR Button và tắt quét AR");
        }
        else
        {
            Debug.LogError("Canvas chưa được gán!");
        }
    }

    public void CloseCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);

            if (enableQRCodeDetectionByActiveCameraIcon)
                enableQRCodeDetection = true;

            // Bật lại quét AR và hiện button
            var barcodeScanner = FindObjectOfType<VuforiaBarcodeARManager>();
            if (barcodeScanner != null)
                barcodeScanner.enabled = true;

            // Hiện lại các button AR
            var allARButtons = GameObject.FindObjectsOfType<ARQRMarker>(true);
            foreach (var marker in allARButtons)
            {
                if (marker.gameObject != null)
                    marker.gameObject.SetActive(true);
            }

            Debug.Log("Đã bật lại quét AR");
        }
    }

    public void setQRCodeDetection(bool enable)
    {
        enableQRCodeDetection = enable;
        enableQRCodeDetectionByActiveCameraIcon = enable;
    }

}
