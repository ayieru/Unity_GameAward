using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    [Header("�v���C���[�̏������W����")]
    public Vector2 SavePointPos;// �v���C���[�����A����Z�[�u�|�C���g���W

    public bool SaveJudge = false;

    public float GetSavePointPosX() { return SavePointPos.x; }
    public float GetSavePointPosY() { return SavePointPos.y; }
}
