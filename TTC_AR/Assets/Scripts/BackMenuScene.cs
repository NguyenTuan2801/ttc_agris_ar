using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackMenuScene : MonoBehaviour
{
    [SerializeField] private Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
