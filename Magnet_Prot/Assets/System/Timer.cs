using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float TimeLimit = 0;

    bool EnableTimer = false;

    private void Start()
    {
        EnableTimer = true;
    }

    private void Update()
    {
        if (!EnableTimer)
        {
            return;
        }

        TimeLimit -= Time.deltaTime;

        if (TimeLimit <= 0.0)
        {
            TimeLimit = 0.0f;
            EnableTimer = false;
        }
    }
}
