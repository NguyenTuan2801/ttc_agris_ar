using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListDeviceSettingView : MonoBehaviour, IDeviceView
{
    [Header("Canvas")]
    public GameObject List_Device_Canvas;
    public GameObject Add_New_Device_Canvas;
    public GameObject Update_Device_Canvas;
    public ScrollRect scrollRect;

    public GameObject Device_Item_Prefab;
    public GameObject Parent_Vertical_Layout_Group;
    public ScrollRect scrollView;
    private List<GameObject> listDeviceItems = new List<GameObject>();

    [Header("Search")]
    public TMP_InputField searchInputField; // Thanh tìm kiếm
    public Button clearButton; // Nút xóa nội dung tìm kiếm
    private List<DeviceInformationModel> allDevices = new List<DeviceInformationModel>(); // Lưu dữ liệu gốc

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;
    private DevicePresenter _presenter;
    private GameObject _deviceItem;
    private int grapperId;
    private Sprite warningConfirmButtonSprite;

    void Awake()
    {
        // var DeviceManager = FindObjectOfType<DeviceManager>();
        _presenter = new DevicePresenter(this,
        ManagerLocator.Instance.DeviceManager._IDeviceService);
        // DeviceManager._IDeviceService

        // Đăng ký sự kiện cho nút xóa nội dung tìm kiếm
        if (clearButton != null)
        {
            clearButton.onClick.RemoveAllListeners();
            clearButton.onClick.AddListener(ClearSearchInput);
        }
    }
    void OnEnable()
    {
        warningConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Warning_Back_Button_Background");
        Debug.Log(warningConfirmButtonSprite);
        grapperId = GlobalVariable.GrapperId;
        LoadListDevice();
    }
    void OnDisable()
    {
    }

    private void RefreshList()
    {
        Device_Item_Prefab.SetActive(true);

        foreach (var item in listDeviceItems)
        {
            if (item != Device_Item_Prefab)
                Destroy(item);
        }

        listDeviceItems.Clear();
    }

    public void LoadListDevice()
    {
        RefreshList();
        _presenter.LoadListDeviceGeneral(grapperId);
    }
    public void DisplayList(List<DeviceInformationModel> models)
    {
        // Lưu dữ liệu gốc để sử dụng cho tìm kiếm
        allDevices = models ?? new List<DeviceInformationModel>();
        RefreshList();

        if (allDevices.Any())
        {
            CreateDeviceItems(allDevices);           
        }
        else
        {
            Debug.Log("No Devices found");
        }
        Device_Item_Prefab.SetActive(false);
        scrollRect.verticalNormalizedPosition = 1;

        // Đăng ký sự kiện tìm kiếm
        if (searchInputField != null)
        {
            searchInputField.onValueChanged.RemoveAllListeners();
            searchInputField.onValueChanged.AddListener(OnSearchValueChanged);
        }
    }

    private void EditDeviceItem(int id)
    {
        GlobalVariable.deviceId = id;
        OpenUpdateCanvas();
    }
    private void DeleDeviceItem(GameObject DeviceItem, DeviceInformationModel model)
    {
        OpenDeleteWarningDialog(DeviceItem, model);
    }

    public void OpenAddNewCanvas()
    {
        Add_New_Device_Canvas.SetActive(true);
        List_Device_Canvas.SetActive(false);
        Update_Device_Canvas.SetActive(false);
    }
    private void OpenUpdateCanvas()
    {
        List_Device_Canvas.SetActive(false);
        Add_New_Device_Canvas.SetActive(false);
        Update_Device_Canvas.SetActive(true);
    }

    private void OpenDeleteWarningDialog(GameObject DeviceItem, DeviceInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");

        var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;

        var dialogText = $"Bạn có chắc chắn muốn xóa thiết bị <color=#ED1C24><b>{model.Code}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút \"xác nhận\" phía dưới";

        DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = dialogText;

        var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa thiết bị khỏi hệ thống?";

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Warning_Icon_For_Dialog");

        var confirmButton = Horizontal_Group.Find("Confirm_Button").GetComponent<Button>();
        var backButton = Horizontal_Group.Find("Back_Button").GetComponent<Button>();

        var confirmButtonSprite = confirmButton.GetComponent<Image>();

        confirmButtonSprite.sprite = warningConfirmButtonSprite;

        var confirmButtonText = confirmButton.GetComponentInChildren<TMP_Text>();
        var backButtonText = backButton.GetComponentInChildren<TMP_Text>();

        // var colors = confirmButton.colors;
        // colors.normalColor = new Color32(92, 237, 115, 255); // #5CED73 in RGB
        // confirmButton.colors = colors;

        confirmButtonText.text = "Xác nhận";
        backButtonText.text = "Trở lại";

        confirmButton.onClick.RemoveAllListeners();

        backButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            Debug.Log(model.Id);
            _deviceItem = DeviceItem;
            _presenter.DeleteDevice(model.Id);
            DialogTwoButton.SetActive(false);
        });
        backButton.onClick.AddListener(() =>
        {
            DialogTwoButton.SetActive(false);
        });
    }
    private void OpenErrorDialog(string title = "Xóa thiết bị thất bại", string message = "Đã có lỗi xảy ra khi xóa thiết bị khỏi hệ thống. Vui lòng thử lại sau")
    {
        DialogOneButton.SetActive(true);

        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();

        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Back_Button_Background");

        var dialog_Icon = DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Icon_For_Dialog");

        var dialog_Content = DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = message;

        var dialog_Title = DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;

        backButton.onClick.RemoveAllListeners();

        backButton.onClick.AddListener(() =>
        {
            DialogOneButton.SetActive(false);
        });

    }
    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }
    private void HideProgressBar()
    {
        Progress.Hide();
    }
    public void ShowLoading(string title) => ShowProgressBar(title, "Đang tải dữ liệu...");
    public void HideLoading() => HideProgressBar();

    public void ShowError(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Device_List_General"))
        {
            OpenErrorDialog(title: "Tải danh sách thất bại", message: "Đã có lỗi xảy ra khi tải danh sách. Vui lòng thử lại sau");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Device"))
        {
            OpenErrorDialog();
        }
    }
    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Device_List_General"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải danh sách thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Device"))
        {
            listDeviceItems.Remove(_deviceItem);
            Destroy(_deviceItem);
            Show_Toast.Instance.ShowToast("success", "Xóa thiết bị thành công");
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }

    // Không dùng trong ListView
    public void DisplayDetail(DeviceInformationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }

    private void CreateDeviceItems(List<DeviceInformationModel> devices)
    {
        foreach (var model in devices)
        {
            int DeviceIndex = devices.IndexOf(model);
            // Debug.Log(DeviceIndex);
            var newDeviceItem = Instantiate(Device_Item_Prefab, Parent_Vertical_Layout_Group.transform);
            newDeviceItem.SetActive(true);
            newDeviceItem.name = model.Code;
            Transform newDeviceItemTransform = newDeviceItem.transform;
            Transform newDeviceItemPreviewInforGroup = newDeviceItemTransform.GetChild(0);
            newDeviceItemPreviewInforGroup.Find("Preview_Device_Code").GetComponent<TMP_Text>().text = model.Code;
            TMP_Text grapperText = newDeviceItemPreviewInforGroup.Find("Preview_Device_GrapLocation")?.GetComponent<TMP_Text>();
            if (grapperText != null)
            {
                grapperText.text = GetGrapperName(grapperId);
            }
            Transform newDeviceItemPreviewButtonGroup = newDeviceItemTransform.GetChild(1);
            listDeviceItems.Add(newDeviceItem);
            newDeviceItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>().onClick.AddListener(() => EditDeviceItem(model.Id));
            newDeviceItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>().onClick.AddListener(() => DeleDeviceItem(newDeviceItem, model));
            listDeviceItems.Add(newDeviceItem);
        }
    }

    // Hàm xử lý nội dung tìm kiếm
    private void OnSearchValueChanged(string input)
    {
        string searchText = input.ToLower().Trim();

        foreach (var item in listDeviceItems)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                item.SetActive(true);
            }
            else
            {
                bool match = item.name.ToLower().Contains(searchText);
                item.SetActive(match);
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(Parent_Vertical_Layout_Group.GetComponent<RectTransform>());
        scrollView.verticalNormalizedPosition = 1f;
    }

    private void ClearSearchInput()
    {
        searchInputField.text = string.Empty;
        OnSearchValueChanged(string.Empty);
    }

    private string GetGrapperName(int id)
    {
        switch (id)
        {
            case 1: return "Grapper A";
            case 2: return "Grapper B";
            case 3: return "Grapper C";
            case 4: return "Lò hơi";
            default: return "Khu vực khác";
        }
    }
}