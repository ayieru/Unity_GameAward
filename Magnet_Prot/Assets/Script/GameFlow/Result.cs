﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField]
    [Header("最初に選択しておくボタン")]
    private Button DefaultSelectButton;

    private GameObject NextStageButton;
    private GameObject SelectButton;
    private GameObject StageText;
    private GameObject TimeText;

    void Start()
    {
        FadeManager.FadeIn();
        NextStageButton = transform.Find("NextStage").gameObject;
        SelectButton = transform.Find("Select").gameObject;
        StageText = transform.Find("StageID").gameObject;
        TimeText = transform.Find("ClearTime").gameObject;

        //最終ステージの場合
        if (Goal.IsLastStage == true)
        {
            NextStageButton.gameObject.SetActive(false);
            Button button = SelectButton.GetComponent<Button>();
            button.Select();
        }
        else
        {
            Button button = DefaultSelectButton.GetComponent<Button>();
            button.Select();
        }

        //ステージ名を表示
        StageText.GetComponent<Text>().text = Goal.CurrentStageName.ToString();

        //クリア時間を表示
        TimeText.GetComponent<Text>().text = Timer.ClearTime.ToString("N2");

    }

    //次のステージへの移動処理
    public void NextStage()
    {
        FadeManager.FadeOut(Goal.CurrentStageIndex + 1);
    }

    //ステージセレクトシーンへの移動処理
    public void StageSelect()
    {
        FadeManager.FadeOut("StageSelect");
    }

    //リトライ処理
    public void Retry()
    {
        FadeManager.FadeOut(Goal.CurrentStageIndex);
    }
}
