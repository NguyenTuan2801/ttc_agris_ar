using System;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CreateDeviceSettingView : MonoBehaviour, IDeviceView
{
    public Initialize_Device_List_Option_Selection initialize_Device_List_Option_Selection;
    private DevicePresenter _presenter;

    [SerializeField] private DeviceInformationModel DeviceInformationModel;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField deviceCode_TextField;
    [SerializeField] private TMP_InputField deviceFunction_TextField;
    [SerializeField] private TMP_InputField deviceType_TextField;
    [SerializeField] private TMP_InputField deviceIOAddress_TextField;    
    [SerializeField] private TMP_InputField deviceModelSeries_TextField;
    [SerializeField] private TMP_InputField deviceManufacturer_TextField;
    [SerializeField] private TMP_InputField devicePartNumber_TextField;
    [SerializeField] private TMP_InputField deviceSerialNumber_TextField;
    [SerializeField] private TMP_InputField deviceManufacturingYear_TextField;
    [SerializeField] private TMP_InputField deviceInstallationDate_TextField;
    [SerializeField] private TMP_InputField deviceMeasurementType_TextField;
    [SerializeField] private TMP_InputField deviceRange_TextField;
    [SerializeField] private TMP_InputField deviceUnit_TextField;
    [SerializeField] private TMP_InputField deviceAccuracy_TextField;
    [SerializeField] private TMP_InputField deviceTestError_TextField;
    [SerializeField] private TMP_InputField deviceLengthOrDN_TextField;
    [SerializeField] private TMP_InputField deviceSupplyVoltage_TextField;
    [SerializeField] private TMP_InputField deviceOutputSignal_TextField;
    [SerializeField] private TMP_InputField deviceIngressProtection_TextField;
    [SerializeField] private TMP_InputField deviceConnectorType_TextField;
    [SerializeField] private TMP_InputField deviceProcessConnection_TextField;
    [SerializeField] private TMP_InputField deviceResponseTime_TextField;
    [SerializeField] private TMP_InputField deviceOtherSpecifications_TextField;
    [SerializeField] private TMP_InputField deviceInstallationLocation_TextField;
    [SerializeField] private TMP_InputField deviceMeasuredMedium_TextField;
    [SerializeField] private TMP_InputField deviceOperatingTemperature_TextField;
    [SerializeField] private TMP_InputField deviceOperatingPressure_TextField;
    [SerializeField] private TMP_InputField deviceCalibrationFrequency_TextField;
    [SerializeField] private TMP_InputField deviceFailureHistory_TextField;
    [SerializeField] private TMP_InputField deviceEnvironmentCondition_TextField;

    [Header("Basic Information")]
    public ScrollRect basic_ScrollRect;
    public RectTransform basic_ViewPortTransform;
    public GameObject basic_Parent_Content_Vertical_Group;

    private Transform temp_Item_Transform;

    [Header("Technical Information")]
    public ScrollRect technical_ScrollRect;
    public RectTransform technical_ViewPortTransform;
    public GameObject technical_Parent_Content_Vertical_Group;

    [Header("Operational Information")]
    public ScrollRect operational_ScrollRect;
    public RectTransform operational_ViewPortTransform;
    public GameObject operational_Parent_Content_Vertical_Group;

    [Header("LayOutGroup")]
    // public GameObject List_Module_Parent_GridLayout_Group;
    public GameObject List_JB_Parent_GridLayout_Group;
    public GameObject List_Additional_Connection_Image_Parent_Vertical_Group;

    [Header("Prefab")]
    public GameObject JB_Item_Prefab;
    public GameObject Module_Item_Prefab;
    public GameObject ConnectionImage_Item_Prefab;

    [Header("Buttons")]
    [SerializeField] private Button submitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button backButtonJBListSelection;
    [SerializeField] private Button backButtonModuleListSelection;
    [SerializeField] private Button backButtonAdditionalConnectionImageListSelection;

    [SerializeField] private GameObject addModuleItem;

    [SerializeField] private Button basicInfoButton;
    [SerializeField] private Button technicalInfoButton;
    [SerializeField] private Button operationalInfoButton;

    [Header("Scroll Views")]
    public GameObject basicInfo_ScrollView;
    public GameObject technicalInfo_ScrollView;
    public GameObject operationalInfo_ScrollView;

    [Header("Dialog Buttons")]
    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;


    [Header("Canvas")]
    public GameObject List_Device_Canvas;
    public GameObject Add_New_Device_Canvas;
    public GameObject Update_Device_Canvas;
    private List<JBInformationModel> temp_JBModels = new List<JBInformationModel>();
    private ModuleInformationModel temp_ModuleModel = new ModuleInformationModel(1, "");
    private Dictionary<string, ImageInformationModel> temp_Dictionary_Additional_ConnectionModel = new();
    private Dictionary<string, JBInformationModel> temp_Dictionary_JBInformationModel = new();
    private int grapperId;
    private Sprite successConfirmButtonSprite;

    private readonly Dictionary<string, List<GameObject>> selectedGameObjects = new()
    {
        { "Module", new List<GameObject>() },
        { "Additional_Connection_Images", new List<GameObject>() },
        { "JB", new List<GameObject>() },

    };

    private readonly Dictionary<string, int> selectedCounts = new()
    {
        { "Module", 0 },
        { "Additional_Connection_Images", 0 },
        { "JB", 0 },
    };

    void Awake()
    {
        _presenter = new DevicePresenter(this, ManagerLocator.Instance.DeviceManager._IDeviceService);
    }

    void OnEnable()
    {
        successConfirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");
        Debug.Log(successConfirmButtonSprite);
        grapperId = GlobalVariable.GrapperId;

        RenewView();

        AddButtonListeners(initialize_Device_List_Option_Selection.JB_List_Selection_Option_Content_Transform, "JB");
        AddButtonListeners(initialize_Device_List_Option_Selection.Module_List_Selection_Option_Content_Transform, "Module");
        AddButtonListeners(initialize_Device_List_Option_Selection.Additional_Connection_Image_List_Selection_Option_Content_Transform, "Additional_Connection_Images");

        backButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();

        backButtonJBListSelection.onClick.RemoveAllListeners();
        backButtonModuleListSelection.onClick.RemoveAllListeners();
        backButtonAdditionalConnectionImageListSelection.onClick.RemoveAllListeners();
        basicInfoButton.onClick.RemoveAllListeners();
        technicalInfoButton.onClick.RemoveAllListeners();
        operationalInfoButton.onClick.RemoveAllListeners();

        backButtonJBListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("JB"));
        backButtonModuleListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("Module"));
        backButtonAdditionalConnectionImageListSelection.onClick.AddListener(() => CloseListSelectionFromBackButton("Additional_Connection_Images"));

        backButton.onClick.AddListener(CloseAddCanvas);
        submitButton.onClick.AddListener(OnSubmitButtonClick);

        basicInfoButton.onClick.AddListener(OpenBasicInfo);
        technicalInfoButton.onClick.AddListener(OpenTechnicalInfo);
        operationalInfoButton.onClick.AddListener(OpenOperationalInfo);

    }

    void OnDisable()
    {
        RenewView();
    }

    private void OnSubmitButtonClick()
    {

        if (string.IsNullOrEmpty(deviceCode_TextField.text))
        {
            OpenErrorDialog("Vui lòng nhập mã thiết bị");
            return;
        }
        if (GlobalVariable.temp_Dictionary_DeviceInformationModel.ContainsKey(deviceCode_TextField.text))
        {
            OpenErrorDialog("Thiết bị đã tồn tại", "Vui lòng nhập mã thiết bị khác");
            return;
        }
        else
        {
            DeviceInformationModel = new DeviceInformationModel(
            code: string.IsNullOrEmpty(deviceCode_TextField.text) ? throw new ArgumentNullException(nameof(deviceCode_TextField.text)) : deviceCode_TextField.text,
            function: string.IsNullOrEmpty(deviceFunction_TextField.text) ? "Chưa cập nhật" : deviceFunction_TextField.text,
            type: string.IsNullOrEmpty(deviceType_TextField.text) ? "Chưa cập nhật" : deviceType_TextField.text,
            ioAddress: string.IsNullOrEmpty(deviceIOAddress_TextField.text) ? "Chưa cập nhật" : deviceIOAddress_TextField.text,
            modelSeries: string.IsNullOrEmpty(deviceModelSeries_TextField.text) ? "Chưa cập nhật" : deviceModelSeries_TextField.text,
            manufacturer: string.IsNullOrEmpty(deviceManufacturer_TextField.text) ? "Chưa cập nhật" : deviceManufacturer_TextField.text,
            partNumber: string.IsNullOrEmpty(devicePartNumber_TextField.text) ? "Chưa cập nhật" : devicePartNumber_TextField.text,
            serialNumber: string.IsNullOrEmpty(deviceSerialNumber_TextField.text) ? "Chưa cập nhật" : deviceSerialNumber_TextField.text,
            manufacturingYear: string.IsNullOrEmpty(deviceManufacturingYear_TextField.text) ? "Chưa cập nhật" : deviceManufacturingYear_TextField.text,
            installationDate: string.IsNullOrEmpty(deviceInstallationDate_TextField.text) ? "Chưa cập nhật" : deviceInstallationDate_TextField.text,
            measurementType: string.IsNullOrEmpty(deviceMeasurementType_TextField.text) ? "Chưa cập nhật" : deviceMeasurementType_TextField.text,
            range: string.IsNullOrEmpty(deviceRange_TextField.text) ? "Chưa cập nhật" : deviceRange_TextField.text,
            unit: string.IsNullOrEmpty(deviceUnit_TextField.text) ? "Chưa cập nhật" : deviceUnit_TextField.text,
            accuracy: string.IsNullOrEmpty(deviceAccuracy_TextField.text) ? "Chưa cập nhật" : deviceAccuracy_TextField.text,
            testError: string.IsNullOrEmpty(deviceTestError_TextField.text) ? "Chưa cập nhật" : deviceTestError_TextField.text,
            lengthOrDN: string.IsNullOrEmpty(deviceLengthOrDN_TextField.text) ? "Chưa cập nhật" : deviceLengthOrDN_TextField.text,
            supplyVoltage: string.IsNullOrEmpty(deviceSupplyVoltage_TextField.text) ? "Chưa cập nhật" : deviceSupplyVoltage_TextField.text,
            outputSignal: string.IsNullOrEmpty(deviceOutputSignal_TextField.text) ? "Chưa cập nhật" : deviceOutputSignal_TextField.text,
            ingressProtection: string.IsNullOrEmpty(deviceIngressProtection_TextField.text) ? "Chưa cập nhật" : deviceIngressProtection_TextField.text,
            connectorType: string.IsNullOrEmpty(deviceConnectorType_TextField.text) ? "Chưa cập nhật" : deviceConnectorType_TextField.text,
            processConnection: string.IsNullOrEmpty(deviceProcessConnection_TextField.text) ? "Chưa cập nhật" : deviceProcessConnection_TextField.text,
            responseTime: string.IsNullOrEmpty(deviceResponseTime_TextField.text) ? "Chưa cập nhật" : deviceResponseTime_TextField.text,
            otherSpecifications: string.IsNullOrEmpty(deviceOtherSpecifications_TextField.text) ? "Chưa cập nhật" : deviceOtherSpecifications_TextField.text,

            jbInformationModels: temp_Dictionary_JBInformationModel.Any() ? temp_Dictionary_JBInformationModel.Values.ToList() : new List<JBInformationModel>(),
            moduleInformationModel: !addModuleItem.activeSelf ? temp_ModuleModel : null,
            additionalConnectionImages: temp_Dictionary_Additional_ConnectionModel.Any() ? temp_Dictionary_Additional_ConnectionModel.Values.ToList() : new List<ImageInformationModel>()
            );

            _presenter.CreateNewDevice(grapperId, DeviceInformationModel);
        }
    }

    private void RenewView()
    {
        ClearActiveChildren(List_JB_Parent_GridLayout_Group);
        ClearActiveChildren(List_Additional_Connection_Image_Parent_Vertical_Group);

        ResetAllInputFields();
        JB_Item_Prefab.SetActive(false);
        Module_Item_Prefab.SetActive(false);
        addModuleItem.SetActive(true);

        temp_Dictionary_Additional_ConnectionModel.Clear();
        temp_Dictionary_JBInformationModel.Clear();
        temp_ModuleModel = new ModuleInformationModel(1, "");

        selectedGameObjects["Additional_Connection_Images"].Clear();
        selectedGameObjects["JB"].Clear();
        selectedGameObjects["Module"].Clear();
        selectedCounts["Additional_Connection_Images"] = 0;
        selectedCounts["JB"] = 0;
        selectedCounts["Module"] = 0;
        basic_ScrollRect.verticalNormalizedPosition = 1;
        technical_ScrollRect.verticalNormalizedPosition = 1;
        operational_ScrollRect.verticalNormalizedPosition = 1;
    }

    public void CloseAddCanvas()
    {
        OpenBasicInfo();
        Add_New_Device_Canvas.SetActive(false);
        Update_Device_Canvas.SetActive(false);
        List_Device_Canvas.SetActive(true);
    }

    private void ResetAllInputFields()
    {
        deviceCode_TextField.text = "";
        deviceFunction_TextField.text = "";
        deviceIOAddress_TextField.text = "";
        deviceType_TextField.text = "";
        deviceModelSeries_TextField.text = "";
        deviceManufacturer_TextField.text = "";
        devicePartNumber_TextField.text = "";
        deviceSerialNumber_TextField.text = "";
        deviceManufacturingYear_TextField.text = "";
        deviceInstallationDate_TextField.text = "";
        deviceMeasurementType_TextField.text = "";
        deviceRange_TextField.text = "";
        deviceUnit_TextField.text = "";
        deviceAccuracy_TextField.text = "";
        deviceTestError_TextField.text = "";
        deviceLengthOrDN_TextField.text = "";
        deviceSupplyVoltage_TextField.text = "";
        deviceOutputSignal_TextField.text = "";
        deviceIngressProtection_TextField.text = "";
        deviceConnectorType_TextField.text = "";
        deviceProcessConnection_TextField.text = "";
        deviceResponseTime_TextField.text = "";
        deviceOtherSpecifications_TextField.text = "";
        deviceInstallationLocation_TextField.text = "";
        deviceMeasuredMedium_TextField.text = "";
        deviceOperatingTemperature_TextField.text = "";
        deviceOperatingPressure_TextField.text = "";
        deviceCalibrationFrequency_TextField.text = "";
        deviceFailureHistory_TextField.text = "";
        deviceEnvironmentCondition_TextField.text = "";
    }

    private void AddButtonListeners(Transform contentTransform, string field)
    {
        foreach (Transform option in contentTransform)
        {
            AddButtonListener(option, () => SelectOption(option.gameObject.GetComponentInChildren<TMP_Text>().text, field));
        }
    }

    private void AddButtonListener(Transform option, UnityEngine.Events.UnityAction action)
    {
        var button = option.gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }

    private void SelectOption(string optionTextValue, string field)
    {

        SetItemTextValue(temp_Item_Transform, optionTextValue, field);
        CloseListSelection(field);

        selectedCounts[field]++;
        if (!selectedGameObjects[field].Contains(temp_Item_Transform.gameObject))
        {
            selectedGameObjects[field].Add(temp_Item_Transform.gameObject);
        }
        else
        {
            Destroy(temp_Item_Transform.gameObject);
        }

    }

    private void SetItemTextValue(Transform temp_Item_Transform, string textValue, string field)
    {
        var itemText = temp_Item_Transform.GetComponentInChildren<TMP_Text>();
        if (itemText == null) return;
        itemText.text = textValue;

        switch (field)
        {

            case "Module":
                if (GlobalVariable.temp_Dictionary_ModuleInformationModel.TryGetValue(textValue, out var module))
                {
                    if (temp_ModuleModel == null || temp_ModuleModel.Name != textValue)
                    {
                        temp_ModuleModel = new ModuleInformationModel(module.Id, module.Name);
                    }
                }
                break;
            case "JB":
                if (GlobalVariable.temp_Dictionary_JBInformationModel.TryGetValue(textValue, out var jB))
                {
                    if (!temp_Dictionary_JBInformationModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_JBInformationModel[textValue] = new JBInformationModel(jB.Id, jB.Name);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;
            case "Additional_Connection_Images":
                if (GlobalVariable.temp_Dictionary_ImageInformationModel.TryGetValue(textValue, out var connectionImageInfo))
                {
                    if (!temp_Dictionary_Additional_ConnectionModel.ContainsKey(textValue))
                    {
                        temp_Dictionary_Additional_ConnectionModel[textValue] = new ImageInformationModel(connectionImageInfo.Id, connectionImageInfo.Name);
                    }
                    else
                    {
                        Destroy(temp_Item_Transform.gameObject);
                    }
                }
                break;
        }
    }

    public void OpenListJBSelection() => OpenListSelection("JB", JB_Item_Prefab, List_JB_Parent_GridLayout_Group);
    public void OpenListModuleSelection() => OpenListSelection("Module", Module_Item_Prefab, null);
    public void OpenListConnectionImageSelection() => OpenListSelection("Additional_Connection_Images", ConnectionImage_Item_Prefab, List_Additional_Connection_Image_Parent_Vertical_Group);

    public void OpenListSelection(string field, GameObject itemPrefab, GameObject parentGroup)
    {
        // if (!initialize_Device_List_Option_Selection.Selection_Option_Canvas.activeSelf)
        //     initialize_Device_List_Option_Selection.Selection_Option_Canvas.SetActive(true);
        if (field != "Module")
        {
            var newItem = Instantiate(itemPrefab, parentGroup.transform);
            newItem.SetActive(true);
            temp_Item_Transform = newItem.transform;
            var button = newItem.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => DeselectItem(newItem, field));
        }
        else
        {
            temp_Item_Transform = itemPrefab.transform;
            itemPrefab.SetActive(true);
            var button = itemPrefab.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => DeselectItem(itemPrefab, field));
            addModuleItem.SetActive(false);
        }

        GetSelectionPanel(field).SetActive(true);
    }

    private void DeselectItem(GameObject item, string field)
    {
        if (field == "Additional_Connection_Images")
        {
            selectedCounts[field]--;
            selectedGameObjects[field].Remove(item);
            temp_Dictionary_Additional_ConnectionModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
            Destroy(item);
        }
        if (field == "JB")
        {
            selectedCounts[field]--;
            selectedGameObjects[field].Remove(item);
            temp_Dictionary_JBInformationModel.Remove(item.GetComponentInChildren<TMP_Text>().text);
            Destroy(item);
        }
        if (field == "Module")
        {
            selectedCounts[field]--;
            selectedGameObjects[field].Remove(item);
            temp_ModuleModel = new ModuleInformationModel(1, "");
            item.SetActive(false);
            addModuleItem.SetActive(true);
        }
    }

    public void CloseListSelection(string field)
    {
        GetSelectionPanel(field).SetActive(false);
        // initialize_Device_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
    }

    public void CloseListSelectionFromBackButton(string field)
    {
        //ClearDeActiveChildren(List_JB_Parent_GridLayout_Group);
        temp_Item_Transform.gameObject.SetActive(false);
        GetSelectionPanel(field).SetActive(false);
        if (field == "Module")
        {
            addModuleItem.SetActive(true);
        }
        // initialize_Device_List_Option_Selection.Selection_Option_Canvas.SetActive(false);
    }

    private void ClearDeActiveChildren(GameObject parentGroup)
    {
        foreach (Transform child in parentGroup.transform)
        {
            if (!child.gameObject.activeSelf && parentGroup.transform.childCount > 1)
                Destroy(child.gameObject);
        }
    }

    private void ClearActiveChildren(GameObject parentGroup)
    {
        foreach (Transform child in parentGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject GetSelectionPanel(string field)
    {
        return field switch
        {
            "JB" => initialize_Device_List_Option_Selection.selection_List_JB_Panel,
            "Module" => initialize_Device_List_Option_Selection.selection_List_ModuleIO_Panel,
            "Additional_Connection_Images" => initialize_Device_List_Option_Selection.selection_List_Additional_Connection_Image_Panel,
            _ => throw new ArgumentException("Invalid field Name")
        };
    }

    private void OpenErrorDialog(string title = "Tạo thiết bị mới thất bại", string message = "Đã có lỗi xảy ra khi tạo thiết bị mới. Vui lòng thử lại sau.")
    {
        DialogOneButton.SetActive(true);
        var backButton = DialogOneButton.transform.Find("Background/Back_Button").GetComponent<Button>();
        backButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Back_Button_Background");
        DialogOneButton.transform.Find("Background/Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Error_Icon_For_Dialog");
        DialogOneButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = message;
        DialogOneButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = title;

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() =>
        {
            DialogOneButton.SetActive(false);
        });
    }

    private void OpenSuccessDialog(DeviceInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");
        var horizontalGroupTransform = backgroundTransform.Find("Horizontal_Group");

        backgroundTransform.Find("Dialog_Status_Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("images/UIimages/Success_Icon_For_Dialog");
        backgroundTransform.Find("Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn đã thành công thêm thiết bị <b><color=#004C8A>{model.Code}</b></color> vào hệ thống";
        backgroundTransform.Find("Dialog_Title").GetComponent<TMP_Text>().text = "Thêm thiết bị mới thành công";

        var confirmButton = horizontalGroupTransform.Find("Confirm_Button").GetComponent<Button>();
        var backButton = horizontalGroupTransform.Find("Back_Button").GetComponent<Button>();

        var confirmButtonText = confirmButton.GetComponentInChildren<TMP_Text>();
        var confirmButtonSprite = confirmButton.GetComponent<Image>().sprite;

        var backButtonText = backButton.GetComponentInChildren<TMP_Text>();


        confirmButtonText.text = "Tiếp tục thêm mới";
        backButtonText.text = "Trở lại danh sách";

        confirmButtonSprite = Resources.Load<Sprite>("images/UIimages/Success_Back_Button_Background");

        confirmButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            ResetAllInputFields();
            DialogTwoButton.SetActive(false);
            basic_ScrollRect.verticalNormalizedPosition = 1;
            technical_ScrollRect.verticalNormalizedPosition = 1;
            operational_ScrollRect.verticalNormalizedPosition = 1;
            RenewView();
        });

        backButton.onClick.AddListener(() =>
        {
            ResetAllInputFields();
            DialogTwoButton.SetActive(false);
            RenewView();
            CloseAddCanvas();
        });
    }

    private void OpenBasicInfo()
    {
        basicInfo_ScrollView.SetActive(true);
        basicInfoButton.image.color = new Color32(226, 226, 226, 255);
        technicalInfo_ScrollView.SetActive(false);
        technicalInfoButton.image.color = new Color32(255, 255, 255, 255);
        operationalInfo_ScrollView.SetActive(false);
        operationalInfoButton.image.color = new Color32(255, 255, 255, 255);
    }
    private void OpenTechnicalInfo()
    {
        basicInfo_ScrollView.SetActive(false);
        basicInfoButton.image.color = new Color32(255, 255, 255, 255);
        technicalInfo_ScrollView.SetActive(true);
        technicalInfoButton.image.color = new Color32(226, 226, 226, 255);
        operationalInfo_ScrollView.SetActive(false);
        operationalInfoButton.image.color = new Color32(255, 255, 255, 255);
    }
    private void OpenOperationalInfo()
    {
        basicInfo_ScrollView.SetActive(false);
        basicInfoButton.image.color = new Color32(255, 255, 255, 255);
        technicalInfo_ScrollView.SetActive(false);
        technicalInfoButton.image.color = new Color32(255, 255, 255, 255);
        operationalInfo_ScrollView.SetActive(true);
        operationalInfoButton.image.color = new Color32(226, 226, 226, 255);
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
        if (GlobalVariable.APIRequestType.Contains("POST_Device"))
        {
            OpenErrorDialog();
        }
    }

    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("POST_Device"))
        {
            Show_Toast.Instance.ShowToast("success", "Thêm thiết bị mới thành công");
            OpenSuccessDialog(DeviceInformationModel);
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False(1f));
    }

    public void DisplayList(List<DeviceInformationModel> models) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
    public void DisplayDetail(DeviceInformationModel model) { }
}
