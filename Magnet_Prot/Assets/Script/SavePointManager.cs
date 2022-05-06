using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    [Header("プレイヤーの初期座標入力")]
    public Vector2 SavePointPos;// プレイヤーが復帰するセーブポイント座標

    public bool SaveJudge = false;

    public float GetSavePointPosX() { return SavePointPos.x; }
    public float GetSavePointPosY() { return SavePointPos.y; }
}
