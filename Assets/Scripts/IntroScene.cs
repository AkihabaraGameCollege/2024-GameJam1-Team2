using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    public Button transitionButton; // ボタンをインスペクタで設定するための変数
    // Start is called before the first frame update
    void Start()
    {
        // ボタンが設定されていれば、クリック時にシーン遷移のメソッドを呼び出す
        if (transitionButton != null)
        {
            transitionButton.onClick.AddListener(OnButtonClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Test Stage 1");
        }   
    }

    // ボタンがクリックされた時に呼び出されるメソッド
    void OnButtonClick()
    {
        SceneManager.LoadScene("Test Stage 1");
    }
}
