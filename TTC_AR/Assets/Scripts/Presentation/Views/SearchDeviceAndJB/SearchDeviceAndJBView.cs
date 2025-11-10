using System;
using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SearchDeviceAndJBView : MonoBehaviour, ISearchDeviceAndJBView
{
    [Header("Filter")]
    public GameObject filterDropdownButton;
    public GameObject filterContent;
    public TMP_Text filterText;
    public string filter_Type = "Device";
    [SerializeField] private bool isFilterContentActive = false;

    public GameObject arrowButtonUp;
    public GameObject arrowButtonDown;
    public GameObject itemPrefab;
    public TMP_InputField inputField;
    public ScrollRect scrollRect;
    public GameObject contentItemSelection;
    public event Action<string> OnValueChangedEvt;
    public event Action<string> OnItemSelectedEvt;
    [SerializeField] private RectTransform contentItemSelectionRect;
    [SerializeField] private List<GameObject> itemGameObjects = new List<GameObject>();
    [SerializeField] private List<DeviceInformationModel> tempDeviceInfo = new List<DeviceInformationModel>();
    [SerializeField] private List<JBInformationModel> tempJBInfo = new List<JBInformationModel>();
    [SerializeField] private List<string> availableOptions = new List<string>();
    [SerializeField] private List<string> deviceOptions = new List<string>();

    [SerializeField] private List<string> jbOptions = new List<string>();

    private Vector2 scrollRectInitialSize;
    public SearchDeviceAndJBPresenter _presenter;
    private int grapperId;

    void Awake()
    {
        if (ManagerLocator.Instance.JBManager != null && ManagerLocator.Instance.DeviceManager != null)
        {
            _presenter = new SearchDeviceAndJBPresenter(this,
                     ManagerLocator.Instance.JBManager._IJBService,
                     ManagerLocator.Instance.DeviceManager._IDeviceService);
        }
    }

    private void OnEnable()
    {
        grapperId = GlobalVariable.GrapperId;
        StopAllCoroutines();
        LoadData();
    }
    private void OnDisable()
    {
        inputField.onValueChanged.RemoveAllListeners();
        filterDropdownButton.GetComponent<Button>().onClick.RemoveListener(ToggleDropdown);
    }
    public void SetInit()
    {
        contentItemSelectionRect ??= contentItemSelection.GetComponent<RectTransform>();
        scrollRectInitialSize = scrollRect.gameObject.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("SearchableDropDown awake");
        filter_Type = GlobalVariable_Search_Devices.selectedDeviceType == "JB" ? "JB/TSD" : "Device";
        //filter_Type = "Device";
    }

    private void LoadData()
    {
        _presenter.LoadDataForSearching(grapperId);
    }

    private void Start()
    {
        //Invoke(nameof(SetInitialTextFieldValue), 2f);
    }

    public void SetInitialTextFieldValue()
    {
        string initialText = "";
        string keyToSelect = "";

        if (GlobalVariable_Search_Devices.selectedDeviceType == "JB")
        {
            if (tempJBInfo.Any() && !string.IsNullOrEmpty(tempJBInfo[0].Name))
            {
                initialText = tempJBInfo[0].Name;
                keyToSelect = initialText.ToLower(); 
            }
            else
            {
                Debug.Log("Debug Log: tempJBInfo[0].Name is null or empty");
                return;
            }
        }
        else
        {
            if (tempDeviceInfo.Any() &&
                !string.IsNullOrEmpty(tempDeviceInfo[0].Code) &&
                !string.IsNullOrEmpty(tempDeviceInfo[0].Function))
            {
                initialText = $"{tempDeviceInfo[0].Code} - {tempDeviceInfo[0].Function}";
                keyToSelect = initialText.ToLower(); 
            }
            else
            {
                Debug.Log("Debug Log: tempDeviceInfo[0].Code or Function is null or empty");
                return;
            }
        }

        inputField.text = initialText;

        OnItemSelectedEvt?.Invoke(keyToSelect);
    }
    public void Initialize()
    {
        if (GlobalVariable_Search_Devices.selectedDeviceType == "JB")
        {
            filter_Type = "JB/TSD";
            tempDeviceInfo = new List<DeviceInformationModel>();
            tempJBInfo = GlobalVariable_Search_Devices.temp_ListJBInformationModel;
            deviceOptions = new List<string>();
            jbOptions = tempJBInfo.Select(jb => jb.Name).ToList();
            availableOptions = jbOptions;
        }
        else
        {
            filter_Type = "Device";
            tempDeviceInfo = GlobalVariable_Search_Devices.temp_ListDeviceInformationModel
            .Where(device => device.Type == GlobalVariable_Search_Devices.selectedDeviceType)
            .ToList();

            tempJBInfo = GlobalVariable_Search_Devices.temp_ListJBInformationModel;

            // SỬA ĐÂY: Ghép Code và Function thành một chuỗi duy nhất theo định dạng "{Code} - {Function}"
            deviceOptions = tempDeviceInfo.Select(device => $"{device.Code} - {device.Function}").ToList();

            jbOptions = tempJBInfo.Select(jb => jb.Name).ToList();

            availableOptions = deviceOptions;
        }

        if (scrollRect == null || inputField == null || contentItemSelection == null || itemPrefab == null)
        {
            Debug.LogError("Cannot find necessary components for SearchableDropDown");
            return;
        }
        else
        {
            if (availableOptions.Count == 0)
            {
                Debug.Log("availableOptions.Count: " + availableOptions.Count);
                return;
            }
            else
            {
                Debug.Log(availableOptions.Count);

                PopulateDropdown(availableOptions);
                UpdateUI();
                //int onValueChangedListenerCount = inputField.onValueChanged.GetPersistentEventCount();

                inputField.onValueChanged.AddListener(OnInputValueChange);

                filterDropdownButton.GetComponent<Button>().onClick.AddListener(ToggleDropdown);
            }

        }
    }

    public void OnDeviceFilterClicked()
    {
        filter_Type = "Device";
        GlobalVariable_Search_Devices.selectedDeviceType = "Cảm biến nhiệt độ";
        Initialize();
        filterText.text = filter_Type;
        // SỬA ĐÂY: Để trống hoặc chọn chuỗi ghép đầu tiên nếu cần (theo yêu cầu trước, nên để trống)
        inputField.text = deviceOptions.Any() ? deviceOptions[0] : ""; 
        Canvas.ForceUpdateCanvases();

        //filter_Type = "Device";
        //UpdateDropdownOptions(deviceOptions);
        //ToggleDropdown();
        //filterText.text = filter_Type;
        //inputField.text = deviceOptions[0];
        //Canvas.ForceUpdateCanvases();
    }

    public void OnJBFilterClicked()
    {
        filter_Type = "JB/TSD";
        GlobalVariable_Search_Devices.selectedDeviceType = "JB";
        Initialize();
        filterText.text = filter_Type;
        inputField.text = jbOptions.Any() ? jbOptions[0] : "";
        Canvas.ForceUpdateCanvases();

        //filter_Type = "JB/TSD";
        //UpdateDropdownOptions(jbOptions);
        //ToggleDropdown();
        //filterText.text = filter_Type;
        //inputField.text = jbOptions[0];
        //Canvas.ForceUpdateCanvases();
    }

    private void UpdateDropdownOptions(List<string> options)
    {
        availableOptions = options;
        if (scrollRect == null || inputField == null || contentItemSelection == null || itemPrefab == null)
        {
            Debug.LogError("Cannot find necessary components for SearchableDropDown");
            return;
        }

        inputField.onValueChanged.RemoveListener(OnInputValueChange);
        PopulateDropdown(availableOptions);
        UpdateUI();

        inputField.onValueChanged.AddListener(OnInputValueChange);

    }

    private void ClearChildren(Transform parentObjectTransform)
    {
        if (parentObjectTransform == null)
        {
            Debug.LogError("parentObjectTransform is null");
            return;
        }
        else
        {
            foreach (Transform child in parentObjectTransform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // private void PopulateDropdown(List<string> options)
    // {
    //     ClearChildren(content.transform);
    //     itemGameObjects.Clear();
    //     foreach (var option in options)
    //     {
    //         var itemObject = Instantiate(itemPrefab, content.transform);
    //         itemObject.name = option;
    //         var textComponent = itemObject.GetComponentInChildren<TMP_Text>();
    //         textComponent.text = option;
    //         itemObject.GetComponent<Button>().onClick.AddListener(() => OnItemSelected(option));
    //         itemGameObjects.Add(itemObject);
    //     }
    //     Debug.Log("itemGameObjects.Count: " + itemGameObjects.Count);
    // }

    private void PopulateDropdown(List<string> options)
    {
        ClearChildren(contentItemSelectionRect);

        itemGameObjects.Clear();

        int optionsCount = options.Count;

        foreach (var option in options)
        {
            int i = options.IndexOf(option);

            var itemObject = Instantiate(itemPrefab, contentItemSelectionRect);

            //    itemObject.name = option;

            var textComponent = itemObject.GetComponentInChildren<TMP_Text>();
            var buttonComponent = itemObject.GetComponent<Button>();

            textComponent.text = option;
            buttonComponent.onClick.AddListener(() => OnItemSelected(option));

            itemGameObjects.Add(itemObject);

            if (i < optionsCount)
            {
                // var optionText = availableOptions[i];
                // var textValue = itemGameObjects[i].GetComponentInChildren<TMP_Text>();
                //textValue.text = optionText;
                // itemGameObjects[i].name = optionText;
                itemGameObjects[i].SetActive(true);
            }
            else
            {
                itemGameObjects[i].SetActive(false);
            }
        }
        Debug.Log("itemGameObjects.Count: " + itemGameObjects.Count);
    }
    private void UpdateUI()
    {
        int optionsCount = availableOptions.Count;
        for (int i = 0; i < itemGameObjects.Count; i++)
        {
            var item = itemGameObjects[i];
            if (i < optionsCount)
            {
                Debug.Log("item Index: " + i);
                var optionText = availableOptions[i];
                var textComponent = item.GetComponentInChildren<TMP_Text>();
                textComponent.text = optionText;
                item.name = optionText; // SỬA ĐÂY: item.name cũng là chuỗi ghép để lọc đúng
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    private void ToggleDropdown()
    {
        isFilterContentActive = !isFilterContentActive;
        filterContent.gameObject.SetActive(isFilterContentActive);
        arrowButtonDown.SetActive(isFilterContentActive);
        arrowButtonUp.SetActive(!isFilterContentActive);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void SetContentActive(bool isActive)
    {
        arrowButtonDown.SetActive(isActive);
        arrowButtonUp.SetActive(!isActive);
    }

    private void OnInputValueChange(string input)
    {
        FilterDropdown(input.ToLower());
    }

    private void FilterDropdown(string input)
    {
        if (!scrollRect.gameObject.activeSelf)
        {
            scrollRect.gameObject.SetActive(true);
            FindObjectOfType<Dropdown_On_ValueChange>()?.UpdateBlurButton();
        }
            
        if (!string.IsNullOrEmpty(input))
        {
            foreach (var item in itemGameObjects)
            {
                bool shouldActivate = item.name.ToLower().Contains(input);
                if (item.activeSelf != shouldActivate)
                {
                    item.SetActive(shouldActivate);
                }
            }
        }
        else
        {
            foreach (var item in itemGameObjects)
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                }
            }
        }
        ResizeContent();
    }

    private void ResizeContent()
    {
        scrollRect.gameObject.GetComponent<RectTransform>().sizeDelta = scrollRectInitialSize;
        int activeItemCount = itemGameObjects.Count(item => item.activeSelf);
        if (activeItemCount > 0)
        {
            Debug.Log("activeItemCount: " + activeItemCount);
            RectTransform itemRect = itemGameObjects.FirstOrDefault(item => item.activeSelf).gameObject.GetComponent<RectTransform>();
            float newHeight = itemRect.sizeDelta.y * activeItemCount * 1.05f;
            Debug.Log("newHeight: " + newHeight);
            contentItemSelectionRect.sizeDelta = new Vector2(contentItemSelectionRect.sizeDelta.x, newHeight);
            scrollRect.gameObject.GetComponent<RectTransform>().sizeDelta = (activeItemCount == 1)
                ? new Vector2(scrollRectInitialSize.x, newHeight * 1.05f)
                : scrollRectInitialSize;
        }
        else
        {
            contentItemSelectionRect.sizeDelta = new Vector2(contentItemSelectionRect.sizeDelta.x, 0);
            scrollRect.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
        //  Canvas.ForceUpdateCanvases();

    }

    private void OnItemSelected(string selectedItem)
    {
        inputField.text = selectedItem;
        OnValueChangedEvt?.Invoke(selectedItem);        
        OnItemSelectedEvt?.Invoke(selectedItem.ToLower());
        scrollRect.gameObject.SetActive(false);

        FindObjectOfType<Dropdown_On_ValueChange>()?.UpdateBlurButton();

        arrowButtonDown.SetActive(false);
        arrowButtonUp.SetActive(true);
    }

    public void ResetDropdown()
    {
        inputField.text = string.Empty;
        ResizeContent();
    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(OnInputValueChange);
        filterDropdownButton.GetComponent<Button>().onClick.RemoveListener(ToggleDropdown);
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
        Debug.LogError(message);
    }

    public void ShowSuccess()
    {
        Debug.Log("Success");
    }

    public void DisplayJBInfor(JBInformationModel model)
    {
        throw new NotImplementedException();
    }

    public void DisplayDevice(DeviceInformationModel model)
    {
        throw new NotImplementedException();
    }

    public void DisplayCreateResult(bool success)
    {
        throw new NotImplementedException();
    }
}