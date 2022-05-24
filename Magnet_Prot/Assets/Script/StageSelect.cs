using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [Header("ステージID(text)")]
    public Text StageIDText;

    [Header("ステージの画像を格納する配列")]
    public Image[] StageButton;

    [Header("選択中のステージの要素番号")]
    public static int SelectID = 0;//1-1

    private float BeforeHorizintal;

    void Start()
    {
        FadeManager.FadeIn();

        //IDテキスト変更
        StageIDChange();
    }

    void Update()
    {
        //コマンドが押されたら、該当するステージを開始する
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Action"))
        {
            FadeManager.FadeOut(StageButton[SelectID].gameObject.name);
        }

        //横入力反応処理：前フレームの入力値が0の場合のみ
        if (Input.GetAxisRaw("Horizontal") > 0 && BeforeHorizintal == 0.0)
        {
            MoveRight();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && BeforeHorizintal == 0.0)
        {
            MoveLeft();
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
    }

}
