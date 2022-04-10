using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参照: https://kinokorori.hatenablog.com/entry/2019/01/16/000000

public class MiddleCameraController : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    private GameObject Player;

    [Header("開始時のプレイヤーオフセット")]
    private Vector3 StartPlayerOffset;

    [Header("開始時のカメラ位置")]
    private Vector3 StartCameraPos;

    [Header("スクロールの差分")]
    public float Rate = 0.15f;

    void Start()
    {
        Player = GameObject.Find("Player");
        StartPlayerOffset = Player.transform.position;
        StartCameraPos = this.transform.position;
    }

    void Update()
    {
        Vector3 v = (Player.transform.position - StartPlayerOffset) * Rate;
        this.transform.position = StartCameraPos + v;
    }
}
