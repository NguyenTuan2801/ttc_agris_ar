using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListRackSettingView : MonoBehaviour, IRackView
{
    [Header("Canvas")]
    public GameObject List_Rack_Canvas;
    public GameObject Add_New_Rack_Canvas;
    public GameObject Update_Rack_Canvas;

    public GameObject Rack_Item_Prefab;
    public GameObject Parent_Vertical_Layout_Group;
    public ScrollRect scrollView;
    private List<GameObject> listRackItems = new List<GameObject>();

    [Header("Search")]
    public TMP_InputField searchInputField; // Thanh tìm kiếm
    public Button clearButton; // Nút xóa nội dung tìm kiếm
    private List<RackInformationModel> allRacks = new List<RackInformationModel>(); // Lưu dữ liệu gốc

    public GameObject DialogOneButton;
    public GameObject DialogTwoButton;
    private RackPresenter _presenter;
    private Sprite warningConfirmButtonSprite;
    private int grapperId;
    private GameObject _rackItem;

    void Awake()
    {
        _presenter = new RackPresenter(this,
        ManagerLocator.Instance.RackManager._IRackService);

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
        LoadListRack();
    }
    void OnDisable()
    {
    }

    private void RefreshList()
    {
        Rack_Item_Prefab.SetActive(true);
        foreach (var item in listRackItems)
        {
            if (item != Rack_Item_Prefab)
                Destroy(item);
        }
        listRackItems.Clear();
    }

    public void LoadListRack()
    {
        RefreshList();
        _presenter.LoadListRack(grapperId);
    }
    public void DisplayList(List<RackInformationModel> models)
    {
        // Lưu dữ liệu gốc để sử dụng cho tìm kiếm
        allRacks = models ?? new List<RackInformationModel>();
        RefreshList();

        if (allRacks.Any())
        {
            CreateRackItems(allRacks);            
        }
        else
        {
            Debug.Log("No Racks found");
        }
        Rack_Item_Prefab.SetActive(false);
        scrollView.verticalNormalizedPosition = 1f; // Scroll to the top

        // Đăng ký sự kiện tìm kiếm
        if (searchInputField != null)
        {
            searchInputField.onValueChanged.RemoveAllListeners();
            searchInputField.onValueChanged.AddListener(OnSearchValueChanged);
        }
    }

    private void EditRackItem(int id)
    {
        GlobalVariable.rackId = id;
        OpenUpdateCanvas();
    }
    private void DeleRackItem(GameObject RackItem, RackInformationModel model)
    {
        OpenDeleteWarningDialog(RackItem, model);
    }

    public void OpenAddNewCanvas()
    {
        Add_New_Rack_Canvas.SetActive(true);
        List_Rack_Canvas.SetActive(false);
        Update_Rack_Canvas.SetActive(false);
    }
    private void OpenUpdateCanvas()
    {
        List_Rack_Canvas.SetActive(false);
        Add_New_Rack_Canvas.SetActive(false);
        Update_Rack_Canvas.SetActive(true);
    }

    private void OpenDeleteWarningDialog(GameObject RackItem, RackInformationModel model)
    {
        DialogTwoButton.SetActive(true);

        var backgroundTransform = DialogTwoButton.transform.Find("Background");

        var Horizontal_Group = DialogTwoButton.transform.Find("Background/Horizontal_Group").gameObject.transform;

        var dialog_Content = DialogTwoButton.transform.Find("Background/Dialog_Content").GetComponent<TMP_Text>().text = $"Bạn có chắc chắn muốn xóa thông tin Rack IO <color=#ED1C24><b>{model.Name}</b></color> khỏi hệ thống? Hãy kiểm tra kĩ trước khi nhấn nút xác nhận phía dưới";

        var dialog_Title = DialogTwoButton.transform.Find("Background/Dialog_Title").GetComponent<TMP_Text>().text = "Xóa Rack IO khỏi hệ thống?";

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
            _rackItem = RackItem;
            _presenter.DeleteRack(model.Id);
            DialogTwoButton.SetActive(false);
        });
        backButton.onClick.AddListener(() =>
        {
            DialogTwoButton.SetActive(false);
        });
    }
    private void OpenErrorDialog(string title = "Xóa Rack IO thất bại", string message = "Đã có lỗi xảy ra khi xóa Rack IO khỏi hệ thống. Vui lòng thử lại sau")
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
        if (GlobalVariable.APIRequestType.Contains("GET_Rack_List"))
        {
            OpenErrorDialog(title: "Tải danh sách thất bại", message: "Đã có lỗi xảy ra khi tải danh sách. Vui lòng thử lại sau");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Rack"))
        {
            OpenErrorDialog(title: "Xóa Rack IO thất bại", message: "Đã có lỗi xảy ra khi xóa Rack IO khỏi hệ thống. Vui lòng thử lại sau");
        }
    }
    public void ShowSuccess()
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Rack_List"))
        {
            Show_Toast.Instance.ShowToast("success", "Tải danh sách thành công");
        }
        if (GlobalVariable.APIRequestType.Contains("DELETE_Rack"))
        {
            listRackItems.Remove(_rackItem);
            Destroy(_rackItem);
            Show_Toast.Instance.ShowToast("success", "Xóa Rack IO thành công");
        }

        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }

    // Không dùng trong ListView
    public void DisplayDetail(RackInformationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }

    private void CreateRackItems(List<RackInformationModel> racks)
    {
        foreach (var model in racks)
        {
            // int RackIndex = models.IndexOf(model);
            // Debug.Log(RackIndex);
            var newRackItem = Instantiate(Rack_Item_Prefab, Parent_Vertical_Layout_Group.transform);
            newRackItem.SetActive(true);
            newRackItem.name = model.Name;
            Transform newRackItemTransform = newRackItem.transform;
            Transform newRackItemPreviewInforGroup = newRackItemTransform.GetChild(0);
            newRackItemPreviewInforGroup.Find("Preview_Rack_Name").GetComponent<TMP_Text>().text = model.Name;
            TMP_Text grapperText = newRackItemPreviewInforGroup.Find("Preview_Rack_GrapLocation")?.GetComponent<TMP_Text>();
            if (grapperText != null)
            {
                grapperText.text = GetGrapperName(grapperId);
            }
            Transform newRackItemPreviewButtonGroup = newRackItemTransform.GetChild(1);
            listRackItems.Add(newRackItem);
            newRackItemPreviewButtonGroup.Find("Group/Edit_Button").GetComponent<Button>().onClick.AddListener(() => EditRackItem(model.Id));
            newRackItemPreviewButtonGroup.Find("Group/Delete_Button").GetComponent<Button>().onClick.AddListener(() => DeleRackItem(newRackItem, model));
            listRackItems.Add(newRackItem);
        }
    }

    // Hàm xử lý nội dung tìm kiếm
    private void OnSearchValueChanged(string input)
    {
        string searchText = input.ToLower().Trim();

        foreach (var item in listRackItems)
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