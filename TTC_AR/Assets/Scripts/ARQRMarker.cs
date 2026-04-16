using UnityEngine;

public class ARQRMarker : MonoBehaviour
{
    [HideInInspector]
    public string fullQRContent;  

    [HideInInspector]
    public string displaySuffix;

    public void OnButtonClicked()
    {
        if (string.IsNullOrEmpty(fullQRContent))
        {
            Debug.LogWarning("fullQRContent rỗng!");
            return;
        }

        Manager manager = FindObjectOfType<Manager>();
        if (manager != null)
        {
            manager.OpenModuleFromAR(fullQRContent); 
        }
        else
        {
            Debug.LogError("Không tìm thấy Manager!");
        }
    }
}