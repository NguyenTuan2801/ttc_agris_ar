using System.Collections.Generic;
using System.Linq;
using EasyUI.Progress;
using UnityEngine;

public class InitModuleScanQRView : MonoBehaviour, IModuleView
{
    private ModulePresenter _modulePresenter;
    private int grapperId;
    void Awake()
    {
        _modulePresenter = new ModulePresenter(this, ManagerLocator.Instance.ModuleManager._IModuleService);
    }
    void OnEnable()
    {
        grapperId = GlobalVariable.GrapperId;
        LoadListModule();
    }
    void OnDisable()
    {
    }

    public void LoadListModule()
    {
        int grapperId = GlobalVariable.GrapperId;
        Debug.Log($"LoadListModule theo grapperId = {grapperId}");

        if (grapperId <= 0)
        {
            Debug.LogWarning("GrapperId không hợp lệ!");
            return;
        }

        // Reset dictionary trước khi load mới
        GlobalVariable.temp_Dictionary_ModuleInformationModel.Clear();

        _modulePresenter.LoadListModule(grapperId);
    }

    public void DisplayList(List<ModuleInformationModel> models)
    {
        if (models == null || models.Count == 0)
        {
            Debug.LogWarning("Không có module nào trong grapper này!");
            return;
        }

        GlobalVariable.temp_Dictionary_ModuleInformationModel.Clear();

        foreach (var m in models)
        {
            if (m == null || string.IsNullOrEmpty(m.Name)) continue;

            string key = $"{GlobalVariable.GrapperId}_{m.Name}";
            GlobalVariable.temp_Dictionary_ModuleInformationModel[key] = m;

            Debug.Log($"Lưu module: {m.Name} (Id={m.Id}, GrapperId={GlobalVariable.GrapperId})");
        }

        Debug.Log($"Đã lưu {models.Count} modules cho GrapperId = {GlobalVariable.GrapperId}");
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

    public void ShowLoading(string title)
    {
        ShowProgressBar(title, "Đang tải dữ liệu...");
    }
    public void HideLoading() => HideProgressBar();
    public void ShowError(string message)
    {
    }

    public void ShowSuccess(string message)
    {
        if (GlobalVariable.APIRequestType.Contains("GET_Module_List"))
        {
            Show_Toast.Instance.ShowToast("success", message);
        }
        StartCoroutine(Show_Toast.Instance.Set_Instance_Status_False());
    }
    // Không dùng trong ListView
    public void DisplayDetail(ModuleInformationModel model) { }
    public void DisplayCreateResult(bool success) { }
    public void DisplayUpdateResult(bool success) { }
    public void DisplayDeleteResult(bool success) { }
}

