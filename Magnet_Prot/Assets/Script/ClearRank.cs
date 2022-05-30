using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Goal.CurrentStageIndex - 2：ビルドインデックスから
 *タイトルとステージセレクトの2つ分の値を引いた数が
 ゲームのステージのインデックスになる*/

public class ClearRank : MonoBehaviour
{

    private void Start()
    {
        StageSelect.StageClearRank[Goal.CurrentStageIndex - 2] = Goal.Rank;
        Debug.Log(Timer.ClearTime);

        DisplayClearRank();
    }

    //クリアランクを表示する
    private void DisplayClearRank()
    {
        if (Goal.Rank == 1)
        {
            gameObject.GetComponent<Text>().text = "S";
        }
        else if(Goal.Rank == 2)
        {
            gameObject.GetComponent<Text>().text = "A";
        }
        else if(Goal.Rank ==3)
        {
            gameObject.GetComponent<Text>().text = "B";
        }
        else
        {
            gameObject.GetComponent<Text>().text = "C";
        }
    }

}
