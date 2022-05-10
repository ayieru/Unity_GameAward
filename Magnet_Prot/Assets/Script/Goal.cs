using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//※注意※　仮実装

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject clearUI;

    private bool timeStop = false;

    private void Update()
    {
        if (timeStop)
        {
            if (Input.anyKeyDown)
            {
                timeStop = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
