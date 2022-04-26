using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : SavePointManager
{
    Transform SavePointTransform;

    public static SavePoint instance;

    void Awake()
    {
        SavePointTransform = this.transform;// transform���擾
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �Z�[�u�����i1�����j
        if (collision.gameObject.CompareTag("Player") && SaveJudge == false)
        {
            Debug.Log("�Z�[�u����");

            // �Z�[�u�|�C���g���ݒu���Ă�����W���v���C���[�����A������W�ɑ��
            SavePointPos = SavePointTransform.position;
            SaveJudge = true;
            instance = this;
        }
    }
}