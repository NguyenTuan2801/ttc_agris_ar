using UnityEngine;

public class ARQRMarker : MonoBehaviour
{
    [HideInInspector]
    public string displaySuffix;

    public void OnButtonClicked()
    {
        if (string.IsNullOrEmpty(displaySuffix))
        {
            Debug.LogWarning("displaySuffix vẫn rỗng!");
            return;
        }

        Debug.Log($"=== AR BUTTON CLICKED === Module: {displaySuffix}");

        Manager manager = FindObjectOfType<Manager>();
        if (manager != null)
        {
            manager.OpenModuleFromAR(displaySuffix);
        }
        else
        {
            Debug.LogError("Không tìm thấy Manager!");
        }
    }
}