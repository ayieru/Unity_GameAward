﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    [Header("Panel Obj")] private GameObject pausePanel;

    [SerializeField]
    [Header("ゲームに戻るボタン")]
    private Button ResumeButton;

    [Header("現在のシーン")]
    private string NowScene;

    [Header("SE：ポーズ")]
    public AudioClip SePause;

    [Header("SE：決定")]
    public AudioClip SeDecision;

    AudioSource audioSource;

    //ポーズ中か
    private bool Pauseflug = false;
    private bool CallOnce = false;

    //スクリプト
    Scene loadScene;

    void Start()
    {
        //フラグを初期化
        Pauseflug = false;
        CallOnce = false;

        //現在のシーン名を取得
        NowScene = SceneManager.GetActiveScene().name;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true || Input.GetButtonDown("PauseKey"))
        {
            Pauseflug = !Pauseflug;
        }

        if(Pauseflug == true && CallOnce == false)
        {
            CallOnce = true;
            PauseNow();
        }

        if(pausePanel.activeSelf == true && Pauseflug == false)
        {
            Resume();
        }

    }

    //ポーズ処理
    public void PauseNow()
    {
        audioSource.PlayOneShot(SePause);

        //ゲーム内の時間を停止する
        Time.timeScale = 0;

        //ポーズメニューをアクティブにする
        pausePanel.SetActive(true);

        //ゲームに戻るボタンが選択された状態にする
        Button button = ResumeButton.GetComponent<Button>();
        button.Select();

    }

    //ゲームに戻る処理
    public void Resume()
    {
        audioSource.PlayOneShot(SeDecision);

        //ポーズ終了
        Pauseflug = false;
        CallOnce = false;

        //止めていた時間を動かす
        Time.timeScale = 1.0f;

        //ポーズメニューを非アクティブにする
        pausePanel.SetActive(false);
    }

    //リトライ処理
    public void Retry()
    {
        audioSource.PlayOneShot(SeDecision);

        //一度ゲームに戻る処理を行う
        Resume();

        //現在のシーンを読み込む
        FadeManager.FadeOut(NowScene);

    }

    //ステージセレクト処理
    public void StageSelect()
    {
        audioSource.PlayOneShot(SeDecision);

        //一度ゲームに戻る処理を行う
        Resume();

        //ステージセレクトシーンへ移動
        FadeManager.FadeOut("StageSelect");
    }

    //ゲームをやめる処理
    private void Exit()
    {
        //止めていた時間を動かす
        Time.timeScale = 1.0f;

        //ゲームを終了する
#if UNITY_EDITOR // Unityの画面上
        UnityEditor.EditorApplication.isPlaying = false;
#else // ビルド後、exeファイル
		Application.Quit();
#endif

    }

}
