using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Goal.CurrentStageIndex - 2：ビルドインデックスから
 *タイトルとステージセレクトの2つ分の値を引いた数が
 ゲームのステージのインデックスになる*/

public class ClearRank : MonoBehaviour
{
    private string text;

    private void Start()
    {
        StageSelect.StageClearRank[Goal.CurrentStageIndex - 2] = Goal.Rank;
        Debug.Log(Timer.ClearTime);

        text = gameObject.GetComponent<Text>().text;

        DisplayClearRank();
    }

    //クリアランクを表示する
    private void DisplayClearRank()
    {
        if (Goal.Rank == 1)
        {
            text = "S";
        }
        else if(Goal.Rank == 2)
        {
            text = "A";
        }
        else if(Goal.Rank ==3)
        {
            text = "B";
        }
        else
        {
            text = "C";
        }
    }
}
