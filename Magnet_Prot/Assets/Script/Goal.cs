using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Goal : MonoBehaviour
{
    [Header("現在のステージ番号")]
    public static int CurrentStageIndex;

    [Header("現在のステージ名")]
    public static string CurrentStageName;

    [Header("最後のステージか?")]
    public  bool IsLast = false;
    public static bool IsLastStage = false;

    [Header("クリアしたか?")]
    public static bool TimeStop = false;

    [Header("クリアタイム：S")]
    public  float S;
    [Header("クリアタイム：A")]
    public  float A;
    [Header("クリアタイム：B")]
    public  float B;
    [Header("クリアタイム：C")]
    public  float C;

    [Header("クリアランク")]
    public static int Rank = 0;

    public static float TimeS;
    public static float TimeA;
    public static float TimeB;
    public static float TimeC;

    public GameObject ClearUI;

    private void Start()
    {
        FadeManager.FadeIn();

        CurrentStageIndex = SceneManager.GetActiveScene().buildIndex;
        CurrentStageName = SceneManager.GetActiveScene().name;
        IsLastStage = IsLast;

        TimeS = S;
        TimeA = A;
        TimeB = B;
        TimeC = C;
    }

    private void Update()
    {
        if (TimeStop)
        {
            if (Input.anyKeyDown)
            {
                TimeStop = false;
                Time.timeScale = 1;
                FadeManager.FadeOut("Result");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TimeStop = true;
            ClearUI.SetActive(true);
            Time.timeScale = 0;
            ClearStatus();
        }
    }

    //クリアランクを求める
    private void ClearStatus()
    {
        if(Timer.ClearTime < TimeS)
        {
            Rank = 1;
        }
        else if(Timer.ClearTime < TimeA)
        {
            Rank = 2;
        }
        else if(Timer.ClearTime < TimeB)
        {
            Rank = 3;
        }
        else if(Timer.ClearTime < TimeC)
        {
            Rank = 4;
        }
    }
}
