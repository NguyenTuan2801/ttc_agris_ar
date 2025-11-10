using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using EasyUI.Progress;
using System.Collections;

public class Dropdown_On_ValueChange : MonoBehaviour
{
    public SearchDeviceAndJBView searchableDropDownView;

    public GameObject prefab_Infor;
    public Open_Detail_Image open_Detail_Image;
    // public EventPublisher eventPublisher;
    public GameObject Device_Information_Group;
    public GameObject List_JB_Group;
    public GameObject JBPrefab;

    [SerializeField] private RectTransform contentTransform;
    [SerializeField] private TMP_Text code_Value_Text, function_Value_Text, type_Value_Text, modelSeries_Value_Text, manufacturer_Value_Text, partNumber_Value_Text, serialNumber_Value_Text, manufacturingYear_Value_Text, installationDate_Value_Text, io_Value_Text, measurementType_Value_Text, range_Value_Text, unit_Value_Text, accuracy_Value_Text, supplyVoltage_Value_Text, outputSignal_Value_Text, ingressProtection_Value_Text, connectorType_Value_Text, processConnection_Value_Text, responseTime_Value_Text, otherSpecifications_Value_Text, installationLocation_Value_Text, measuredMedium_Value_Text, operatingTemperature_Value_Text, operatingPressure_Value_Text, calibrationFrequency_Value_Text, failureHistory_Value_Text, environmentCondition_Value_Text;
    [SerializeField] private Image JB_Location_Image_Prefab;
    [SerializeField] private Image JB_Connection_Wiring_Image_Prefab;
    [SerializeField] private GameObject JB_Connection_Group, bottom_App_Bar;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Dictionary<string, Sprite> spriteCache = new();
    [SerializeField] private List<Image> instantiatedImages = new();
    [SerializeField] private Transform deviceInfo;

    [SerializeField] private GameObject searchScrollPanel;

    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button blurButton;

    private Dictionary<string, DeviceInformationModel> deviceDictionary;
    private Dictionary<string, JBInformationModel> jBDictionary;
    private string _jbName = "";

    private void Awake()
    {
        searchableDropDownView ??= GameObject.Find("Searchable").GetComponent<SearchDeviceAndJBView>();
        InitUIElements();

        //searchableDropDownView.OnValueChangedEvt += OnInputValueChanged;
        searchableDropDownView.OnItemSelectedEvt += OnItemSelected;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        if (searchableDropDownView != null)
        {
            //searchableDropDownView.OnValueChangedEvt -= OnInputValueChanged;
            searchableDropDownView.OnItemSelectedEvt -= OnItemSelected;
        }
    }

    private void Start()
    {
        StartCoroutine(LoadData());
        clearButton.onClick.AddListener(ClearSearchInputField);
        //UpdateBlurButton();
    }
    void OnDestroy()
    {
        ResetResources();
        StopAllCoroutines();
    }

    private IEnumerator LoadData()
    {
        yield return new WaitUntil(
            () => GlobalVariable_Search_Devices.temp_ListDeviceInformationModel.Any() &&
            GlobalVariable_Search_Devices.temp_ListJBInformationModel.Any()
        );

        try
        {
            Prepare_Device_Dictionary_For_Searching();
            Prepare_JB_Dictionary_For_Searching();
            searchableDropDownView.Initialize();
            searchableDropDownView.SetInitialTextFieldValue();
            searchScrollPanel.SetActive(false);
            UpdateBlurButton();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.InnerException);
        }
    }

    private void InitUIElements()
    {
        scrollRect ??= prefab_Infor.GetComponent<ScrollRect>();
        var content = prefab_Infor.transform.Find("Content");
        deviceInfo = content.Find("Device_Info");
        code_Value_Text ??= deviceInfo.Find("Code_group/Code_value").GetComponent<TMP_Text>();
        function_Value_Text ??= deviceInfo.Find("Function_group/Function_value").GetComponent<TMP_Text>();
        io_Value_Text ??= deviceInfo.Find("IO_group/IO_value").GetComponent<TMP_Text>();
        type_Value_Text ??= deviceInfo.Find("Type_group/Type_value").GetComponent<TMP_Text>();
        modelSeries_Value_Text ??= deviceInfo.Find("ModelSeries_group/ModelSeries_value").GetComponent<TMP_Text>();
        manufacturer_Value_Text ??= deviceInfo.Find("Manufacturer_group/Manufacturer_value").GetComponent<TMP_Text>();
        partNumber_Value_Text ??= deviceInfo.Find("PartNumber_group/PartNumber_value").GetComponent<TMP_Text>();
        serialNumber_Value_Text ??= deviceInfo.Find("SerialNumber_group/SerialNumber_value").GetComponent<TMP_Text>();
        manufacturingYear_Value_Text ??= deviceInfo.Find("ManufacturingYear_group/ManufacturingYear_value").GetComponent<TMP_Text>();
        installationDate_Value_Text ??= deviceInfo.Find("InstallationDate_group/InstallationDate_value").GetComponent<TMP_Text>();
        measurementType_Value_Text ??= deviceInfo.Find("MeasurementType_group/MeasurementType_value").GetComponent<TMP_Text>();
        range_Value_Text ??= deviceInfo.Find("Range_group/Range_value").GetComponent<TMP_Text>();
        unit_Value_Text ??= deviceInfo.Find("Unit_group/Unit_value").GetComponent<TMP_Text>();
        accuracy_Value_Text ??= deviceInfo.Find("Accuracy_group/Accuracy_value").GetComponent<TMP_Text>();
        supplyVoltage_Value_Text ??= deviceInfo.Find("SupplyVoltage_group/SupplyVoltage_value").GetComponent<TMP_Text>();
        outputSignal_Value_Text ??= deviceInfo.Find("OutputSignal_group/OutputSignal_value").GetComponent<TMP_Text>();
        ingressProtection_Value_Text ??= deviceInfo.Find("IngressProtection_group/IngressProtection_value").GetComponent<TMP_Text>();
        connectorType_Value_Text ??= deviceInfo.Find("ConnectorType_group/ConnectorType_value").GetComponent<TMP_Text>();
        processConnection_Value_Text ??= deviceInfo.Find("ProcessConnection_group/ProcessConnection_value").GetComponent<TMP_Text>();
        responseTime_Value_Text ??= deviceInfo.Find("ResponseTime_group/ResponseTime_value").GetComponent<TMP_Text>();
        otherSpecifications_Value_Text ??= deviceInfo.Find("OtherSpecifications_group/OtherSpecifications_value").GetComponent<TMP_Text>();
        installationLocation_Value_Text ??= deviceInfo.Find("InstallationLocation_group/InstallationLocation_value").GetComponent<TMP_Text>();
        measuredMedium_Value_Text ??= deviceInfo.Find("MeasuredMedium_group/MeasuredMedium_value").GetComponent<TMP_Text>();
        operatingTemperature_Value_Text ??= deviceInfo.Find("OperatingTemperature_group/OperatingTemperature_value").GetComponent<TMP_Text>();
        operatingPressure_Value_Text ??= deviceInfo.Find("OperatingPressure_group/OperatingPressure_value").GetComponent<TMP_Text>();
        calibrationFrequency_Value_Text ??= deviceInfo.Find("CalibrationFrequency_group/CalibrationFrequency_value").GetComponent<TMP_Text>();
        failureHistory_Value_Text ??= deviceInfo.Find("FailureHistory_group/FailureHistory_value").GetComponent<TMP_Text>();
        environmentCondition_Value_Text ??= deviceInfo.Find("EnvironmentCondition_group/EnvironmentCondition_value").GetComponent<TMP_Text>();

        // var jbConnectionGroup = content.Find("JB_Connection_group/JB_Connection_text_group");
        JB_Connection_Group ??= content.Find("JB_Connection_group").gameObject;
        JB_Location_Image_Prefab ??= JB_Connection_Group.transform.Find("image_BackGround/JB_Location_Image").GetComponent<Image>();
        JB_Connection_Wiring_Image_Prefab ??= JB_Connection_Group.transform.Find("JB_Connection_Wiring").GetComponent<Image>();
    }

    private void Prepare_Device_Dictionary_For_Searching()
    {
        if (GlobalVariable_Search_Devices.selectedDeviceType == "JB")
        {
            deviceDictionary = new Dictionary<string, DeviceInformationModel>(); // Không tải thiết bị
            return;
        }

        var tempListDevice = GlobalVariable_Search_Devices.temp_ListDeviceInformationModel
            .Where(device => device.Type == GlobalVariable_Search_Devices.selectedDeviceType)
            .ToList();
        //deviceDictionary = tempListDevice
        //    .GroupBy(device => device.Code.ToLower())
        //    .ToDictionary(g => g.Key, g => g.First());

        //foreach (var device in tempListDevice)
        //{
        //    var functionKey = device.Function.ToLower();
        //    if (!deviceDictionary.ContainsKey(functionKey))
        //    {
        //        deviceDictionary.Add(functionKey, device);
        //    }
        //}
        deviceDictionary = tempListDevice
        .ToDictionary(device => ($"{device.Code} - {device.Function}").ToLower(),
                      device => device);
    }

    private void Prepare_JB_Dictionary_For_Searching()
    {
        var tempListJB = GlobalVariable_Search_Devices.temp_ListJBInformationModel;

        jBDictionary = tempListJB
            .GroupBy(jb => jb.Name.ToLower())
            .ToDictionary(g => g.Key, g => g.First());
    }

    //private void OnInputValueChanged(string input)
    //{
    //    ClearWiringGroupAndCache();        
    //    //if (List_JB_Group.activeSelf)
    //    //{
    //    //    List_JB_Group.SetActive(false);
    //    //}
    //    //if (Device_Information_Group.activeSelf)
    //    //{
    //    //    Device_Information_Group.SetActive(false);
    //    //}

    //    switch (searchableDropDownView.filter_Type)
    //    {
    //        case "Device":
    //            if (deviceDictionary.TryGetValue(input.ToLower(), out var device))
    //            {
    //                List_JB_Group.SetActive(true);
    //                Device_Information_Group.SetActive(true);
    //                UpdateDeviceInformation(device);
    //            }
    //            break;
    //        case "JB/TSD":
    //            Device_Information_Group.SetActive(false);

    //            List_JB_Group.SetActive(true);
    //            JBPrefab.SetActive(true);
    //            if (jBDictionary.TryGetValue(input.ToLower(), out var jB))
    //            {
    //                UpdateJBInformation(jB);
    //            }
    //            break;
    //    }
    //}

    private void ClearWiringGroupAndCache()
    {
        foreach (Transform child in List_JB_Group.transform)
        {
            if (child.gameObject != JB_Connection_Group && child.gameObject.name.Contains("(Clone)"))
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in JB_Connection_Group.transform)
        {
            if (child.gameObject != JB_Connection_Wiring_Image_Prefab && child.gameObject.name.Contains("(Clone)"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private async void UpdateDeviceInformation(DeviceInformationModel device)
    {
        if (!deviceInfo.gameObject.activeSelf) deviceInfo.gameObject.SetActive(true);
        code_Value_Text.text = device.Code;
        function_Value_Text.text = device.Function;
        io_Value_Text.text = device.IOAddress;
        type_Value_Text.text = device.Type;
        modelSeries_Value_Text.text = device.ModelSeries;
        manufacturer_Value_Text.text = device.Manufacturer;
        partNumber_Value_Text.text = device.PartNumber;
        serialNumber_Value_Text.text = device.SerialNumber;
        manufacturingYear_Value_Text.text = device.ManufacturingYear;
        installationDate_Value_Text.text = device.InstallationDate;
        measurementType_Value_Text.text = device.MeasurementType;
        range_Value_Text.text = device.Range;
        unit_Value_Text.text = device.Unit;
        accuracy_Value_Text.text = device.Accuracy;
        supplyVoltage_Value_Text.text = device.SupplyVoltage;
        outputSignal_Value_Text.text = device.OutputSignal;
        ingressProtection_Value_Text.text = device.IngressProtection;
        connectorType_Value_Text.text = device.ConnectorType;
        processConnection_Value_Text.text = device.ProcessConnection;
        responseTime_Value_Text.text = device.ResponseTime;
        otherSpecifications_Value_Text.text = device.OtherSpecifications;
        installationLocation_Value_Text.text = device.InstallationLocation;
        measuredMedium_Value_Text.text = device.MeasuredMedium;
        operatingTemperature_Value_Text.text = device.OperatingTemperature;
        operatingPressure_Value_Text.text = device.OperatingPressure;
        calibrationFrequency_Value_Text.text = device.CalibrationFrequency;
        failureHistory_Value_Text.text = device.FailureHistory;
        environmentCondition_Value_Text.text = device.EnvironmentCondition;

        if (device.JBInformationModels.Any())
        {
            ShowProgressBar("Đang tải dữ liệu...");

            if (device.JBInformationModels.Count == 1)
            {
                if (!JBPrefab.activeSelf) JBPrefab.SetActive(true);
                var JBName = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_value").GetComponent<TMP_Text>();
                var JBLocation = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_location").GetComponent<TMP_Text>();
                _jbName = device.JBInformationModels[0].Name;
                var jbInformationModel = GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.TryGetValue(_jbName, out var jbInfo) ? jbInfo : null;
                JBName.text = jbInformationModel.Name;
                JBLocation.text = jbInformationModel.Location;
                Debug.Log($"JB Name: {_jbName}, JB Location: {jbInformationModel.Location}");
                if (!string.IsNullOrEmpty(_jbName))
                {
                    Debug.Log("Run LoadDeviceSprites");
                    await LoadForDevice(
                        jbInfo: jbInformationModel,
                        additionalImages: device.AdditionalConnectionImages,
                        locationImage: JB_Location_Image_Prefab,
                        connectionImagePrefab: JB_Connection_Wiring_Image_Prefab,
                        parentGroup: JB_Connection_Group.transform
                    );
                    // Canvas.ForceUpdateCanvases();
                    // LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);                  
                }               
            }
            else
            {
                if (!JBPrefab.activeSelf) JBPrefab.SetActive(true);
                foreach (var jb in device.JBInformationModels)
                {
                    int jbIndex = device.JBInformationModels.IndexOf(jb);
                    var newJB = Instantiate(JBPrefab, List_JB_Group.transform);
                    var JBName = newJB.transform.Find("JB_Connection_text_group/JB_Connection_value").GetComponent<TMP_Text>();
                    var JBLocation = newJB.transform.Find("JB_Connection_text_group/JB_Connection_location").GetComponent<TMP_Text>();
                    _jbName = jb.Name;
                    var jbInformationModel = GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.TryGetValue(_jbName, out var jbInfo) ? jbInfo : null;
                    JBName.text = jbInformationModel.Name;
                    JBLocation.text = jbInformationModel.Location;
                    Debug.Log($"JB Name: {_jbName}, JB Location: {jbInformationModel.Location}");
                    if (!string.IsNullOrEmpty(_jbName))
                    {
                        Image locImg = newJB.transform.Find("image_BackGround/JB_Location_Image").GetComponent<Image>();
                        Image connPrefab = newJB.transform.Find("JB_Connection_Wiring").GetComponent<Image>();

                        await LoadForDevice(
                            jbInfo: jbInformationModel,
                            additionalImages: device.AdditionalConnectionImages,
                            locationImage: locImg,
                            connectionImagePrefab: connPrefab,
                            parentGroup: newJB.transform
                        );
                    }
                }
                // Canvas.ForceUpdateCanvases();
                // LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);
                JBPrefab.SetActive(true);                
            }
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(JB_Connection_Group.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(List_JB_Group.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(prefab_Infor.transform as RectTransform);
            //prefab_Infor.transform.Find("Content").gameObject.SetActive(false);
            //await Task.Delay(1000);
            //// prefab_Infor.SetActive(true);
            //// await Task.Delay(300);
            //// prefab_Infor.SetActive(false);
            //// await Task.Delay(300);
            //prefab_Infor.transform.Find("Content").gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(prefab_Infor.transform as RectTransform);

            HideProgressBar();

        }
        else
        {
            List_JB_Group.SetActive(false);
        }

        Debug.Log($"Device info panel active: {Device_Information_Group.activeSelf}");

    }

    private async void UpdateJBInformation(JBInformationModel jB)
    {
        ShowProgressBar("Đang tải dữ liệu...");
        if (deviceInfo.gameObject.activeSelf) deviceInfo.gameObject.SetActive(false);
        var JBName = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_value").GetComponent<TMP_Text>();
        var JBLocation = JBPrefab.transform.Find("JB_Connection_text_group/JB_Connection_location").GetComponent<TMP_Text>();
        _jbName = jB.Name;
        GlobalVariable_Search_Devices.jbName = _jbName;
        var jbInformationModel = GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.TryGetValue(_jbName, out var jbInfo) ? jbInfo : null;
        JBName.text = $"{jbInformationModel.Name}:";
        JBLocation.text = jbInformationModel.Location;
        Debug.Log($"JB Name: {_jbName}, JB Location: {jbInformationModel.Location}");
        if (!string.IsNullOrEmpty(_jbName))
        {
            await LoadForJB(
            jbInfo: jbInformationModel,
            locationImage: JB_Location_Image_Prefab,
            connectionImagePrefab: JB_Connection_Wiring_Image_Prefab,
            parentGroup: JB_Connection_Group.transform
        );
        }
        // Canvas.ForceUpdateCanvases();
        // LayoutRebuilder.ForceRebuildLayoutImmediate(List_JB_Group.transform as RectTransform);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(JB_Connection_Group.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(List_JB_Group.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(prefab_Infor.transform as RectTransform);
        //prefab_Infor.transform.Find("Content").gameObject.SetActive(false);
        //await Task.Delay(1000);
        //// prefab_Infor.transform.Find("Content").gameObject.SetActive(true);
        //// await Task.Delay(300);
        //// prefab_Infor.transform.Find("Content").gameObject.SetActive(false);
        //// await Task.Delay(300);
        //prefab_Infor.transform.Find("Content").gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(prefab_Infor.transform as RectTransform);

        HideProgressBar();

        Debug.Log($"JB info panel active: {List_JB_Group.activeSelf}");

    }

    private async Task LoadDeviceSprites(List<ImageInformationModel> list_Additional_Images, JBInformationModel jbInformationModel, Image LocationImage, Image ConnectionImage, Transform JB_List_Connection_Group)
    {
        if (jbInformationModel == null || jbInformationModel.ListConnectionImages == null)
        {
            Debug.LogError("jbInformationModel hoặc ListConnectionImages bị null!");
            return;
        }

        var total_List_Connection_Images = new List<ImageInformationModel>(jbInformationModel.ListConnectionImages);

        if (list_Additional_Images != null && list_Additional_Images.Count > 0)
        {
            total_List_Connection_Images.AddRange(list_Additional_Images);
        }
        Debug.Log("Run LoadDeviceSprites: ApplySpriteJBImages");
        await ApplySpriteJBImages(
          outdoorImage: jbInformationModel.OutdoorImage,
          listConnectionImages: total_List_Connection_Images,
          JB_Location_Image: LocationImage,
          JB_Connection_Wiring_Image: ConnectionImage,
          JB_List_Connection_Group: JB_List_Connection_Group);
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
        else
        {
            Debug.LogWarning("scrollRect bị null, không thể thay đổi vị trí roll!");
        }
    }

    private async Task ApplySpriteJBImages(ImageInformationModel outdoorImage, List<ImageInformationModel> listConnectionImages, Image JB_Location_Image, Image JB_Connection_Wiring_Image, Transform JB_List_Connection_Group)
    {
        var tasks = new List<Task>();

        if (outdoorImage != null)
        {
            if (!string.IsNullOrEmpty(outdoorImage.Name))
            {
                tasks.Add(searchableDropDownView._presenter.LoadImageAsync(outdoorImage.Name, JB_Location_Image));
                var buttonComponent = JB_Location_Image.gameObject.GetComponent<Button>();
                AddButtonListener(buttonComponent, () => open_Detail_Image.Open_Detail_Canvas(JB_Location_Image));
            }
        }
        else
        {   //! Được ghi chú trên sơ đồ
            var buttonComponent = JB_Location_Image.gameObject.GetComponent<Button>();
            AddButtonListener(buttonComponent, () => open_Detail_Image.Open_Detail_Canvas(JB_Location_Image));
            //! Nhớ đổi thành tên ảnh thay vì Url
            tasks.Add(searchableDropDownView._presenter.LoadImageAsync("JB_Location_Noted.png", JB_Location_Image));

        }

        if (listConnectionImages.Any())
        {
            foreach (var image in listConnectionImages)
            {
                var newImageObject = Instantiate(JB_Connection_Wiring_Image.gameObject, JB_List_Connection_Group);
                var imageComponent = newImageObject.GetComponent<Image>();
                var buttonComponent = newImageObject.GetComponent<Button>();

                AddButtonListener(buttonComponent, () => open_Detail_Image.Open_Detail_Canvas(imageComponent));
                tasks.Add(searchableDropDownView._presenter.LoadImageAsync(image.Name, newImageObject.GetComponent<Image>()));

            }
        }
        await Task.WhenAll(tasks);

        JB_Connection_Wiring_Image.gameObject.SetActive(false);

        ResizeImages(JB_Location_Image, JB_List_Connection_Group);
    }

    private void AddButtonListener(Button button, Action onClickAction)
    {

        if (button == null)
        {
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            onClickAction?.Invoke();
        });
    }

    private void ShowProgressBar(string title)
    {
        Progress.Show(title, ProgressColor.Blue, true);
    }

    private void HideProgressBar()
    {
        Progress.Hide();
    }

    private void ResizeImages(Image locationImage, Transform JB_List_Connection_Group)
    {
        StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(locationImage, forceUpdate: false));

        foreach (var connectionImage in JB_List_Connection_Group.GetComponentsInChildren<Image>())
        {
            if (connectionImage.gameObject.activeSelf && (connectionImage.name.Contains("(Clone)") || connectionImage.name.Contains("Location")))
                StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(connectionImage, forceUpdate: false));
        }
    }

    // private void SetSprite(Image imageComponent, string jb_name)
    // {
    //     if (!spriteCache.TryGetValue(jb_name, out var jbSprite))
    //     {
    //         spriteCache.TryGetValue("JB_TSD_Location_Note", out jbSprite);
    //     }
    //     imageComponent.sprite = jbSprite;
    //     imageComponent.gameObject.GetComponent<Button>().onClick.AddListener(() => open_Detail_Image.Open_Detail_Canvas(imageComponent));
    //     StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(imageComponent));
    // }

    // private void CreateAndSetSprite(string jb_name)
    // {
    //     var jbLocationImage = Instantiate(JB_Location_Image_Prefab, JB_Connection_Group.transform);
    //     jbLocationImage.transform.SetSiblingIndex(JB_Location_Image_Prefab.transform.GetSiblingIndex() + 1);
    //     jbLocationImage.gameObject.SetActive(true);
    //     SetSprite(jbLocationImage, jb_name);
    //     instantiatedImages.Add(jbLocationImage);
    // }

    // private void ClearInstantiatedImages()
    // {
    //     foreach (var img in instantiatedImages)
    //     {
    //         if (img != null)
    //         {
    //             Destroy(img.gameObject);
    //         }
    //     }
    //     instantiatedImages.Clear();
    // }

    private void ResetResources()
    {
        ClearWiringGroupAndCache();
        GlobalVariable_Search_Devices.temp_ListDeviceInformationModel.Clear();
        GlobalVariable_Search_Devices.temp_Dictionary_JBInformationModel.Clear();
        GlobalVariable_Search_Devices.temp_ListJBInformationModel.Clear();
        GlobalVariable_Search_Devices.temp_List_Device_For_Fitler.Clear();
        GlobalVariable_Search_Devices.temp_List_JB_For_Fitler.Clear();
    }

    private void HandleOrientationChange(ScreenOrientation newOrientation)
    {
        // if (newOrientation == ScreenOrientation.Portrait)
        // {
        //     bottom_App_Bar.SetActive(true);
        // }
        // else if (newOrientation == ScreenOrientation.LandscapeLeft || newOrientation == LandscapeRight)
        // {
        //     bottom_App_Bar.SetActive(false);
        // }
    }

    private void ClearSearchInputField()
    {
        searchInputField.text = string.Empty;
    }

    private void CloseSearchPanel()
    {
        searchScrollPanel.SetActive(false);
        UpdateBlurButton();
    }

    public void UpdateBlurButton()
    {
        bool shouldShow = searchScrollPanel.activeSelf;
        blurButton.gameObject.SetActive(shouldShow);

        if (shouldShow)
        {
            blurButton.onClick.RemoveAllListeners();
            blurButton.onClick.AddListener(CloseSearchPanel);
        }
        else
        {
            blurButton.onClick.RemoveAllListeners();
        }
    }

    private async Task LoadForDevice(JBInformationModel jbInfo, List<ImageInformationModel> additionalImages, Image locationImage, Image connectionImagePrefab, Transform parentGroup)
    {
        if (jbInfo == null) return;

        var tasks = new List<Task>();

        // Load Outdoor Image
        if (!string.IsNullOrEmpty(jbInfo.OutdoorImage?.Name))
        {
            tasks.Add(searchableDropDownView._presenter.LoadImageAsync(jbInfo.OutdoorImage.Name, locationImage));
        }
        else
        {
            tasks.Add(searchableDropDownView._presenter.LoadImageAsync("JB_Location_Noted.png", locationImage));
        }
        AddButtonListener(locationImage.GetComponent<Button>(), () => open_Detail_Image.Open_Detail_Canvas(locationImage));

        // Load TẤT CẢ AdditionalConnectionImages
        if (additionalImages != null && additionalImages.Count > 0)
        {
            foreach (var img in additionalImages)
            {
                var clone = Instantiate(connectionImagePrefab.gameObject, parentGroup);
                var imgComp = clone.GetComponent<Image>();
                clone.SetActive(true);
                tasks.Add(searchableDropDownView._presenter.LoadImageAsync(img.Name, imgComp));
                AddButtonListener(clone.GetComponent<Button>(), () => open_Detail_Image.Open_Detail_Canvas(imgComp));
            }
        }

        connectionImagePrefab.gameObject.SetActive(false);
        await Task.WhenAll(tasks);

        ResizeImages(locationImage, parentGroup);
        ScrollToTop();
    }

    private async Task LoadForJB(JBInformationModel jbInfo, Image locationImage, Image connectionImagePrefab, Transform parentGroup)
    {
        if (jbInfo == null) return;

        var tasks = new List<Task>();

        // Load Outdoor Image
        if (!string.IsNullOrEmpty(jbInfo.OutdoorImage?.Name))
        {
            tasks.Add(searchableDropDownView._presenter.LoadImageAsync(jbInfo.OutdoorImage.Name, locationImage));
        }
        else
        {
            tasks.Add(searchableDropDownView._presenter.LoadImageAsync("JB_Location_Noted.png", locationImage));
        }
        AddButtonListener(locationImage.GetComponent<Button>(), () => open_Detail_Image.Open_Detail_Canvas(locationImage));

        // Load TẤT CẢ ListConnectionImages
        if (jbInfo.ListConnectionImages != null && jbInfo.ListConnectionImages.Any())
        {
            foreach (var img in jbInfo.ListConnectionImages)
            {
                var clone = Instantiate(connectionImagePrefab.gameObject, parentGroup);
                var imgComp = clone.GetComponent<Image>();
                clone.SetActive(true);
                tasks.Add(searchableDropDownView._presenter.LoadImageAsync(img.Name, imgComp));
                AddButtonListener(clone.GetComponent<Button>(), () => open_Detail_Image.Open_Detail_Canvas(imgComp));
            }
        }

        connectionImagePrefab.gameObject.SetActive(false);
        await Task.WhenAll(tasks);

        ResizeImages(locationImage, parentGroup);
        ScrollToTop();
    }

    private void ScrollToTop()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void OnItemSelected(string selectedKey)
    {
        ClearWiringGroupAndCache(); // Chỉ xóa khi thực sự chọn item mới

        switch (searchableDropDownView.filter_Type)
        {
            case "Device":
                if (deviceDictionary.TryGetValue(selectedKey, out var device))
                {
                    List_JB_Group.SetActive(true);
                    Device_Information_Group.SetActive(true);
                    UpdateDeviceInformation(device);
                }
                break;

            case "JB/TSD":
                Device_Information_Group.SetActive(false);
                List_JB_Group.SetActive(true);
                JBPrefab.SetActive(true);
                if (jBDictionary.TryGetValue(selectedKey, out var jB))
                {
                    UpdateJBInformation(jB);
                }
                break;
        }
    }
}