using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�ƁF http://blawat2015.no-ip.com/~mieki256/diary/201401303.html
//�摜�����[�v���������c���Ǔ���c

public class BackGroundManager : MonoBehaviour
{
    [Header("�I�t�Z�b�g")]
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
