using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Vuforia;

public class VuforiaBarcodeARManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject qrButtonPrefab;
    public float relativeScale = 0.78f;
    public Vector3 localOffset = new Vector3(0, 0.22f, 0);

    private Dictionary<string, GameObject> activeButtons = new Dictionary<string, GameObject>();

    private void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
    }

    private void OnVuforiaStarted()
    {
        foreach (var barcode in FindObjectsOfType<BarcodeBehaviour>())
        {
            barcode.OnTargetStatusChanged += OnBarcodeStatusChanged;
        }
    }

    private void OnBarcodeStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (behaviour is not BarcodeBehaviour barcodeBehaviour) return;

        string content = barcodeBehaviour.InstanceData?.Text?.Trim();
        if (string.IsNullOrEmpty(content)) return;

        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (!activeButtons.ContainsKey(content))
            {
                CreateQRButton(content, barcodeBehaviour.transform);
            }
        }
        else if (status.Status == Status.NO_POSE)
        {
            RemoveQRButton(content);
        }
    }

    private void CreateQRButton(string content, Transform qrTransform)
    {
        GameObject newButton = Instantiate(qrButtonPrefab, qrTransform);
        newButton.name = "AR_" + content;

        // Set text cho TextMeshPro
        TextMeshProUGUI tmpText = newButton.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpText != null)
        {
            string[] parts = content.Split('_');
            string suffix = parts.Length > 0 ? parts[parts.Length - 1] : content;
            tmpText.text = suffix;
        }

        // === QUAN TRỌNG: Gán displaySuffix vào ARQRMarker ===
        ARQRMarker marker = newButton.GetComponent<ARQRMarker>();
        if (marker != null)
        {
            marker.displaySuffix = content.Split('_').LastOrDefault() ?? content;
            Debug.Log($"Gán displaySuffix = {marker.displaySuffix} cho button {content}");
        }
        else
        {
            Debug.LogError($"ARQRMarker chưa được attach trên prefab ARGameObject!");
        }

        newButton.transform.localPosition = localOffset;
        newButton.transform.localScale = Vector3.one * relativeScale;
        newButton.transform.localRotation = Quaternion.Euler(0, 180, 0);

        activeButtons[content] = newButton;
    }
    private void RemoveQRButton(string content)
    {
        if (activeButtons.TryGetValue(content, out GameObject btn) && btn != null)
        {
            Destroy(btn);
            activeButtons.Remove(content);
        }
    }

    private void LateUpdate()
    {
        foreach (var btn in activeButtons.Values)
        {
            if (btn == null) continue;

            Vector3 dirToCamera = Camera.main.transform.position - btn.transform.position;

            // Cải tiến: Giữ cho button luôn "đứng thẳng" hơn khi camera di chuyển ngang
            Quaternion lookRot = Quaternion.LookRotation(dirToCamera);
            Quaternion uprightRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);  // Chỉ lấy góc Y
            Quaternion finalRot = uprightRot * Quaternion.Euler(0, 180, 0);

            btn.transform.rotation = Quaternion.Slerp(btn.transform.rotation, finalRot, 20f * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
    }

    public Dictionary<string, GameObject> GetActiveButtons()
    {
        return activeButtons;
    }
}