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
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �Z�[�u����
        if (collision.gameObject.CompareTag("Player") && SaveJudge == false)
        {
            Debug.Log("�Z�[�u����");

            // �Z�[�u�|�C���g���ݒu���Ă�����W���v���C���[�����A������W�ɑ��
            SavePointPos = SavePointTransform.position;
            SaveJudge = true;
        }
    }
}