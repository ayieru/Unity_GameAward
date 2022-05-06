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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �Z�[�u�����i1�����j
        if (SaveJudge) return;

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�Z�[�u����");

            // �Z�[�u�|�C���g���ݒu���Ă�����W���v���C���[�����A������W�ɑ��
            SavePointPos = SavePointTransform.position;
            SaveJudge = true;
            instance = this;
        }
    }
}