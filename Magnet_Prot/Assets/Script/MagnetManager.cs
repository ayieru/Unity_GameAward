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

    float magForce;         //���͂��󂯂�I�u�W�F�N�g�ɂ������
    float magForceX;
    float magForceY;
}
