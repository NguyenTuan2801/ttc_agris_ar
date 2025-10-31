using EasyUI.Progress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserMenu : MonoBehaviour
{
    [SerializeField] private GameObject userMenu;
    [SerializeField] private Button openUserMenuButton;
    [SerializeField] private Button closeUserMenuButton;
    [SerializeField] private Button blurPanelButton;
    [SerializeField] private Button signOutButton;

    // Start is called before the first frame update
    void Start()
    {
        openUserMenuButton.onClick.AddListener(OpenUserMenu);
        closeUserMenuButton.onClick.AddListener(CloseUserMenu);
        blurPanelButton.onClick.AddListener(CloseUserMenu);
        signOutButton.onClick.AddListener(SignOut);
        userMenu.SetActive(false);
        blurPanelButton.gameObject.SetActive(false);
    }

    private void OpenUserMenu()
    {
        userMenu.SetActive(true);
        blurPanelButton.gameObject.SetActive(true);
    }
    private void CloseUserMenu()
    {
        userMenu.SetActive(false);
        blurPanelButton.gameObject.SetActive(false);
    }
    private void SignOut()
    {
        StartCoroutine(SignOutCoroutine());
    }
    
    private IEnumerator SignOutCoroutine()
    {
        ShowLoading("Đang đăng xuất...");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LoginScene");
    }

    private void ShowProgressBar(string title, string details)
    {
        Progress.Show(title, ProgressColor.Blue, true);
        Progress.SetDetailsText(details);
    }

    private IEnumerator HideProgressBar()
    {
        yield return new WaitForSeconds(0.5f);
        Progress.Hide();
    }
    public void ShowLoading(string title) => ShowProgressBar(title, "Đang đăng xuất...");
    public void HideLoading() => StartCoroutine(HideProgressBar());

    // Update is called once per frame
    void Update()
    {
        
    }
}
