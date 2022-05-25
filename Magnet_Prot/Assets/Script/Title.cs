using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private GameObject Startbutton;

    void Start()
    {
        FadeManager.FadeIn();
        Startbutton = transform.Find("StartButton").gameObject;

        //Startボタンが選択された状態にする
        Button button = Startbutton.GetComponent<Button>();
        button.Select();
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
#if UNITY_EDITOR // Unityの画面上
        UnityEditor.EditorApplication.isPlaying = false;
#else // ビルド後、exeファイル
		Application.Quit();
#endif
    }

}
