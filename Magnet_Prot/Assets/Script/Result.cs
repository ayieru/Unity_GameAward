using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private GameObject NextStageButton;
    private GameObject SelectButton;
    private GameObject StageText;

    void Start()
    {
        FadeManager.FadeIn();
        NextStageButton = transform.Find("NextStage").gameObject;
        SelectButton = transform.Find("Select").gameObject;
        StageText = transform.Find("StageIDText").gameObject;

        //最終ステージの場合
        if (Goal.IsLastStage == true)
        {
            NextStageButton.gameObject.SetActive(false);
            Button button = SelectButton.GetComponent<Button>();
            button.Select();

        }
        else
        {
            Button button = NextStageButton.GetComponent<Button>();
            button.Select();
        }

        //ステージ名を表示
        StageText.GetComponent<Text>().text = Goal.CurrentStageName.ToString();
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
