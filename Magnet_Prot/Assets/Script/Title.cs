using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("Start(text)")]
    public Text StartText;

    [Header("Exit(text)")]
    public Text ExitText;

    void Start()
    {
        FadeManager.FadeIn();

    }

    void Update()
    {
        
    }

    public void GameStart()
    {

    }
}
