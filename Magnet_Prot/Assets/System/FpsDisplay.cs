using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    int frameCount;
    float prevTime;
    float fps;

    [SerializeField]
    int FpsLimit = 60;

    [SerializeField]
    int FontSize = 50;

    private void Start()
    {
        frameCount = 0;
        prevTime = 0.0f;

        Application.targetFrameRate = FpsLimit;
    }

    private void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        GUI.skin.label.fontSize = FontSize;
        GUILayout.Label(fps.ToString("f1") + "fps");
#endif
    }
}
