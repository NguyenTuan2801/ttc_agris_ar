using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NavScene : MonoBehaviour
{
    public string previousSceneName;
    public List<string> recentSceneName;
    public List<Button> listButton;

    private readonly Dictionary<string, (int grapperId, string deviceType)> plcBoxSceneConfig =
        new Dictionary<string, (int, string)>(StringComparer.OrdinalIgnoreCase)
    {
        // GRAP A
        { "PLCBoxGrapA_TemperatureSensor", (1, "Cảm biến nhiệt độ") },
        { "PLCBoxGrapA_LevelSensor",       (1, "Cảm biến mức") },
        { "PLCBoxGrapA_JB",                (1, "JB") },
        { "PLCBoxGrapA_PressureSensor",    (1, "Cảm biến áp suất") },
        { "PLCBoxGrapA_FlowSensor",        (1, "Cảm biến lưu lượng") },
        { "PLCBoxGrapA_Valve",             (1, "Van điều khiển") },
        { "PLCBoxGrapA_OtherDevice",       (1, "OtherDevice") },

        // GRAP B
        { "PLCBoxGrapB_TemperatureSensor", (2, "Cảm biến nhiệt độ") },
        { "PLCBoxGrapB_LevelSensor",       (2, "Cảm biến mức") },
        { "PLCBoxGrapB_JB",                (2, "JB") },
        { "PLCBoxGrapB_PressureSensor",    (2, "Cảm biến áp suất") },
        { "PLCBoxGrapB_FlowSensor",        (2, "Cảm biến lưu lượng") },
        { "PLCBoxGrapB_Valve",             (2, "Van điều khiển") },
        { "PLCBoxGrapB_OtherDevice",       (2, "OtherDevice") },

        // GRAP C
        { "PLCBoxGrapC_TemperatureSensor", (3, "Cảm biến nhiệt độ") },
        { "PLCBoxGrapC_LevelSensor",       (3, "Cảm biến mức") },
        { "PLCBoxGrapC_JB",                (3, "JB") },
        { "PLCBoxGrapC_PressureSensor",    (3, "Cảm biến áp suất") },
        { "PLCBoxGrapC_FlowSensor",        (3, "Cảm biến lưu lượng") },
        { "PLCBoxGrapC_Valve",             (3, "Van điều khiển") },
        { "PLCBoxGrapC_OtherDevice",       (3, "OtherDevice") },

        // LÒ HƠI
        { "PLCBoxLH_TemperatureSensor",    (4, "Cảm biến nhiệt độ") },
        { "PLCBoxLH_LevelSensor",          (4, "Cảm biến mức") },
        { "PLCBoxLH_JB",                   (4, "JB") },
        { "PLCBoxLH_PressureSensor",       (4, "Cảm biến áp suất") },
        { "PLCBoxLH_FlowSensor",           (4, "Cảm biến lưu lượng") },
        { "PLCBoxLH_Valve",                (4, "Van điều khiển") },
        { "PLCBoxLH_OtherDevice",          (4, "OtherDevice") },
    };

    private void Start()
    {
        for (int i = 0; i < listButton.Count; i++)
        {
            int localIndex = i;
            listButton[i].onClick.AddListener(() =>
            {
                StartCoroutine(NavigateNewScene(localIndex));
            });
        }
    }

    private IEnumerator NavigateNewScene(int buttonIndex)
    {
        string targetScene = recentSceneName[buttonIndex];

        if (targetScene != previousSceneName)
        {
            SetGrapperAndDeviceType(targetScene);
            GlobalVariable.ready_To_Nav_New_Scene = true;
            yield return Scene_Manager.Instance.WaitAndNavigate(targetScene, previousSceneName);
        }
    }

    private void SetGrapperAndDeviceType(string sceneName)
    {
        // Nếu là scene PLCBox → xử lý theo config
        if (plcBoxSceneConfig.TryGetValue(sceneName, out var config))
        {
            GlobalVariable.GrapperId = config.grapperId;
            GlobalVariable_Search_Devices.selectedDeviceType = config.deviceType;

            if (config.deviceType == "OtherDevice")
            {
                SetOtherDevices();
            }

            Debug.Log($"[NavScene] PLCBox Scene: {sceneName} | GrapperId = {config.grapperId} | Type = {config.deviceType}");
        }
        else
        {
            Debug.Log($"[NavScene] Scene khác (không phải PLCBox): {sceneName} → Không thay đổi GrapperId");
        }
    }

    private void SetOtherDevices()
    {
        if (GlobalVariable_Search_Devices.all_Device_Models == null ||
            GlobalVariable_Search_Devices.all_Device_Models.Count == 0)
        {
            Debug.LogError("all_Device_Models chưa được load!");
            GlobalVariable_Search_Devices.temp_ListDeviceInformationModel.Clear();
            return;
        }

        var excludedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Cảm biến nhiệt độ", "Cảm biến mức", "JB",
            "Cảm biến áp suất", "Cảm biến lưu lượng", "Van điều khiển"
        };

        GlobalVariable_Search_Devices.temp_ListDeviceInformationModel =
            GlobalVariable_Search_Devices.all_Device_Models
                .Where(device => !excludedTypes.Contains(device.Type))
                .ToList();

        Debug.Log($"[OtherDevice] GrapperId={GlobalVariable.GrapperId} → Loaded {GlobalVariable_Search_Devices.temp_ListDeviceInformationModel.Count} other devices.");
    }
}