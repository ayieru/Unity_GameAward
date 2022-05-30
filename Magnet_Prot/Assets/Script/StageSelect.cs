using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    const int STAGE_TAX = 20;

    [Header("ステージID(text)")]
    public Text StageIDText;

    [Header("クリアランク(text)")]
    public Text ClearRankText;

    [Header("ステージの画像を格納する配列")]
    public Image[] StageButton;

    [Header("SE：選択")]
    public AudioClip SeSelect;
    [Header("SE：決定")]
    public AudioClip SeDecision;

    [Header("選択中のステージの要素番号")]
    public static int SelectID = 0;//1-1

    [Header("各ステージのクリアランク")]
    public static int[] StageClearRank;

    private float BeforeHorizintal;
    AudioSource audioSource;

    [RuntimeInitializeOnLoadMethod()]
    private static void Initialize()
    {
        StageClearRank = new int[STAGE_TAX];
    }

    void Start()
    {
        FadeManager.FadeIn();

        //IDテキスト変更
        StageIDChange();

        StageButton[SelectID].gameObject.SetActive(true);
        DisplayClearRank();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //該当するステージを開始する
        if (Input.GetButtonDown("Action"))
        {
            audioSource.PlayOneShot(SeDecision);
            FadeManager.FadeOut(StageButton[SelectID].gameObject.name);

        }

        //タイトルへ戻る
        if (Input.GetKeyDown(KeyCode.Escape) == true || Input.GetButtonDown("Jump"))
        {
            FadeManager.FadeOut("Title");
        }

        //横入力反応処理：前フレームの入力値が0の場合のみ
        if (Input.GetAxisRaw("Horizontal") > 0 && BeforeHorizintal == 0.0)
        {
            MoveRight();
            audioSource.PlayOneShot(SeSelect);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && BeforeHorizintal == 0.0)
        {
            MoveLeft();
            audioSource.PlayOneShot(SeSelect);
        }

        BeforeHorizintal = Input.GetAxisRaw("Horizontal");
    }

    //ステージIDのテキストを変更する
    private void StageIDChange()
    {
        StageIDText.text = StageButton[SelectID].gameObject.name;
    }

    //画像切り替え:左
    private void MoveLeft()
    {
        //現在の画像を非アクティブに変更
        StageButton[SelectID].gameObject.SetActive(false);

        //1-1を選択していた場合は、最後のステージの画像へ移動
        if (SelectID == 0)
        {
            SelectID = StageButton.Length - 1;
        }
        else
        {
            SelectID -= 1;
        }

        //次の画像をアクティブに変更
        StageButton[SelectID].gameObject.SetActive(true);

        //IDテキスト変更
        StageIDChange();

        DisplayClearRank();
    }

    //画像切り替え:右
    private void MoveRight()
    {
        //現在の画像を非アクティブに変更
        StageButton[SelectID].gameObject.SetActive(false);

        //最後のステージを選択していた場合は、1-1の画像へ移動
        if (SelectID == StageButton.Length - 1)
        {
            SelectID = 0;
        }
        else
        {
            SelectID += 1;
        }

        //次の画像をアクティブに変更
        StageButton[SelectID].gameObject.SetActive(true);

        //IDテキスト変更
        StageIDChange();

        DisplayClearRank();
    }

    //クリアランクを表示する
    private void DisplayClearRank()
    {
        if (StageClearRank[SelectID] == 1)
        {
            ClearRankText.GetComponent<Text>().text = "S";
        }
        else if (StageClearRank[SelectID] == 2)
        {
            ClearRankText.GetComponent<Text>().text = "A";
        }
        else if (StageClearRank[SelectID] == 3)
        {
            ClearRankText.GetComponent<Text>().text = "B";
        }
        else if (StageClearRank[SelectID] == 4)
        {
            ClearRankText.GetComponent<Text>().text = "C";
        }
        else
        {
            ClearRankText.GetComponent<Text>().text = "";
        }
    }


}
