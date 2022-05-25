using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    [Header("Startボタン")]
    public Button Startbutton;

    void Start()
    {
        FadeManager.FadeIn();

        //Startボタンが選択された状態にする
        Button button = Startbutton.GetComponent<Button>();
        button.Select();

    }

    void Update()
    {
        
    }

    //ゲーム開始処理
    public void GameStart()
    {
        //ステージセレクトシーンへ移動
        FadeManager.FadeOut("StageSelect");

    }

    //ゲーム終了処理
    public void Exit()
    {
        //ゲームを終了する
#if UNITY_EDITOR // Unityの画面上
        UnityEditor.EditorApplication.isPlaying = false;
#else // ビルド後、exeファイル
		Application.Quit();
#endif

    }

}
