using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("Start(text)")]
    public Text StartText;

    [Header("Exit(text)")]
    public Text ExitText;

    void Start()
    {
        FadeManager.FadeIn();

    }

    void Update()
    {
        
    }

    //Textの色を変更する処理
    private void TextColorChange(Text text)
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
