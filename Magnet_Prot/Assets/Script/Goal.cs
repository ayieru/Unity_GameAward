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

    public GameObject ClearUI;

    private void Start()
    {
        FadeManager.FadeIn();

        CurrentStageIndex = SceneManager.GetActiveScene().buildIndex;
        CurrentStageName = SceneManager.GetActiveScene().name;
        IsLastStage = IsLast;
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
            ClearUI.SetActive(true);
            Time.timeScale = 0;
            TimeStop = true;
        }
    }
}
