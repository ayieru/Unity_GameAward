using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//éQè∆: https://kinokorori.hatenablog.com/entry/2019/01/16/000000

public class MiddleCameraController : MonoBehaviour
{
    private GameObject player;
    private Vector3 startPlayerOffset;
    private Vector3 startCameraPos;
    public  float RATE = 0.05f;

    void Start()
    {
        player = GameObject.Find("Player");
        startPlayerOffset = player.transform.position;
        startCameraPos = this.transform.position;
    }

    void Update()
    {
        Vector3 v = (player.transform.position - startPlayerOffset) * RATE;
        this.transform.position = startCameraPos + v;
    }
}
