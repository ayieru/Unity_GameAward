using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("経過時間")]
    private float ElapsedTime = 0;

    [Header("クリア時間")]
    public static float ClearTime = 0;

    bool EnableTimer = false;
    public GameObject TimeText;

    private void Start()
    {
        EnableTimer = true;
        ElapsedTime = 0;
        ClearTime = 0;
    }

    private void Update()
    {
        if (!EnableTimer)
        {
            return;
        }

        ElapsedTime += Time.deltaTime;

        //クリア時間を表示
        TimeText.GetComponent<Text>().text = ElapsedTime.ToString("N2");

        if (Goal.TimeStop == true)
        {
            ClearTime = ElapsedTime;
            EnableTimer = false;
        }
    }
}

//ElapsedTime.ToString("N2")
