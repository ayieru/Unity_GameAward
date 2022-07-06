using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    private bool SaveJudge = false; // セーブ地点通ったか通ってないか判定

    public bool GetSaveJadge() { return SaveJudge; }            // 判定をゲットする
    public void SetSaveJadge(bool jadge) { SaveJudge = jadge; } // 判定をセットする

    protected Transform SavePointPos; // プレイヤーが復帰するセーブポイント座標

    public Transform GetSavePointPos() { return SavePointPos; }
    //public float GetSavePointPosY() { return SavePointPos.y; }
}
