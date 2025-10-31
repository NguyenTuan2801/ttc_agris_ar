using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Open_Selection_DeviceType : MonoBehaviour
{
    [SerializeField] private Button GrapA_Button;
    [SerializeField] private Button GrapB_Button;
    [SerializeField] private Button GrapC_Button;
    [SerializeField] private Button LH_Button;

    // Start is called before the first frame update
    void Start()
    {
        GrapA_Button.onClick.AddListener(OpenSelectionDeviceGrapA);
        GrapB_Button.onClick.AddListener(OpenSelectionDeviceGrapB);

    }

    private void OpenSelectionDeviceGrapA()
    {
        SceneManager.LoadScene("SelectionDeviceTypeGrapA");
    }

    private void OpenSelectionDeviceGrapB()
    {
        SceneManager.LoadScene("SelectionDeviceTypeGrapB");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
