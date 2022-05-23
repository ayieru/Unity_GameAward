using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    [Header("pausePanel Obj")] private GameObject pausePanel;
    [Header("scene Obj")] public GameObject sceneObj;

    //ポーズ中か確かめるフラグ
    private bool pauseflug = false;

    //スクリプト
    Scene loadScene;

    [Header("現在のシーン")]
    private string NowScene;

    [SerializeField]
    [Header("ゲームに戻るボタン")]
    private Button ResumeButton;

    void Start()
    {
        //ポーズフラグをfalseにする
        pauseflug = false;

        //現在のシーン名を取得
        NowScene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {

        //ポーズ中じゃないとき
        if (pauseflug == false)
        {
            //ポーズボタンが押されたら
            if (Input.GetKeyDown(KeyCode.Escape) == true || Input.GetButtonDown("PauseKey"))
            {
                //ポーズする
                PauseNow();
            }
        }

    }

    //ポーズ処理
    public void PauseNow()
    {
        //ポーズ中
        pauseflug = true;

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
        //ポーズ終了
        pauseflug = false;

        //止めていた時間を動かす
        Time.timeScale = 1.0f;

        //ポーズメニューを非アクティブにする
        pausePanel.SetActive(false);
    }

    //リトライ処理
    public void Retry()
    {
        //一度ゲームに戻る処理を行う
        Resume();

        //現在のシーンを読み込む
        FadeManager.FadeOut(NowScene);

    }

    //ステージセレクト処理
    public void StageSelect()
    {
        //一度ゲームに戻る処理を行う
        Resume();

        //ステージセレクトシーンへ移動
        FadeManager.FadeOut(NowScene);
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
