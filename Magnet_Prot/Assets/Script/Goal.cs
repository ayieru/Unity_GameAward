using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Goal : MonoBehaviour
{
    [Header("現在のステージ番号")]
    public static int CurrentStageIndex;


    [Header("最後のステージか?")]
    public  bool IsLast = false;
    public static bool IsLastStage = false;

    [SerializeField]
    GameObject ClearUI;

    private bool timeStop = false;

    private void Start()
    {
        FadeManager.FadeIn();
        CurrentStageIndex = SceneManager.GetActiveScene().buildIndex;

        IsLastStage = IsLast;
    }

    private void Update()
    {
        if (timeStop)
        {
            if (Input.anyKeyDown)
            {
                timeStop = false;
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
            timeStop = true;
        }
    }
}
