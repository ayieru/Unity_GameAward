using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //次のステージへ移動する
    public void NextStage()
    {

    }

    public void StageSelect()
    {
        //ステージセレクトシーンへ移動
        FadeManager.FadeOut("StageSelect");
    }

    //タイトル処理
    public void Title()
    {
        //タイトルシーンへ移動
        FadeManager.FadeOut("Title");
    }


}
