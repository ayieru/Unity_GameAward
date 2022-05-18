using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFieldBrockManager : MonoBehaviour
{
    public float SaveCenterPosX; // オブジェクトの中心X座標
    public float SaveCenterPosY; // オブジェクトの中心Y座標

    public float SaveRightPosX;  // 右側のX座標保存
    public float SaveLeftPosX;   // 左側のX座標保存
    public float SaveTopPosY;    // Y座標保存


    public float GetSaveRightPosX() { return SaveRightPosX; }
    public float GetSaveLeftPosX() { return SaveLeftPosX; }
    public float GetSaveTopPosY() { return SaveTopPosY; }
}
