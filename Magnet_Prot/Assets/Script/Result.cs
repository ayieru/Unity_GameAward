using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private GameObject NextStageButton;

    void Start()
    {
        FadeManager.FadeIn();
        NextStageButton = transform.Find("NextStageButton").gameObject;

        //最終ステージの場合
        if (Goal.IsLastStage == true)
        {
            NextStageButton.gameObject.SetActive(false);
        }
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

    //タイトルシーンへの移動処理
    public void Title()
    {
        FadeManager.FadeOut("Title");
    }
}
