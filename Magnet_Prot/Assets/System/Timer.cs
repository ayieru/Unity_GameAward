using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("制限時間")]
    [SerializeField]
    private float LimitTime = 300;

    [Header("経過時間")]
    private float ElapsedTime = 0;

    [Header("クリア時間")]
    public static float ClearTime = 0;

    bool EnableTimer = false;
    public GameObject TimeText;

    public static bool RankSave;

    private void Start()
    {
        EnableTimer = true;
        ElapsedTime = 0;
        ClearTime = 0;
        RankSave = false;
    }

    private void Update()
    {
        if (!EnableTimer)
        {
            return;
        }

        LimitTime -= Time.deltaTime;

        ElapsedTime += Time.deltaTime;

        //制限時間を表示
        TimeText.GetComponent<Text>().text = LimitTime.ToString("f0");

        if(LimitTime<=0.0f)
        {
            LimitTime = 0.0f;

            EnableTimer = false;

            FadeManager.FadeOut("Over");

            return;
        }

        if (Goal.TimeStop == true)
        {
            ClearTime = ElapsedTime;
            RankSave = true;
            EnableTimer = false;
        }
    }
}

//ElapsedTime.ToString("N2")
