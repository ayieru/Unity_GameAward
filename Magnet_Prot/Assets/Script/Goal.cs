﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject clearUI;

    private bool timeStop = false;

    private void Start()
    {
        FadeManager.FadeIn();
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
            clearUI.SetActive(true);
            Time.timeScale = 0;
            timeStop = true;
        }
    }
}
