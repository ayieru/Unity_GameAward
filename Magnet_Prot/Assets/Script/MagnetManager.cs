using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    public enum Magnet_Pole
    {
        S,
        N,
        None,
    }

    [Header("��")]
    public Magnet_Pole Pole = Magnet_Pole.N;

    public Magnet_Pole GetPole() { return Pole; }

    float magForce;         //���͂��󂯂�I�u�W�F�N�g�ɂ������
    float magForceX;
    float magForceY;
}
