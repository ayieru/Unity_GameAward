using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q��: https://kinokorori.hatenablog.com/entry/2019/01/16/000000

public class MiddleCameraController : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g")]
    private GameObject Player;

    [Header("�J�n���̃v���C���[�I�t�Z�b�g")]
    private Vector3 StartPlayerOffset;

    [Header("�J�n���̃J�����ʒu")]
    private Vector3 StartCameraPos;

    [Header("�X�N���[���̍���")]
    public float Rate = 0.15f;

    void Start()
    {
        Player = GameObject.Find("Player");
        StartPlayerOffset = Player.transform.position;
        StartCameraPos = this.transform.position;
    }

    void Update()
    {
        Vector3 v = (Player.transform.position - StartPlayerOffset) * Rate;
        this.transform.position = StartCameraPos + v;
    }
}
