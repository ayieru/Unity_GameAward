using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参照： http://blawat2015.no-ip.com/~mieki256/diary/201401303.html
//画像をループさせたい…けど難しい…

public class BackGroundManager : MonoBehaviour
{
    [Header("オフセット")]
    public float Offset;

    private Vector3 Oldpos;
    private Vector3 Oldcampos;

    void Start()
    {
        Oldpos = transform.position;
        Oldcampos = Camera.main.transform.position;
    }

    void Update()
    {
        Vector3 V = new Vector3((Camera.main.transform.position.x - Oldcampos.x) / Offset, 0, 0);
        transform.localPosition = Oldpos + V;
    }
}
